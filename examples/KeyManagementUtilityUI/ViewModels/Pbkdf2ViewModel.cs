using System;
using System.Windows;
using System.Windows.Input;
using Absio.Sdk.Crypto;
using KeyManagementUtilityUI.Utils;
using SimpleMvvmToolkit.Express;

namespace KeyManagementUtilityUI.ViewModels
{
    public class Pbkdf2ViewModel : ViewModelBase<Pbkdf2ViewModel>
    {
        private readonly Pbkdf2Helper _pbkdf2Helper = new Pbkdf2Helper();
        private DelegateCommand _deriveCommand;
        private string _iterationsText;
        private string _keySizeText;
        private string _outputText;
        private string _passwordText;
        private string _saltHex;
        private bool _saltHexIsChecked;

        public string PasswordText
        {
            get => _passwordText;
            set
            {
                _passwordText = value;
                NotifyPropertyChanged(nameof(PasswordText));
                _deriveCommand.RaiseCanExecuteChanged();
            }
        }

        public bool SaltHexIsChecked
        {
            get => _saltHexIsChecked;
            set
            {
                _saltHexIsChecked = value;
                NotifyPropertyChanged(nameof(SaltHexIsChecked));
                _deriveCommand.RaiseCanExecuteChanged();
            }
        }

        public string SaltHex
        {
            get => _saltHex;
            set
            {
                _saltHex = value;
                NotifyPropertyChanged(nameof(SaltHex));
                _deriveCommand.RaiseCanExecuteChanged();
            }
        }

        public string KeySizeText
        {
            get => _keySizeText;
            set
            {
                _keySizeText = value;
                NotifyPropertyChanged(nameof(KeySizeText));
                _deriveCommand.RaiseCanExecuteChanged();
            }
        }

        public string IterationsText
        {
            get => _iterationsText;
            set
            {
                _iterationsText = value;
                NotifyPropertyChanged(nameof(IterationsText));
                _deriveCommand.RaiseCanExecuteChanged();
            }
        }

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

        public ICommand DeriveCommand =>
            _deriveCommand ?? (_deriveCommand = new DelegateCommand(DeriveExecuted, DeriveCanExecute));

        private bool DeriveCanExecute()
        {
            var passwordValid = !string.IsNullOrEmpty(PasswordText);
            var saltValid = !SaltHexIsChecked || !string.IsNullOrEmpty(SaltHex);
            var iterationsValid = int.TryParse(IterationsText, out _);

            return passwordValid && saltValid && iterationsValid;
        }

        private void DeriveExecuted()
        {
            try
            {
                byte[] salt;
                if (SaltHexIsChecked)
                {
                    salt = Hex.FromHex(SaltHex);
                }
                else
                {
                    salt = CryptoUtils.GetRandomBytes(16);
                    SaltHex = Hex.ToHex(salt);
                }

                byte[] key = _pbkdf2Helper.GenerateDerivedKey(PasswordText, salt, int.Parse(IterationsText));
                OutputText = Hex.ToHex(key);
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred while deriving. Check inputs and try again.", e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine(e);
            }
        }
    }
}