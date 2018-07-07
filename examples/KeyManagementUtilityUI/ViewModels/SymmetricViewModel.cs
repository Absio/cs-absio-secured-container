using System;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Absio.Sdk.Crypto.Cipher;
using KeyManagementUtilityUI.Utils;
using SimpleMvvmToolkit.Express;

namespace KeyManagementUtilityUI.ViewModels
{
    public class SymmetricViewModel : ViewModelBase<SymmetricViewModel>
    {
        private DelegateCommand _decryptCommand;
        private string _decryptInputText;
        private string _decryptIvText;
        private string _decryptKeyText;
        private string _decryptOutputText;
        private DelegateCommand _encryptCommand;
        private string _encryptHexIv;
        private bool _encryptHexIvIsChecked;
        private string _encryptHexKey;
        private bool _encryptHexKeyIsChecked;
        private string _encryptInputText;
        private string _encryptOutputText;
        private string _generatedKey;
        private DelegateCommand _generateKeyCommand;
        private string _generateKeySize;
        private bool _generateWithKeySizeIsChecked;

        public bool GenerateWithKeySizeIsChecked
        {
            get => _generateWithKeySizeIsChecked;
            set
            {
                _generateWithKeySizeIsChecked = value;
                NotifyPropertyChanged(nameof(GenerateWithKeySizeIsChecked));
                _generateKeyCommand.RaiseCanExecuteChanged();
            }
        }

        public string GenerateKeySize
        {
            get => _generateKeySize;
            set
            {
                _generateKeySize = value;
                NotifyPropertyChanged(nameof(GenerateKeySize));
                _generateKeyCommand.RaiseCanExecuteChanged();
            }
        }

        public string GeneratedKey
        {
            get => _generatedKey;
            set
            {
                _generatedKey = value;
                NotifyPropertyChanged(nameof(GeneratedKey));
                _generateKeyCommand.RaiseCanExecuteChanged();
            }
        }

        public ICommand GenerateKeyCommand
        {
            get => _generateKeyCommand ??
                   (_generateKeyCommand = new DelegateCommand(GenerateKeyExecuted,
                       GenerateKeyCanExecute));
        }

        public ICommand EncryptCommand
        {
            get => _encryptCommand ?? (_encryptCommand = new DelegateCommand(EncryptExecuted, EncryptCanExecute));
        }

        public ICommand DecryptCommand
        {
            get => _decryptCommand ?? (_decryptCommand = new DelegateCommand(DecryptExecuted, DecryptCanExecute));
        }

        public bool EncryptHexKeyIsChecked
        {
            get => _encryptHexKeyIsChecked;
            set
            {
                _encryptHexKeyIsChecked = value;
                NotifyPropertyChanged(nameof(EncryptHexIvIsChecked));
                _encryptCommand.RaiseCanExecuteChanged();
            }
        }

        public string EncryptHexKey
        {
            get => _encryptHexKey;
            set
            {
                _encryptHexKey = value;
                NotifyPropertyChanged(nameof(EncryptHexKey));
                _encryptCommand.RaiseCanExecuteChanged();
            }
        }

        public bool EncryptHexIvIsChecked
        {
            get => _encryptHexIvIsChecked;
            set
            {
                _encryptHexIvIsChecked = value;
                NotifyPropertyChanged(nameof(EncryptHexIvIsChecked));
                _encryptCommand.RaiseCanExecuteChanged();
            }
        }

        public string EncryptHexIv
        {
            get => _encryptHexIv;
            set
            {
                _encryptHexIv = value;
                NotifyPropertyChanged(nameof(EncryptHexIv));
                _encryptCommand.RaiseCanExecuteChanged();
            }
        }

        public string EncryptInputText
        {
            get => _encryptInputText;
            set
            {
                _encryptInputText = value;
                NotifyPropertyChanged(nameof(EncryptInputText));
                _encryptCommand.RaiseCanExecuteChanged();
            }
        }

        public string EncryptOutputText
        {
            get => _encryptOutputText;
            set
            {
                _encryptOutputText = value;
                NotifyPropertyChanged(nameof(EncryptOutputText));
                _encryptCommand.RaiseCanExecuteChanged();
            }
        }

