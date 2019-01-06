namespace HeartChamberIdentification.Services
{
    public static class PerformanceService
    {
        private const int _inContour = 1;
        private const int _outContour = 0;

        public static double ComputeAccuracy(int[] actualOutput, int[] expectedOutput)
        {
            var goodResultsCount = 0;
            for (var i = 0; i < actualOutput.Length; i++)
            {
                if (actualOutput[i] == expectedOutput[i])
                    goodResultsCount++;
            }

            var result = goodResultsCount / (double)actualOutput.Length;
            return result;
        }

        public static double ComputePrecision(int[] actualOutput, int[] expectedOutput)
        {
            var truePositiveCount = 0;
            var falsePositiveCount = 0;
            for (var i = 0; i < actualOutput.Length; i++)
            {
                if (actualOutput[i] == _inContour &&
                    expectedOutput[i] == _inContour)
                    truePositiveCount++;

                if (actualOutput[i] == _inContour &&
                    expectedOutput[i] == _outContour)
                    falsePositiveCount++;
            }

            if (truePositiveCount + falsePositiveCount == 0)
                return 0;

            var result = truePositiveCount / (truePositiveCount + (double)falsePositiveCount);
            return result;
        }

        public static double ComputeRecall(int[] actualOutput, int[] expectedOutput)
        {
            var truePositiveCount = 0;
            var falseNegativeCount = 0;
            for (var i = 0; i < actualOutput.Length; i++)
            {
                if (actualOutput[i] == _inContour &&
                    expectedOutput[i] == _inContour)
                    truePositiveCount++;

                if (actualOutput[i] == _outContour &&
                    expectedOutput[i] == _inContour)
                    falseNegativeCount++;
            }

            if (truePositiveCount + falseNegativeCount == 0)
                return 0;

            var result = truePositiveCount / (truePositiveCount + (double)falseNegativeCount);
            return result;
        }
    }
}
