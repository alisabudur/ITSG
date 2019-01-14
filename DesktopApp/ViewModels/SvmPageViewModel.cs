using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Input;
using DesktopApp.Models;
using DesktopApp.Utils;
using HeartChamberIdentification.Services;

namespace DesktopApp.ViewModels
{
    public class SvmPageViewModel: PageBaseViewModel
    {
        private ICommand _startTraining;
        private ICommand _startTesting;
        private SvmService _svmService;
        private ImageService _imageService;

        public SvmPageViewModel() : base(new SvmModel())
        {
            _imageService = new ImageService();
        }

        public ICommand StartTrainingCommand =>
            _startTraining ?? (_startTraining = new CommandHandler(StartTraining, _canExecute));

        public ICommand StartTestingCommand =>
            _startTesting ?? (_startTesting = new CommandHandler(StartTesting, _canExecute));

        public SvmModel SvmModel
        {
            get { return (SvmModel)Model; }
            set
            {
                Model = value;
                OnPropertyChanged(nameof(SvmModel));
            }
        }

        public void StartTraining()
        {
            var input = _imageService.GetMlInputFromImages(SvmModel.TrainingFolder);
            var output = _imageService.GetMlOutputFromImages(SvmModel.TrainingFolder);
            _svmService = new SvmService();
            _svmService.Train(input, output);

            SvmModel.IsTrained = true;
        }

        public void StartTesting()
        {
            var testImage = new Bitmap(SvmModel.TestImagePath);
            var testInput = _imageService.GetPixelsFromImage(SvmModel.TestImagePath);
            var testOutput = _svmService.Compute(testInput);
            var resultImage = _imageService.AddContourToImage(testImage, testOutput);

            if (File.Exists(SvmModel.ResultImagePath))
                File.Delete(SvmModel.ResultImagePath);

            resultImage.Save(SvmModel.ResultImagePath);
            SvmModel.ResultImageVisibility = Visibility.Visible;
        }
    }
}
