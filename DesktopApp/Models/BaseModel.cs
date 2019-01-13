using System.ComponentModel;
using System.IO;
using System.Windows;

namespace DesktopApp.Models
{
    public abstract class BaseModel : INotifyPropertyChanged
    {
        private string _trainingFolder;
        private string _testingFolder;
        private string _testImagePath;
        private bool _isTrained;
        private Visibility _isResultImageVisibility;

        public BaseModel()
        {
            ResultImageVisibility = Visibility.Hidden;
        }

        public string TrainingFolder
        {
            get { return _trainingFolder;}
            set
            {
                _trainingFolder = value;
                OnPropertyChanged(nameof(TrainingFolder));
                OnPropertyChanged(nameof(StartTrainingEnabled));
            }
        }

        public string TestingFolder
        {
            get { return _testingFolder; }
            set
            {
                _testingFolder = value;
                OnPropertyChanged(nameof(TestingFolder));
            }
        }

        public string TestImagePath
        {
            get { return _testImagePath; }
            set
            {
                _testImagePath = value;
                OnPropertyChanged(nameof(TestImagePath));
                OnPropertyChanged(nameof(StartTestingEnabled));
            }
        }

        public string ResultImagePath
        {
            get
            {
                var resultPath = "E:\\Master\\Semestrul 3\\Intelligent tools for social goods\\Results";
                var testImageName = Path.GetFileName(TestImagePath);
                if (testImageName != null)
                {
                    var resultImagePath = Path.Combine(resultPath, testImageName);
                    return resultImagePath;
                }

                return string.Empty;
            }
        }

        public bool IsTrained
        {
            get { return _isTrained; }
            set
            {
                _isTrained = value;
                OnPropertyChanged(nameof(IsTrained));
                OnPropertyChanged(nameof(IsTraitenLabelVisibility));
            }
        }

        public Visibility ResultImageVisibility
        {
            get { return _isResultImageVisibility; }
            set
            {
                _isResultImageVisibility = value;
                OnPropertyChanged(nameof(ResultImageVisibility));
                OnPropertyChanged(nameof(ResultImagePath));
            }
        }

        public Visibility IsTraitenLabelVisibility => IsTrained ? Visibility.Visible : Visibility.Hidden;

        public virtual bool StartTestingEnabled => IsTrained && !string.IsNullOrEmpty(TestImagePath);

        public virtual bool StartTrainingEnabled => !string.IsNullOrEmpty(TrainingFolder);


        #region Property changed

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyname)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyname));
        }

        #endregion
    }
}
