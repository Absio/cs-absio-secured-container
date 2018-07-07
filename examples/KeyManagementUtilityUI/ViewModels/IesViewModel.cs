using System;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Absio.Sdk.Crypto;
using Absio.Sdk.Crypto.Ecc;
using Absio.Sdk.Crypto.KeyPair;
using Absio.Sdk.Crypto.Keys;
using KeyManagementUtilityUI.Utils;
using SimpleMvvmToolkit.Express;

namespace KeyManagementUtilityUI.ViewModels
{
    public class IesViewModel : ViewModelBase<IesViewModel>
    {
        private readonly AbsioIesHelper _absioIesHelper = new AbsioIesHelper();
        private readonly KeyPairHelper _keyPairHelper = new KeyPairHelper();
        private bool _aliceKeyInputRadioIsChecked;
        private string _alicePrivateKeyText;
        private string _alicePublicKeyText;
        private bool _bobKeyInputRadioIsChecked;
        private string _bobPrivateKeyText;
        private string _bobPublicKeyText;
        private DelegateCommand _decryptCommand;
        private string _decryptedObjectIdText;
        private string _decryptedOutputText;
        private string _decryptedSenderIdText;
        private DelegateCommand _encryptCommand;
        private string _encryptedOutputText;
        private string _objectIdText;
        private string _senderIdText;
        private string _textInputText;

        public bool AliceKeyInputRadioIsChecked
        {
            get => _aliceKeyInputRadioIsChecked;
            set
            {
                _aliceKeyInputRadioIsChecked = value;
                NotifyPropertyChanged(nameof(AliceKeyInputRadioIsChecked));
                _encryptCommand.RaiseCanExecuteChanged();
                _decryptCommand.RaiseCanExecuteChanged();
            }
        }

        public string AlicePrivateKeyText
        {
            get => _alicePrivateKeyText;
            set
            {
                _alicePrivateKeyText = value;
                NotifyPropertyChanged(nameof(AlicePrivateKeyText));
                _encryptCommand.RaiseCanExecuteChanged();
                _decryptCommand.RaiseCanExecuteChanged();
            }
        }

        public bool BobKeyInputRadioIsChecked
        {
            get => _bobKeyInputRadioIsChecked;
            set
            {
                _bobKeyInputRadioIsChecked = value;
                NotifyPropertyChanged(nameof(BobKeyInputRadioIsChecked));
                _encryptCommand.RaiseCanExecuteChanged();
                _decryptCommand.RaiseCanExecuteChanged();
            }
        }

        public string BobPublicKeyText
        {
            get => _bobPublicKeyText;
            set
            {
                _bobPublicKeyText = value;
                NotifyPropertyChanged(nameof(BobPublicKeyText));
                _encryptCommand.RaiseCanExecuteChanged();
                _decryptCommand.RaiseCanExecuteChanged();
            }
        }

        public string TextInputText
        {
            get => _textInputText;
            set
            {
                _textInputText = value;
                NotifyPropertyChanged(nameof(TextInputText));
                _encryptCommand.RaiseCanExecuteChanged();
                _decryptCommand.RaiseCanExecuteChanged();
            }
        }

        public string SenderIdText
        {
            get => _senderIdText;
            set
            {
                _senderIdText = value;
                NotifyPropertyChanged(nameof(SenderIdText));
                _encryptCommand.RaiseCanExecuteChanged();
                _decryptCommand.RaiseCanExecuteChanged();
            }
        }

        public string ObjectIdText
        {
            get => _objectIdText;
            set
            {
                _objectIdText = value;
                NotifyPropertyChanged(nameof(ObjectIdText));
                _encryptCommand.RaiseCanExecuteChanged();
                _decryptCommand.RaiseCanExecuteChanged();
            }
        }

        public ICommand EncryptCommand =>
            _encryptCommand ?? (_encryptCommand = new DelegateCommand(EncryptExecuted, EncryptCanExecute));

        public string EncryptedOutputText
        {
            get => _encryptedOutputText;
            set
            {
                _encryptedOutputText = value;
                NotifyPropertyChanged(nameof(EncryptedOutputText));
                _encryptCommand.RaiseCanExecuteChanged();
                _decryptCommand.RaiseCanExecuteChanged();
            }
        }

        public string AlicePublicKeyText
        {
            get => _alicePublicKeyText;
            set
            {
                _alicePublicKeyText = value;
                NotifyPropertyChanged(nameof(AlicePublicKeyText));
                _encryptCommand.RaiseCanExecuteChanged();
                _decryptCommand.RaiseCanExecuteChanged();
            }
        }

        public string BobPrivateKeyText
        {
            get => _bobPrivateKeyText;
            set
            {
                _bobPrivateKeyText = value;
                NotifyPropertyChanged(nameof(BobPrivateKeyText));
                _encryptCommand.RaiseCanExecuteChanged();
                _decryptCommand.RaiseCanExecuteChanged();
            }
        }

        public ICommand DecryptCommand =>
            _decryptCommand ?? (_decryptCommand = new DelegateCommand(DecryptExecuted, DecryptCanExecute));

        public string DecryptedOutputText
        {
            get => _decryptedOutputText;
            set
            {
                _decryptedOutputText = value;
                NotifyPropertyChanged(nameof(DecryptedOutputText));
                _encryptCommand.RaiseCanExecuteChanged();
                _decryptCommand.RaiseCanExecuteChanged();
            }
        }

