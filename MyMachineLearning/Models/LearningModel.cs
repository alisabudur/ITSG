using System.Collections.Generic;

namespace MyMachineLearning.Models
{
    public class LearningModel<T> where T : class
    {
        public IEnumerable<T> TestData { get; set; }
        public IEnumerable<T> TrainData { get; set; }
    }
}