        public string DecryptInputText
        {
            get => _decryptInputText;
            set
            {
                _decryptInputText = value;
                NotifyPropertyChanged(nameof(DecryptInputText));
                _decryptCommand.RaiseCanExecuteChanged();
            }
        }

        public string DecryptOutputText
        {
            get => _decryptOutputText;
            set
            {
                _decryptOutputText = value;
                NotifyPropertyChanged(nameof(DecryptOutputText));
                _decryptCommand.RaiseCanExecuteChanged();
            }
        }

        public string DecryptKeyText
        {
            get => _decryptKeyText;
            set
            {
                _decryptKeyText = value;
                NotifyPropertyChanged(nameof(DecryptKeyText));
                _decryptCommand.RaiseCanExecuteChanged();
            }
        }

        public string DecryptIvText
        {
            get => _decryptIvText;
            set
            {
                _decryptIvText = value;
                NotifyPropertyChanged(nameof(DecryptIvText));
                _decryptCommand.RaiseCanExecuteChanged();
            }
        }

        private void GenerateKeyExecuted()
        {
            try
            {
                var cipherHelper = new CipherHelper();
                var keyBytes = GenerateWithKeySizeIsChecked
                    ? cipherHelper.GenerateKey(int.Parse(GenerateKeySize))
                    : cipherHelper.GenerateKey();

                GeneratedKey = Hex.ToHex(keyBytes);
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred while generating the symmetric key. Check inputs and try again.", e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine(e);
            }
        }

        private bool GenerateKeyCanExecute()
        {
            return !GenerateWithKeySizeIsChecked || !string.IsNullOrEmpty(GenerateKeySize);
        }

        private bool EncryptCanExecute()
        {
            var encryptKeyIsValid = !EncryptHexKeyIsChecked || !string.IsNullOrEmpty(EncryptHexKey);
            var encryptIvIsValid = !EncryptHexIvIsChecked || !string.IsNullOrEmpty(EncryptHexIv);
            var encryptInputIsValid = !string.IsNullOrEmpty(EncryptInputText);
            return encryptKeyIsValid && encryptIvIsValid && encryptInputIsValid;
        }

        private bool DecryptCanExecute()
        {
            var decryptKeyIsValid = !string.IsNullOrEmpty(DecryptKeyText);
            var decryptIvIsValid = !string.IsNullOrEmpty(DecryptIvText);
            var decryptInputIsValid = !string.IsNullOrEmpty(DecryptInputText);

            return decryptKeyIsValid && decryptIvIsValid && decryptInputIsValid;
        }

        private async void EncryptExecuted()
        {
            try
            {
                CipherHelper helper = new CipherHelper();
                byte[] key;
                if (EncryptHexKeyIsChecked)
                {
                    key = Hex.FromHex(EncryptHexKey);
                }
                else
                {
                    key = helper.GenerateKey();
                    EncryptHexKey = Hex.ToHex(key);
                }

                byte[] iv;
                if (EncryptHexIvIsChecked)
                {
                    iv = Hex.FromHex(EncryptHexIv);
                }
                else
                {
                    iv = helper.GenerateIV();
                    EncryptHexIv = Hex.ToHex(iv);
                }

                byte[] data = Encoding.UTF8.GetBytes(EncryptInputText);

                byte[] encrypted = await helper.EncryptAsync(key, iv, data);
                EncryptOutputText = Hex.ToHex(encrypted);
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred while encrypting. Check inputs and try again.", e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine(e);
            }
        }

        private async void DecryptExecuted()
        {
            try
            {
                CipherHelper helper = new CipherHelper();
                byte[] key = Hex.FromHex(DecryptKeyText);
                byte[] iv = Hex.FromHex(DecryptIvText);
                byte[] data = Hex.FromHex(DecryptInputText);

                byte[] decrypted = await helper.DecryptAsync(key, iv, data);

                DecryptOutputText = Encoding.UTF8.GetString(decrypted);
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred while decrypting. Check inputs and try again.", e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine(e);
            }
        }
    }
}