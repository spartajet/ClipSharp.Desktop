using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ClipSharp.Core.ViewModels;

namespace ClipSharp.Core.SettingModels
{
    public class PageBaseViewModel
    {
        public PageBaseViewModel()
        {
            InvokeCommand = new FACommand(PageInvoked);
        }

        public MainWindowViewModel Parent { get; set; }

        public string Header { get; init; }

        public string Description { get; init; }

        public string IconResourceKey { get; init; }

        public string PageKey { get; init; }

        public string[] SearchKeywords { get; init; }

        public FACommand InvokeCommand { get; }

        private void PageInvoked(object param)
        {
            NavigationService.Instance.NavigateFromContext(this);
        }
    }

    public class FACommand : ICommand
    {
        public FACommand(Action<object> executeMethod)
        {
            _executeMethod = executeMethod;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            _executeMethod.Invoke(parameter);
        }

        private Action<object> _executeMethod;
    }

}
