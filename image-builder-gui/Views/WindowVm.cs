using System;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ImageBuilder.Services;

namespace ImageBuilder
{
    public class WindowVm : ViewModelBase
    {
        private readonly ImageBuilderService _imageBuilderService;
        private bool _isBusy = false;

        public WindowVm()
        {
            _imageBuilderService = new ImageBuilderService();

            BuildImageCommand = new RelayCommand(async () => await BuildImage());
        }

        private async Task BuildImage()
        {
            IsBusy = true;

            await _imageBuilderService.BuildImageAsync();

            IsBusy = false;
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public ICommand BuildImageCommand { get; }
    }
}
