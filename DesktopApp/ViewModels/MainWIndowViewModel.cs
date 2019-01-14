using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using DesktopApp.Utils;
using DesktopApp.Views;

namespace DesktopApp.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private bool _canExecute;
        private Page _currentPage;
        private IDictionary<string, Page> _pages;
        private ICommand _navigateToAnnPage;
        private ICommand _navigateToSvmPage;


        public MainWindowViewModel()
        {
            _canExecute = true;
            _pages = new Dictionary<string, Page>()
            {
                { Constants.AnnPage, new AnnPage()},
                { Constants.SvmPage, new SvmPage() }
            };
            Page = _pages[Constants.AnnPage];
        }

        public Page Page
        {
            get { return _currentPage; }
            set { _currentPage = value; OnPropertyChanged("Page"); }
        }

        public ICommand NavigateToAnnPage => _navigateToAnnPage ?? (_navigateToAnnPage = new CommandHandler(
                                                         () => { Page = _pages[Constants.AnnPage]; }, _canExecute));

        public ICommand NavigateToSvmPage => _navigateToSvmPage ?? (_navigateToSvmPage = new CommandHandler(
                                                        () => { Page = _pages[Constants.SvmPage]; }, _canExecute));
    }
}
