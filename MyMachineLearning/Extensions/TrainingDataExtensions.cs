using System;
using System.Collections.Generic;
using System.Linq;
using MyMachineLearning.Interfaces;
using MyMachineLearning.Models;

namespace MyMachineLearning.Extensions
{
    public static class TrainingDataExtensions
    {
        /// <summary>
        /// Splits the learning data into training data set and testing data set. 
        /// First, it splits the initial data set in kfold subsets. Then, it selects as training set 
        /// the subset with the inde equal to testSetIndex. The remaining subsets are used for training.  
        /// </summary>
        /// <typeparam name="T">Type of the learning item.</typeparam>
        /// <param name="items">Represents the data set items.</param>
        /// <param name="kfold">Represents in how many subsets we want to split the data set.</param>
        /// <param name="testSetIndex">Represents the index of the subset that we want to use for testing.</param>
        /// <returns>Returns the initial data set splitted into training data set and testing data set.</returns>
        public static LearningModel<T> GetLearningModel<T>(this IEnumerable<T> items, int kfold, int testSetIndex)
            where T : class, IEntity
        {
            if (testSetIndex > kfold)
                throw new Exception($"Testing subset index cannot be greater than k = {kfold}.");

            var itemsArray = items as T[] ?? items.ToArray();
            var totalCount = itemsArray.Count();
            var subsetLength = totalCount / kfold;
            var takeSubsetLength = subsetLength;
            var lastSubsetcount = totalCount / kfold + totalCount % kfold;

            var trainingSet = new List<T>();
            var testingSet = new List<T>();

            for (var i = 0; i < kfold; i++)
            {
                if (i == kfold - 1)
                    takeSubsetLength = lastSubsetcount;

                var subset = itemsArray
                    .Skip(subsetLength * i)
                    .Take(takeSubsetLength)
                    .ToList();

                if (testSetIndex == i)
                {
                    testingSet = subset;
                }
                else
                {
                    trainingSet.AddRange(subset);
                }
            }

            return new LearningModel<T>
            {
                TrainData = trainingSet,
                TestData = testingSet
            };
        }

        /// <summary>
        /// Builds the input set and the output set from the initial set of learning items.
        /// </summary>
        /// <typeparam name="T">Type of the learning item.</typeparam>
        /// <param name="items">Represents the data set items.</param>
        /// <returns>Returns the input set and the output set from the initial set of learning items.</returns>
        public static InputOutputModel GetInputOutputModel<T>(this IEnumerable<T> items)
            where T : class, IEntity
        {
            var itemsArray = items.ToArray();

            var input = new List<double[]>();
            var output = new List<double[]>();

            foreach (var item in itemsArray)
            {
                input.Add(item.ToInputModel());
                output.Add(item.ToOutputModel());
            }

            return new InputOutputModel
            {
                Input = input.ToArray(),
                Output = output.ToArray()
            };
        }
    }
}
