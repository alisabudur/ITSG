using System.Windows.Forms;
using System.Windows.Input;
using DesktopApp.Models;
using DesktopApp.Utils;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace DesktopApp.ViewModels
{
    public abstract class PageBaseViewModel : BaseViewModel
    {
        protected bool _canExecute;

        private ICommand _selectTrainingFolder;
        private ICommand _selectTestingFolder;
        private ICommand _uploadTestImage;

        private BaseModel _model;

        public PageBaseViewModel(BaseModel model)
        {
            _canExecute = true;
            _model = model;
        }

        public ICommand SelectTrainingFolderCommand => 
            _selectTrainingFolder ?? (_selectTrainingFolder = new CommandHandler(SelectTrainingFolder, _canExecute));

        public ICommand SelectTestingFolderCommand => 
            _selectTestingFolder ?? (_selectTestingFolder = new CommandHandler(SelectTestingFolder, _canExecute));

        public ICommand UploadTestImageCommand => 
            _uploadTestImage ?? (_uploadTestImage = new CommandHandler(UploadImage, _canExecute));

        protected BaseModel Model
        {
            get { return _model; }
            set
            {
                _model = value;
                OnPropertyChanged(nameof(Model));
            }
        }

        public void UploadImage()
        {
            var op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                        "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                        "Portable Network Graphic (*.png)|*.png";

            if (op.ShowDialog() == true)
            {
                _model.TestImagePath = op.FileName;
                OnPropertyChanged(nameof(Model));
            }
        }

        public void SelectTrainingFolder()
        {
            using (var op = new FolderBrowserDialog())
            {
                DialogResult result = op.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(op.SelectedPath))
                {
                    _model.TrainingFolder = op.SelectedPath;
                }
            }
        }

        public void SelectTestingFolder()
        {
            using (var op = new FolderBrowserDialog())
            {
                DialogResult result = op.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(op.SelectedPath))
                {
                    _model.TestingFolder = op.SelectedPath;
                }
            }
        }
    }
}
