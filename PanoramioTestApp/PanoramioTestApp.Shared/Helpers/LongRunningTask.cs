using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PanoramioTestApp.Helpers
{
    public class LongRunningTask : INotifyPropertyChanged
    {
        private bool _isWorking;
        private string _error;
        private string _status;

        public bool IsWorking
        {
            get { return _isWorking; }
            set
            {
                if (_isWorking == value)
                    return;

                _isWorking = value;
                OnPropertyChanged();
            }
        }

        public string Error
        {
            get { return _error; }
            set
            {
                if (_error == value)
                    return;

                _error = value;
                OnPropertyChanged();
            }
        }

        public string Status
        {
            get { return _status; }
            set
            {
                if (_status == value)
                    return;

                _status = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
