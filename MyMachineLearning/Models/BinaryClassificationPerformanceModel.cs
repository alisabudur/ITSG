using MyMachineLearning.Interfaces;

namespace MyMachineLearning.Models
{
    public class BinaryClassificationPerformanceModel : IPerformance
    {
        public double Precision { get; set; }
        public double Recall { get; set; }
        public double Accuracy { get; set; }
    }
}