        public string DecryptedSenderIdText
        {
            get => _decryptedSenderIdText;
            set
            {
                _decryptedSenderIdText = value;
                NotifyPropertyChanged(nameof(DecryptedSenderIdText));
                _encryptCommand.RaiseCanExecuteChanged();
                _decryptCommand.RaiseCanExecuteChanged();
            }
        }

        public string DecryptedObjectIdText
        {
            get => _decryptedObjectIdText;
            set
            {
                _decryptedObjectIdText = value;
                NotifyPropertyChanged(nameof(DecryptedObjectIdText));
                _encryptCommand.RaiseCanExecuteChanged();
                _decryptCommand.RaiseCanExecuteChanged();
            }
        }

        private bool EncryptCanExecute()
        {
            var alicePrivateKeyIsValid = !AliceKeyInputRadioIsChecked || !string.IsNullOrEmpty(AlicePrivateKeyText);
            var bobPublicKeyIsValid = !BobKeyInputRadioIsChecked || !string.IsNullOrEmpty(BobPublicKeyText);
            var inputIsValid = !string.IsNullOrEmpty(TextInputText);

            return alicePrivateKeyIsValid && bobPublicKeyIsValid && inputIsValid;
        }

        private async void EncryptExecuted()
        {
            try
            {
                EcKey alicePrivateKey;
                if (AliceKeyInputRadioIsChecked)
                {
                    alicePrivateKey = await _keyPairHelper.PrivateKeyFromDerAsync(Hex.FromHex(AlicePrivateKeyText));
                }
                else
                {
                    alicePrivateKey = await _keyPairHelper.GenerateKeyPairAsync(EllipticCurve.Default);
                    AlicePrivateKeyText = Hex.ToHex(alicePrivateKey.ToDer());
                    AlicePublicKeyText = Hex.ToHex(alicePrivateKey.GetPublicKey().ToDer());
                }

                EcKey bobPublicKey;
                if (BobKeyInputRadioIsChecked)
                {
                    bobPublicKey = await _keyPairHelper.PublicKeyFromDerAsync(Hex.FromHex(BobPublicKeyText));
                }
                else
                {
                    var bobKeyPair = await _keyPairHelper.GenerateKeyPairAsync(EllipticCurve.Default);
                    bobPublicKey = bobKeyPair.GetPublicKey();
                    BobPrivateKeyText = Hex.ToHex(bobKeyPair.ToDer());
                    BobPublicKeyText = Hex.ToHex(bobPublicKey.ToDer());
                }

                var helper = _absioIesHelper;
                var signingPrivateKey = new SigningKey
                {
                    Active = true,
                    Bytes = alicePrivateKey.ToDer(),
                    Index = 0,
                    IsPublic = false
                };
                var derivationPublicKey = new DerivationKey
                {
                    Active = true,
                    Bytes = bobPublicKey.ToDer(),
                    Index = 0,
                    IsPublic = true
                };
                var senderId = string.IsNullOrEmpty(SenderIdText) ? Guid.NewGuid() : Guid.Parse(SenderIdText);
                SenderIdText = senderId.ToString();
                var objectId = string.IsNullOrEmpty(ObjectIdText) ? Guid.NewGuid() : Guid.Parse(ObjectIdText);
                ObjectIdText = objectId.ToString();
                byte[] encrypted = await helper.EncryptAsync(Encoding.UTF8.GetBytes(TextInputText), signingPrivateKey,
                    derivationPublicKey,
                    senderId, objectId);
                EncryptedOutputText = Hex.ToHex(encrypted);
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred while encrypting. Check inputs and try again.", e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine(e);
            }
        }

        private bool DecryptCanExecute()
        {
            var alicePublicKeyIsValid = !string.IsNullOrEmpty(AlicePublicKeyText);
            var bobPrivateKeyIsValid = !string.IsNullOrEmpty(BobPrivateKeyText);
            var encryptedIsValid = !string.IsNullOrEmpty(TextInputText);

            return alicePublicKeyIsValid && bobPrivateKeyIsValid && encryptedIsValid;
        }

        private async void DecryptExecuted()
        {
            try
            {
                EcKey alicePublicKey = await _keyPairHelper.PublicKeyFromDerAsync(Hex.FromHex(AlicePublicKeyText));
                EcKey bobPrivateKey = await _keyPairHelper.PrivateKeyFromDerAsync(Hex.FromHex(BobPrivateKeyText));

                var helper = _absioIesHelper;
                var signingPublicKey = new SigningKey
                {
                    Active = true,
                    Bytes = alicePublicKey.ToDer(),
                    Index = 0,
                    IsPublic = true
                };
                var derivationPrivateKey = new DerivationKey
                {
                    Active = true,
                    Bytes = bobPrivateKey.ToDer(),
                    Index = 0,
                    IsPublic = true
                };

                DecryptedEccData decrypted = await helper.DecryptAsync(Hex.FromHex(EncryptedOutputText), signingPublicKey,
                    derivationPrivateKey);

                DecryptedSenderIdText = decrypted.Sender.ToString();
                DecryptedObjectIdText = decrypted.Id.ToString();
                DecryptedOutputText = Encoding.UTF8.GetString(decrypted.Data);
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred while decrypting. Check inputs and try again.", e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine(e);
            }
        }
    }
}