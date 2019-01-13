using System.Windows.Input;
using DesktopApp.Models;

namespace DesktopApp.ViewModels
{
    public class SvmPageViewModel: PageBaseViewModel
    {
        private ICommand _startTraining;
        private ICommand _startTesting;

        public SvmPageViewModel() : base(new SvmModel())
        {
        }

        public ICommand StartTrainingCommand =>
            _startTraining ?? (_startTraining = new CommandHandler(StartTraining, _canExecute));

        public ICommand StartTestingCommand =>
            _startTesting ?? (_startTesting = new CommandHandler(StartTesting, _canExecute));

        public SvmModel SvmModel
        {
            get { return (SvmModel)Model; }
            set
            {
                Model = value;
                OnPropertyChanged(nameof(SvmModel));
            }
        }

        public void StartTraining()
        {
            SvmModel.IsTrained = true;
        }

        public void StartTesting()
        {

        }
    }
}
