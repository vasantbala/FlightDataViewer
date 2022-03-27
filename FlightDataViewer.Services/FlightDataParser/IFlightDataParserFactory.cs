using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightDataViewer.Services.FlightDataParser
{
    /// <summary>
    /// Interface for FlightDataParserFactory
    /// </summary>
    public interface IFlightDataParserFactory
    {
        IFlightDataParser Create(FlightDataParserType parserType);
    }
}
