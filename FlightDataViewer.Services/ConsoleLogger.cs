using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightDataViewer.Services
{
    /// <summary>
    /// Not an ideal logger. Created a logger to show my intention of logging
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }

        public void Log(Exception ex)
        {
            Console.WriteLine($"{ex.Message}\r\n{ex.StackTrace}");
        }
    }
}
