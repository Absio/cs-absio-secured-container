using System;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Absio.Sdk.Crypto;
using Absio.Sdk.Crypto.Ecc;
using Absio.Sdk.Crypto.KeyPair;
using Absio.Sdk.Crypto.Signature;
using KeyManagementUtilityUI.Utils;
using SimpleMvvmToolkit.Express;

namespace KeyManagementUtilityUI.ViewModels
{
    public class EcdsaViewModel : ViewModelBase<EcdsaViewModel>
    {
        private readonly KeyPairHelper _keyPairHelper = new KeyPairHelper();
        private readonly SignatureHelper _signatureHelper = new SignatureHelper();
        private string _dataText;
        private bool _privateKeyInputRadioIsChecked;
        private string _privateKeyText;
        private string _publicKeyText;
        private string _signatureOutputText;
        private DelegateCommand _signCommand;
        private DelegateCommand _verifyCommand;
        private string _verifyOutputText;

        public bool PrivateKeyInputRadioIsChecked
        {
            get => _privateKeyInputRadioIsChecked;
            set
            {
                _privateKeyInputRadioIsChecked = value;
                NotifyPropertyChanged(nameof(PrivateKeyInputRadioIsChecked));
                _signCommand.RaiseCanExecuteChanged();
                _verifyCommand.RaiseCanExecuteChanged();
            }
        }

        public string PrivateKeyText
        {
            get => _privateKeyText;
            set
            {
                _privateKeyText = value;
                NotifyPropertyChanged(nameof(PrivateKeyText));
                _signCommand.RaiseCanExecuteChanged();
                _verifyCommand.RaiseCanExecuteChanged();
            }
        }

        public string DataText
        {
            get => _dataText;
            set
            {
                _dataText = value;
                NotifyPropertyChanged(nameof(DataText));
                _signCommand.RaiseCanExecuteChanged();
                _verifyCommand.RaiseCanExecuteChanged();
            }
        }

        public string SignatureOutputText
        {
            get => _signatureOutputText;
            set
            {
                _signatureOutputText = value;
                NotifyPropertyChanged(nameof(SignatureOutputText));
                _signCommand.RaiseCanExecuteChanged();
                _verifyCommand.RaiseCanExecuteChanged();
            }
        }

        public string PublicKeyText
        {
            get => _publicKeyText;
            set
            {
                _publicKeyText = value;
                NotifyPropertyChanged(nameof(PublicKeyText));
                _signCommand.RaiseCanExecuteChanged();
                _verifyCommand.RaiseCanExecuteChanged();
            }
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

        public ICommand SignCommand =>
            _signCommand ?? (_signCommand = new DelegateCommand(SignExecuted, SignCanExecute));

        public ICommand VerifyCommand =>
            _verifyCommand ?? (_verifyCommand = new DelegateCommand(VerifyExecuted, VerifyCanExecute));

        private bool SignCanExecute()
        {
            var privateKeyIsValid = !PrivateKeyInputRadioIsChecked || !string.IsNullOrEmpty(PrivateKeyText);
            var dataIsValid = !string.IsNullOrEmpty(DataText);

            return privateKeyIsValid && dataIsValid;
        }

        private async void SignExecuted()
        {
            try
            {
                EcKey privateKey;
                if (PrivateKeyInputRadioIsChecked)
                {
                    privateKey = await _keyPairHelper.PrivateKeyFromDerAsync(Hex.FromHex(PrivateKeyText));
                }
                else
                {
                    privateKey = await _keyPairHelper.GenerateKeyPairAsync(EllipticCurve.Default);
                    PrivateKeyText = Hex.ToHex(privateKey.ToDer());
                    PublicKeyText = Hex.ToHex(privateKey.GetPublicKey().ToDer());
                }

                SignatureOutputText =
                    Hex.ToHex(await _signatureHelper.SignAsync(privateKey, Encoding.UTF8.GetBytes(DataText)));
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred while signing. Check inputs and try again.", e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine(e);
            }
        }

        private async void VerifyExecuted()
        {
            try
            {
                var verified = await _signatureHelper.VerifyAsync(Hex.FromHex(PublicKeyText),
                    Encoding.UTF8.GetBytes(DataText), Hex.FromHex(SignatureOutputText));
                VerifyOutputText = verified.ToString();
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred while verifying. Check inputs and try again.", e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine(e);
            }
        }

        private bool VerifyCanExecute()
        {
            var publicKeyIsValid = !string.IsNullOrEmpty(PublicKeyText);
            var dataIsValid = !string.IsNullOrEmpty(DataText);
            var signatureIsValid = !string.IsNullOrEmpty(SignatureOutputText);

            return publicKeyIsValid && dataIsValid && signatureIsValid;
        }
    }
}