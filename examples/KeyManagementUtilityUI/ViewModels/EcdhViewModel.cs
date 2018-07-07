using System;
using System.Windows;
using System.Windows.Input;
using Absio.Sdk.Crypto;
using Absio.Sdk.Crypto.Ecc;
using Absio.Sdk.Crypto.KeyAgreement;
using Absio.Sdk.Crypto.KeyPair;
using KeyManagementUtilityUI.Utils;
using SimpleMvvmToolkit.Express;

namespace KeyManagementUtilityUI.ViewModels
{
    public class EcdhViewModel : ViewModelBase<EcdhViewModel>
    {
        private readonly KeyAgreementHelper _keyAgreementHelper = new KeyAgreementHelper();
        private readonly KeyPairHelper _keyPairHelper = new KeyPairHelper();
        private DelegateCommand _deriveCommand;
        private string _derivedOutputText;
        private bool _privateKeyInputRadioIsChecked;
        private string _privateKeyText;
        private bool _publicKeyInputRadioIsChecked;
        private string _publicKeyText;

        public bool PublicKeyInputRadioIsChecked
        {
            get => _publicKeyInputRadioIsChecked;
            set
            {
                _publicKeyInputRadioIsChecked = value;
                NotifyPropertyChanged(nameof(PublicKeyInputRadioIsChecked));
                _deriveCommand.RaiseCanExecuteChanged();
            }
        }

        public string PublicKeyText
        {
            get => _publicKeyText;
            set
            {
                _publicKeyText = value;
                NotifyPropertyChanged(nameof(PublicKeyText));
                _deriveCommand.RaiseCanExecuteChanged();
            }
        }

        public bool PrivateKeyInputRadioIsChecked
        {
            get => _privateKeyInputRadioIsChecked;
            set
            {
                _privateKeyInputRadioIsChecked = value;
                NotifyPropertyChanged(nameof(PrivateKeyInputRadioIsChecked));
                _deriveCommand.RaiseCanExecuteChanged();
            }
        }

        public string PrivateKeyText
        {
            get => _privateKeyText;
            set
            {
                _privateKeyText = value;
                NotifyPropertyChanged(nameof(PrivateKeyText));
                _deriveCommand.RaiseCanExecuteChanged();
            }
        }

        public string DerivedOutputText
        {
            get => _derivedOutputText;
            set
            {
                _derivedOutputText = value;
                NotifyPropertyChanged(nameof(DerivedOutputText));
                _deriveCommand.RaiseCanExecuteChanged();
            }
        }

        public ICommand DeriveCommand =>
            _deriveCommand ?? (_deriveCommand = new DelegateCommand(DeriveExecuted, DeriveCanExecute));

        private async void DeriveExecuted()
        {
            try
            {
                EcKey alicePublicKey;
                if (PublicKeyInputRadioIsChecked)
                {
                    alicePublicKey = await _keyPairHelper.PublicKeyFromDerAsync(Hex.FromHex(PublicKeyText));
                }
                else
                {
                    var aliceKeyPair = await _keyPairHelper.GenerateKeyPairAsync(EllipticCurve.Default);
                    alicePublicKey = aliceKeyPair.GetPublicKey();
                    PublicKeyText = Hex.ToHex(alicePublicKey.ToDer());
                }

                EcKey bobPrivateKey;
                if (PrivateKeyInputRadioIsChecked)
                {
                    bobPrivateKey = await _keyPairHelper.PrivateKeyFromDerAsync(Hex.FromHex(PrivateKeyText));
                }
                else
                {
                    bobPrivateKey = await _keyPairHelper.GenerateKeyPairAsync(EllipticCurve.Default);
                    PrivateKeyText = Hex.ToHex(bobPrivateKey.ToDer());
                }

                var helper = _keyAgreementHelper;
                var sharedSecret = await helper.GeneratedSharedKeyAsync(bobPrivateKey, alicePublicKey);
                DerivedOutputText = Hex.ToHex(sharedSecret);
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred while deriving. Check inputs and try again.", e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine(e);
            }
        }

        private bool DeriveCanExecute()
        {
            var alicePubKeyValid = !PublicKeyInputRadioIsChecked || !string.IsNullOrEmpty(PublicKeyText);
            var bobPriKeyValid = !PrivateKeyInputRadioIsChecked || !string.IsNullOrEmpty(PrivateKeyText);

            return alicePubKeyValid && bobPriKeyValid;
        }
    }
}