using System;
using System.Windows;
using System.Windows.Input;
using Absio.Sdk.Crypto.Ecc;
using Absio.Sdk.Crypto.KeyPair;
using KeyManagementUtilityUI.Utils;
using SimpleMvvmToolkit.Express;

namespace KeyManagementUtilityUI.ViewModels
{
    public class AsymmetricViewModel : ViewModelBase<AsymmetricViewModel>
    {
        private readonly KeyPairHelper _keyPairHelper = new KeyPairHelper();
        private DelegateCommand _generateCommand;
        private string _privateKeyText;
        private string _publicKeyText;

        public ICommand GenerateCommand
        {
            get => _generateCommand ?? (_generateCommand = new DelegateCommand(GenerateExecuted, GenerateCanExecute));
        }

        public string PublicKeyText
        {
            get => _publicKeyText;
            set
            {
                _publicKeyText = value;
                NotifyPropertyChanged(nameof(PublicKeyText));
            }
        }

        public string PrivateKeyText
        {
            get => _privateKeyText;
            set
            {
                _privateKeyText = value;
                NotifyPropertyChanged(nameof(PrivateKeyText));
            }
        }

        private async void GenerateExecuted()
        {
            try
            {
                var keyPair = await _keyPairHelper.GenerateKeyPairAsync(EllipticCurve.Default);
                var publicKey = keyPair.GetPublicKey().ToDer();
                PublicKeyText = Hex.ToHex(publicKey);
                var privateKey = keyPair.ToDer();
                PrivateKeyText = Hex.ToHex(privateKey);
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred while generating the key pair. Check inputs and try again.", e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine(e);
            }
        }

        private bool GenerateCanExecute()
        {
            return true;
        }
    }
}