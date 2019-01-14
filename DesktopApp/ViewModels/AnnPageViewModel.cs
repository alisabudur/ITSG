using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Accord.Neuro;
using DesktopApp.Models;
using DesktopApp.Utils;
using HeartChamberIdentification.Services;
using HeartChamberIdentification.Utils;

namespace DesktopApp.ViewModels
{
    public class AnnPageViewModel : PageBaseViewModel
    {
        private ICommand _startTraining;
        private ICommand _startTesting;
        private ICommand _refresh;
        private ICommand _doTestingFolder;
        private AnnService _annService;
        private ImageService _imageService;

        public AnnPageViewModel() : base(new AnnModel())
        {
            _imageService = new ImageService();
        }

        public ICommand StartTrainingCommand =>
            _startTraining ?? (_startTraining = new CommandHandler(StartTraining, _canExecute));

        public ICommand StartTestingCommand =>
            _startTesting ?? (_startTesting = new CommandHandler(StartTesting, _canExecute));

        public ICommand RefreshCommand =>
            _refresh ?? (_refresh = new CommandHandler(Refresh, _canExecute));

        public ICommand DoTestingFolderCommand =>
            _doTestingFolder ?? (_doTestingFolder = new CommandHandler(DoSelectTestingFolder, _canExecute));

        public AnnModel AnnModel
        {
            get { return (AnnModel)Model; }
            set
            {
                Model = value;
                OnPropertyChanged(nameof(AnnModel));
            }
        }

        public void StartTraining()
        {
            var input = _imageService.GetMlInputFromImages(AnnModel.TrainingFolder);
            var output = _imageService.GetMlOutputFromImages(AnnModel.TrainingFolder);

            var inputCount = input[0].Length;
            var network = new ActivationNetwork(
                new BipolarSigmoidFunction(1), 
                inputCount,
                10,
                inputCount);

            _annService = new AnnService(
                network,
                new TerminationCondition { MinError = AnnModel.MinError, NumberOfEpochs = AnnModel.NoOfEpochs },
                AnnModel.LearningRate);

            _annService.Train(input, output);
            AnnModel.IsTrained = true;
        }

        public void StartTesting()
        {
            var testImage = new Bitmap(AnnModel.TestImagePath);
            var testInput = _imageService.GetPixelsFromImage(AnnModel.TestImagePath);
            var testOutput = _annService.Compute(testInput);
            var resultImage = _imageService.AddContourToImage(testImage, testOutput);

            if (File.Exists(AnnModel.ResultImagePath))
                File.Delete(AnnModel.ResultImagePath);

            resultImage.Save(AnnModel.ResultImagePath);
            AnnModel.ResultImageVisibility = Visibility.Visible;
        }

        public void Refresh()
        {
            _annService = null;
            AnnModel = new AnnModel();
        }

        public void DoSelectTestingFolder()
        {
            if (!string.IsNullOrEmpty(AnnModel.TestingFolder))
            {
                // get all images from folder
                StringBuilder sb = new StringBuilder();
                var images = Directory.GetFiles(AnnModel.TestingFolder, "*.*", SearchOption.AllDirectories).Where(f => !f.Contains("-label")).ToList();
                foreach (var imgPath in images)
                {
                    var testImage = new Bitmap(imgPath);
                    var testImageLabeledPath = string.Format(@"{0}/{1}{2}", AnnModel.TestingFolder, Path.GetFileNameWithoutExtension(imgPath) + "-label", Path.GetExtension(imgPath));
                    var testImageLabeledInput = new Bitmap(testImageLabeledPath);

                    // run through network
                    var testInput = _imageService.GetPixelsFromImage(imgPath);
                    var testOutput = _annService.Compute(testInput);
                    var resultImage = _imageService.AddContourToImage(testImage, testOutput);

                    // do results comparison
                    var successfullyMatchedPixels = 0;
                    var unsuccessfullyMatchedPixels = 0;
                    for (var x = 0; x < testImageLabeledInput.Width; x++)
                    {
                        for (var y = 0; y < testImageLabeledInput.Height; y++)
                        {
                            var labeledImagePixel = testImageLabeledInput.GetPixel(x, y);
                            var resultImagePixel = resultImage.GetPixel(x, y);

                            var isLabeled = _imageService.IsPartOfContour(labeledImagePixel);
                            var isDetected = resultImagePixel.R == Color.DarkOrange.R && resultImagePixel.G == Color.DarkOrange.G && resultImagePixel.B == Color.DarkOrange.B;
                            if (isLabeled && isDetected)
                            {
                                successfullyMatchedPixels++;
                            }

                            if (isLabeled && !isDetected)
                            {
                                unsuccessfullyMatchedPixels++;
                            }
                        }
                    }

                    double successRate = (successfullyMatchedPixels * 100) / (double)(successfullyMatchedPixels + unsuccessfullyMatchedPixels);
                    sb.AppendLine(String.Format(@"Image {0}, matched pixels: {1:0.00} %", Path.GetFileName(imgPath), successRate));
                }

                string finalResults = sb.ToString();
                MessageBox.Show(finalResults);
            }
        }
    }
}
