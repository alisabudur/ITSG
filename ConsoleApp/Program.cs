using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using Accord.Neuro;
using HeartChamberIdentification.Services;
using HeartChamberIdentification.Utils;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopWatch1 = new Stopwatch();
            var imageService = new ImageService();
            var trainingPath =
                "E:\\Master\\Semestrul 3\\Intelligent tools for social goods\\Dataset\\Cropped\\Training";

            var testingPath =
                "E:\\Master\\Semestrul 3\\Intelligent tools for social goods\\Dataset\\Cropped\\Testing";

            var resultPath = "E:\\Master\\Semestrul 3\\Intelligent tools for social goods\\Results";

            stopWatch1.Start();
            var input = imageService.GetMlInputFromImages(trainingPath);
            var output = imageService.GetMlOutputFromImages(trainingPath);

            var inputCount = input[0].Length;
            var network = new ActivationNetwork(new SigmoidFunction(1), inputCount, inputCount / 2, inputCount / 2, inputCount / 2, inputCount);
            var annService = new AnnService(
                network,
                new TerminationCondition { MinError = 0.01, NumberOfEpochs = 1 },
                0.01);

            annService.Train(input, output);

            var testImageName = "training_axial_crop_pat8.jpg";
            var testImagePath = Path.Combine(testingPath, testImageName);
            var testImage = new Bitmap(testImagePath);

            var testInput = imageService.GetPixelsFromImage(testImagePath);
            var testOutput = annService.Compute(testInput);
            var resultImage = imageService.GetImageFromPixels(testOutput, testImage.Width, testImage.Height);
            resultImage.Save(Path.Combine(resultPath, testImageName));

            stopWatch1.Stop();
            Console.WriteLine($"Finished: {stopWatch1.ElapsedMilliseconds}ms");
        }
    }
}
