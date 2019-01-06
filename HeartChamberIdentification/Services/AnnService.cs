using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Accord.Neuro;
using HeartChamberIdentification.DAL;
using HeartChamberIdentification.Extensions;
using MyMachineLearning.Extensions;
using MyMachineLearning.Interfaces;
using MyMachineLearning.Models;
using MyMachineLearning.Services;

namespace HeartChamberIdentification.Services
{
    public class AnnService : AnnServiceBase<Pixel>
    {
        public AnnService(
            ActivationNetwork network,
            TerminationCondition terminationCondition)
        : base(network, terminationCondition)
        {
        }

        /// <summary>
        /// Gets the performance of Artificial Neural Network previously trained.
        /// </summary>
        /// <param name="testData">Represents the testing data set.</param>
        /// <returns>Returns the performance result.</returns>
        public override IPerformance GetNetworkPerformane(IEnumerable<Pixel> testData)
        {
            var ioModel = testData.GetInputOutputModel();
            var input = ioModel.Input;

            var actualOutput = new List<int>();
            var expectedOutput = ioModel.Output.Select(x => (int)x[0]).ToArray();

            foreach (var inputItem in input)
            {
                var outputItem = Network.Compute(inputItem);
                actualOutput.Add(outputItem[0] >= 0.99 ? 1 : 0);
            }

            return new BinaryClassificationPerformanceModel
            {
                Precision = PerformanceService.ComputePrecision(actualOutput.ToArray(), expectedOutput),
                Recall = PerformanceService.ComputeRecall(actualOutput.ToArray(), expectedOutput),
                Accuracy = PerformanceService.ComputeAccuracy(actualOutput.ToArray(), expectedOutput)
            };
        }

        /// <summary>
        /// Gets the image contour.
        /// </summary>
        /// <param name="image">Represents the image for which we wnat to get the contour.</param>
        /// <returns>Returns the initial image with the contour.</returns>
        public Bitmap GetImageContour(Bitmap image, IEnumerable<Pixel> testData)
        {
            var resultImage = image.DeepClone();
            var testDataArray = testData.ToArray();
            var i = 0;

            for (var x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    var item = testDataArray[i];
                    var pixelColor = image.GetPixel(x, y);
                    var input = new double[]
                    {
                        item.Red,
                        pixelColor.GetBrightness()
                    };

                    var output = Network.Compute(input);
                    var isContour = output[0] >= 0.99;

                    if (isContour)
                    {
                        resultImage.SetPixel(x, y, Color.Red);
                    }

                    i++;
                }
            }

            return resultImage;
        }
    }
}
