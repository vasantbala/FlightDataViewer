using FlightDataViewer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightDataViewer.Services.FlightDataParser
{
    /// <summary>
    /// XML Parser. Not implementated
    /// </summary>
    public class FlightDataXmlParser : IFlightDataParser
    {
        public FlightTransit[] Parse(FlightDataSource dataSource)
        {
            throw new NotImplementedException();
        }
    }
}
