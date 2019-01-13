using Accord.Neuro;
using Accord.Neuro.Learning;
using MyMachineLearning.Models;

namespace HeartChamberIdentification.Services
{
    public class AnnService
    {
        private readonly ActivationNetwork Network;

        private readonly BackPropagationLearning _learning;
        private readonly TerminationCondition _terminationCondition;
        private readonly ImageService _imageService;

        /// <summary>
        /// Constructor for AnnServiceBase
        /// </summary>
        /// <param name="network">Represents the Artificial Neural Network.</param>
        /// <param name="terminationCondition">Represents the termination condition of 
        /// Artificial Neural Network.</param>
        public AnnService(
            ActivationNetwork network,
            TerminationCondition terminationCondition)
        {
            Network = network;
            _learning = new BackPropagationLearning(network);
            _terminationCondition = terminationCondition;
        }

        /// <summary>
        /// Trains a Neural Neywork with Back-propagation learning.
        /// </summary>
        /// <param name="input">The input of the ANN.</param>
        /// <param name="output">The output of the ANN.</param>
        public void Train(double[][] input, double[][] output)
        {
            var needToStop = false;
            var epoch = 0;

            while (!needToStop && epoch < _terminationCondition.NumberOfEpochs)
            {
                var error = _learning.RunEpoch(input, output)
                            / input.Length/input[0].Length;

                if (error < _terminationCondition.MinError)
                    needToStop = true;

                epoch++;
            }
        }

        /// <summary>
        /// Computes the result of the Artificial Neural network previously trained.
        /// </summary>
        /// <param name="input">The input of the ANN.</param>
        /// <returns>Returns the output of ANN.</returns>
        public double[] Compute(double[] input)
        {
            return Network.Compute(input);
        }
    }
}
