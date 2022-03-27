using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightDataViewer.Services.FlightDataParser
{
    /// <summary>
    /// An entity to carry information about the data source.
    /// In the future, if json/ xml details can be provided here
    /// </summary>
    public class FlightDataSource
    {
        public string FilePath { get; set; }
    }
}
