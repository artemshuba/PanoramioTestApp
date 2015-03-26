using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using PanoramioTestApp.Helpers;

namespace PanoramioTestApp.ViewModel
{
    public class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase
    {
        private readonly Dictionary<string, LongRunningTask> _tasks = new Dictionary<string, LongRunningTask>();

        public Dictionary<string, LongRunningTask> Tasks
        {
            get { return _tasks; }
        }

        public virtual void Activate(NavigationMode navigationMode)
        {

        }

        public virtual void Deactivate()
        {

        }

        #region Long Running Tasks helpers

        protected void RegisterTasks(params string[] ids)
        {
            foreach (var id in ids)
            {
                _tasks.Add(id, new LongRunningTask());
            }
        }

        protected void TaskStarted(string id, string status = null)
        {
            _tasks[id].Error = null;
            _tasks[id].IsWorking = true;
            _tasks[id].Status = status;
        }

        protected void TaskFinished(string id)
        {
            _tasks[id].IsWorking = false;
            _tasks[id].Status = string.Empty;
        }

        protected void TaskError(string id, string error)
        {
            _tasks[id].Error = error;
            _tasks[id].IsWorking = false;
        }


        #endregion
    }
}
