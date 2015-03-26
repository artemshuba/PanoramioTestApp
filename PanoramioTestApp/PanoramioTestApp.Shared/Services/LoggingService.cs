using System;
using System.Diagnostics;

namespace PanoramioTestApp.Services
{
    public static class LoggingService
    {
        public static void Log(Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }
}
