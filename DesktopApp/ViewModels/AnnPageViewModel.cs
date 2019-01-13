﻿using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Accord.Neuro;
using DesktopApp.Models;
using HeartChamberIdentification.Services;
using MyMachineLearning.Models;

namespace DesktopApp.ViewModels
{
    public class AnnPageViewModel: PageBaseViewModel
    {
        private ICommand _startTraining;
        private ICommand _startTesting;
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

        public AnnModel AnnModel
        {
            get { return (AnnModel) Model; }
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
                new SigmoidFunction(1), 
                inputCount, inputCount / 2, 
                inputCount / 2, 
                inputCount / 2, 
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
            var resultImage = _imageService.GetImageFromPixels(testOutput, testImage.Width, testImage.Height);

            if(File.Exists(AnnModel.ResultImagePath))
                File.Delete(AnnModel.ResultImagePath);

            resultImage.Save(AnnModel.ResultImagePath);
            AnnModel.ResultImageVisibility = Visibility.Visible;
        }
    }
}