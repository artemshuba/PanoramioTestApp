using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramioTestApp.ViewModel
{
    public class ViewModelLocator
    {
        private static MainViewModel _main;

        public static MainViewModel Main
        {
            get { return _main; }
        }

        static ViewModelLocator()
        {
            _main = new MainViewModel();
        }
    }
}
