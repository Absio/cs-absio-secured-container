using System;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Absio.Sdk.Crypto.Kdf;
using KeyManagementUtilityUI.Utils;
using SimpleMvvmToolkit.Express;

namespace KeyManagementUtilityUI.ViewModels
{
    public class Kdf2ViewModel : ViewModelBase<Kdf2ViewModel>
    {
        private readonly Kdf2Helper _kdf2Helper = new Kdf2Helper();
        private DelegateCommand _deriveCommand;
        private string _inputHex;
        private bool _inputHexIsChecked;
        private string _inputText;
        private string _keySize;
        private string _outputText;

        public string InputText
        {
            get => _inputText;
            set
            {
                _inputText = value;
                NotifyPropertyChanged(nameof(InputText));
                _deriveCommand.RaiseCanExecuteChanged();
            }
        }

        public bool InputHexIsChecked
        {
            get => _inputHexIsChecked;
            set
            {
                _inputHexIsChecked = value;
                NotifyPropertyChanged(nameof(InputHexIsChecked));
                _deriveCommand.RaiseCanExecuteChanged();
            }
        }

        public string InputHex
        {
            get => _inputHex;
            set
            {
                _inputHex = value;
                NotifyPropertyChanged(nameof(InputHex));
                _deriveCommand.RaiseCanExecuteChanged();
            }
        }

        public string KeySize
        {
            get => _keySize;
            set
            {
                _keySize = value;
                NotifyPropertyChanged(nameof(KeySize));
                _deriveCommand.RaiseCanExecuteChanged();
            }
        }

        public ICommand DeriveCommand =>
            _deriveCommand ?? (_deriveCommand = new DelegateCommand(DeriveExecuted, DeriveCanExecute));

        public string OutputText
        {
            get => _outputText;
            set
            {
                _outputText = value;
                NotifyPropertyChanged(nameof(OutputText));
                _deriveCommand.RaiseCanExecuteChanged();
            }
        }

        private bool DeriveCanExecute()
        {
            var inputIsValid = InputHexIsChecked ? !string.IsNullOrEmpty(InputHex) : !string.IsNullOrEmpty(InputText);
            var keySizeIsValid = int.TryParse(KeySize, out _);

            return inputIsValid && keySizeIsValid;
        }

        private async void DeriveExecuted()
        {
            try
            {
                var input = InputHexIsChecked ? Hex.FromHex(InputHex) : Encoding.UTF8.GetBytes(InputText);
                var keySize = int.Parse(KeySize);

                byte[] result = await _kdf2Helper.DeriveKeyAsync(input, keySize);
                OutputText = Hex.ToHex(result);
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred while deriving. Check inputs and try again.", e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine(e);
            }
        }
    }
}