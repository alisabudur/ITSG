using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning.VectorMachines.Learning;
using HeartChamberIdentification.Extensions;

namespace HeartChamberIdentification.Services
{
    public class SvmService
    {
        private SupportVectorMachine _svm;

        /// <summary>
        /// Trains a Support Vector Machine.
        /// </summary>
        /// <param name="input">The input of the SVM.</param>
        /// <param name="output">The output of the SVM.</param>
        public void Train(double[][] input, double[][] output)
        {
            var learn = new SequentialMinimalOptimization()
            {
                UseComplexityHeuristic = true,
                UseKernelEstimation = false
            };

            _svm = learn.Learn(input, output.ConvertToIntArray());
        }

        /// <summary>
        /// Computes the result of the Support Vector Machine previously trained.
        /// </summary>
        /// <param name="input">The input of the SVM.</param>
        /// <returns>Returns the output of SVM.</returns>
        public double[] Compute(double[] input)
        {
            double[] result = new double[input.Length];
            _svm.Transform(input, result);

            return result;
        }
    }
}
