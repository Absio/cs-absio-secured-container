using System;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Absio.Sdk.Crypto.Mac;
using KeyManagementUtilityUI.Utils;
using SimpleMvvmToolkit.Express;

namespace KeyManagementUtilityUI.ViewModels
{
    public class MacViewModel : ViewModelBase<MacViewModel>
    {
        private readonly MacHelper _macHelper = new MacHelper();
        private DelegateCommand _digestCommand;
        private string _digestInputText;
        private string _digestOutputText;
        private DelegateCommand _generateKeyCommand;
        private string _keyText;
        private DelegateCommand _verifyCommand;
        private string _verifyOutputText;

        public ICommand GenerateKeyCommand
        {
            get => _generateKeyCommand ??
                   (_generateKeyCommand = new DelegateCommand(GenerateKeyExecuted, GenerateKeyCanExecute));
        }

        public string KeyText
        {
            get => _keyText;
            set
            {
                _keyText = value;
                NotifyPropertyChanged(nameof(KeyText));
                _generateKeyCommand.RaiseCanExecuteChanged();
                _digestCommand.RaiseCanExecuteChanged();
                _verifyCommand.RaiseCanExecuteChanged();
            }
        }

        public string DigestInputText
        {
            get => _digestInputText;
            set
            {
                _digestInputText = value;
                NotifyPropertyChanged(nameof(DigestInputText));
                _digestCommand.RaiseCanExecuteChanged();
                _verifyCommand.RaiseCanExecuteChanged();
            }
        }

        public ICommand DigestCommand
        {
            get => _digestCommand ?? (_digestCommand = new DelegateCommand(DigestExecuted, DigestCanExecute));
        }

        public string DigestOutputText
        {
            get => _digestOutputText;
            set
            {
                _digestOutputText = value;
                NotifyPropertyChanged(nameof(DigestOutputText));
                _verifyCommand.RaiseCanExecuteChanged();
            }
        }

        public ICommand VerifyCommand
        {
            get => _verifyCommand ?? (_verifyCommand = new DelegateCommand(VerifyExecuted, VerifyCanExecute));
        }

        public string VerifyOutputText
        {
            get => _verifyOutputText;
            set
            {
                _verifyOutputText = value;
                NotifyPropertyChanged(nameof(VerifyOutputText));
            }
        }

        private bool GenerateKeyCanExecute()
        {
            return true;
        }

        private void GenerateKeyExecuted()
        {
            try
            {
                KeyText = Hex.ToHex(_macHelper.GenerateKey());
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred while generating the key. Check inputs and try again.", e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine(e);
            }
        }

        private bool DigestCanExecute()
        {
            return !string.IsNullOrEmpty(KeyText) && !string.IsNullOrEmpty(DigestInputText);
        }

        private async void DigestExecuted()
        {
            try
            {
                DigestOutputText =
                    Hex.ToHex(await _macHelper.DigestAsync(Hex.FromHex(KeyText),
                        Encoding.UTF8.GetBytes(DigestInputText)));
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred while digesting. Check inputs and try again.", e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine(e);
            }
        }

        private bool VerifyCanExecute()
        {
            return !string.IsNullOrEmpty(KeyText) && !string.IsNullOrEmpty(DigestInputText) &&
                   !string.IsNullOrEmpty(DigestOutputText);
        }

        private async void VerifyExecuted()
        {
            try
            {
                VerifyOutputText = (await _macHelper.VerifyAsync(Hex.FromHex(KeyText),
                    Encoding.UTF8.GetBytes(DigestInputText), Hex.FromHex(DigestOutputText))).ToString();
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred while verifying. Check inputs and try again.", e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine(e);
            }
        }
    }
}