using System.Collections.Generic;
using Accord.Neuro;
using Accord.Neuro.Learning;
using MyMachineLearning.Extensions;
using MyMachineLearning.Interfaces;
using MyMachineLearning.Models;

namespace MyMachineLearning.Services
{
    public abstract class AnnServiceBase<T> where T : class, IEntity
    {
        protected readonly ActivationNetwork Network;

        private readonly BackPropagationLearning _learning;
        private readonly TerminationCondition _terminationCondition;

        /// <summary>
        /// Constructor for AnnServiceBase
        /// </summary>
        /// <param name="network">Represents the Artificial Neural Network.</param>
        /// <param name="terminationCondition">Represents the termination condition of 
        /// Artificial Neural Network.</param>
        protected AnnServiceBase(
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
        /// <param name="trainData">Represents the training data set.</param>
        public void Train(IEnumerable<T> trainData)
        {
            var ioModel = trainData.GetInputOutputModel();
            var input = ioModel.Input;
            var output = ioModel.Output;

            var needToStop = false;
            var epoch = 0;

            while (!needToStop && epoch < _terminationCondition.NumberOfEpochs)
            {
                var error = _learning.RunEpoch(input, output)
                            / input.Length;

                if (error < _terminationCondition.MinError)
                    needToStop = true;

                epoch++;
            }
        }

        /// <summary>
        /// Gets the performance of Artificial Neural Network previously trained.
        /// </summary>
        /// <param name="testData">Represents the testing data set.</param>
        /// <returns>Returns the performance result.</returns>
        public abstract IPerformance GetNetworkPerformane(IEnumerable<T> testData);
    }
}
