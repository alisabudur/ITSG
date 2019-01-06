using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Accord.Neuro;
using HeartChamberIdentification.DAL;
using HeartChamberIdentification.Extensions;
using HeartChamberIdentification.Services;
using MyMachineLearning.Models;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new PixelDbContex())
            {
                const int datasetCount = 3000;
                var trainData = db.Pixels.Where(x => x.ImageName.Contains("training_axial_crop_pat2"))
                    .ToArray();

                var network = new ActivationNetwork(new SigmoidFunction(), 2, 2, 2, 2, 1);
                var terminationCondition = new TerminationCondition
                {
                    MinError = 0.001,
                    NumberOfEpochs = 10000
                };

                var annService = new AnnService(
                    network,
                    terminationCondition);

                var normalizedTrainData = trainData.Normalize().ToList();
                var tdInContour = normalizedTrainData.Where(x => x.IsContour).Take(datasetCount).ToList();
                var tdOutContour = normalizedTrainData.Where(x => !x.IsContour).Take(datasetCount).ToList();
                var td = new List<Pixel>();

                for (var i = 0; i < datasetCount; i++)
                {
                    td.Add(tdInContour[i]);
                    td.Add(tdOutContour[i]);
                }

                annService.Train(td);

                var datasetPath =
                    @"E:\Master\Semestrul 3\Intelligent tools for social goods\Dataset\Cropped";
                var testImageName = "training_axial_crop_pat2.jpg";
                var testImagePath = Path.Combine(datasetPath, testImageName);

                var testData = db.Pixels.Where(x => x.ImageName.Contains(testImageName)).ToArray();
                var normalizedTestData = testData.Normalize().ToArray();
                var annPerformance = (BinaryClassificationPerformanceModel)annService.GetNetworkPerformane(normalizedTestData);

                Console.WriteLine($"Precision = {annPerformance.Precision}");
                Console.WriteLine($"Recall = {annPerformance.Recall}");
                Console.WriteLine($"Accuracy = {annPerformance.Accuracy}");

                using (var testImage = new Bitmap(testImagePath))
                using (var resultImage = annService.GetImageContour(testImage, normalizedTestData))
                {
                    var resultsPath = @"E:\Master\Semestrul 3\Intelligent tools for social goods\Results";
                    var resultImageName = "training_axial_crop_pat2_result.jpg";
                    var resultImagePath = Path.Combine(resultsPath, resultImageName);

                    if(File.Exists(resultImagePath)) 
                        File.Delete(resultImagePath);

                    resultImage.Save(resultImagePath, ImageFormat.Jpeg);
                }
            }
        }
    }
}
