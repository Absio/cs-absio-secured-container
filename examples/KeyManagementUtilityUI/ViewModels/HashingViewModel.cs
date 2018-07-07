using System;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Absio.Sdk.Crypto.Digest;
using KeyManagementUtilityUI.Utils;
using SimpleMvvmToolkit.Express;

namespace KeyManagementUtilityUI.ViewModels
{
    public class HashingViewModel : ViewModelBase<HashingViewModel>
    {
        private readonly MessageDigestHelper _messageDigestHelper = new MessageDigestHelper();
        private DelegateCommand _hashCommand;
        private string _hashInputText;
        private string _hashOutputText;

        public ICommand HashCommand
        {
            get => _hashCommand ?? (_hashCommand = new DelegateCommand(HashExecuted, HashCanExecute));
        }

        public string HashOutputText
        {
            get => _hashOutputText;
            set
            {
                _hashOutputText = value;
                NotifyPropertyChanged(nameof(HashOutputText));
                _hashCommand.RaiseCanExecuteChanged();
            }
        }

        public string HashInputText
        {
            get => _hashInputText;
            set
            {
                _hashInputText = value;
                NotifyPropertyChanged(nameof(HashInputText));
                _hashCommand.RaiseCanExecuteChanged();
            }
        }

        private bool HashCanExecute()
        {
            return !string.IsNullOrEmpty(HashInputText);
        }

        private async void HashExecuted()
        {
            try
            {
                var bytes = Encoding.UTF8.GetBytes(HashInputText);
                var messageDigestHelper = _messageDigestHelper;
                var result = await messageDigestHelper.DigestAsync(bytes);
                HashOutputText = Hex.ToHex(result);
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred while hashing. Check inputs and try again.", e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine(e);
            }
        }
    }
}