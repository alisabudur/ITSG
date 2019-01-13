using System.ComponentModel;
using System.Windows;

namespace DesktopApp.Models
{
    public class AnnModel: BaseModel, IDataErrorInfo
    {
        private double _minError;
        private double _learningRate;
        private int _noOfEpochs;

        public double MinError
        {
            get { return _minError; }
            set
            {
                _minError = value;
                OnPropertyChanged(nameof(MinError));
                OnPropertyChanged(nameof(StartTrainingEnabled));
            }
        }

        public double LearningRate
        {
            get { return _learningRate; }
            set
            {
                _learningRate = value;
                OnPropertyChanged(nameof(LearningRate));
                OnPropertyChanged(nameof(StartTrainingEnabled));
            }
        }

        public int NoOfEpochs
        {
            get { return _noOfEpochs; }
            set
            {
                _noOfEpochs = value;
                OnPropertyChanged(nameof(NoOfEpochs));
                OnPropertyChanged(nameof(StartTrainingEnabled));
            }
        }

        public override bool StartTrainingEnabled => 
            base.StartTrainingEnabled && NoOfEpochs > 0 
                                      && LearningRate > 0 
                                      && MinError > 0;

        #region IDataErrorInfo

        public string Error
        {
            get { return null; }
        }

        public string this[string columnName]
        {
            get
            {
                string error = null;

                switch (columnName)
                {
                    case "LearningRate":
                        if (_learningRate < 0)
                            error = "Learning rate must be a positove number.";
                        break;

                    case "Iterations":
                        if (_noOfEpochs < 0)
                            error = "Epochs must be a positove number.";
                        break;

                    case "AnnError":
                        if (_minError < 0)
                            error = "Error must be a positove number.";
                        break;

                    case "TrainingFolder":
                        if (string.IsNullOrEmpty(TrainingFolder))
                            error = "Training folder is required.";
                        break;
                }
                return (error);
            }
        }
        #endregion
    }
}
