using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightDataViewer.Services.FlightDataParser
{
    public class FlightDataParserFactory : IFlightDataParserFactory
    {
        readonly Func<FlightDataParserType, IFlightDataParser> _factoryImpl;
        public FlightDataParserFactory(Func<FlightDataParserType, IFlightDataParser> factoryImpl)
        {
            _factoryImpl = factoryImpl;
        }
        public IFlightDataParser Create(FlightDataParserType parserType)
        {
            return _factoryImpl(parserType);
        }
    }
}
