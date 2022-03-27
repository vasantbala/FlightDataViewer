using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace FlightDataViewer.Services
{
    /// <summary>
    /// Singleton implementaion of dependency manager
    /// </summary>
    public class DependencyConfigurationManager
    {
        #region private
        private IUnityContainer _unityContainer = null;
        private static DependencyConfigurationManager _instance = null;

        private void Configure()
        {
            if (_unityContainer == null)
            {
                _unityContainer = new UnityContainer();
                _unityContainer.RegisterType<ILogger, ConsoleLogger>();
                _unityContainer.RegisterType<FlightDataParser.IFlightDataParser
                    , FlightDataParser.FlightDataTextFileParser>(FlightDataParser.FlightDataParserType.Text.ToString());
                _unityContainer.RegisterType<FlightDataParser.IFlightDataParser
                    , FlightDataParser.FlightDataTextFileParser>(FlightDataParser.FlightDataParserType.Json.ToString());
                _unityContainer.RegisterType<FlightDataParser.IFlightDataParser
                    , FlightDataParser.FlightDataTextFileParser>(FlightDataParser.FlightDataParserType.XML.ToString());
                

                Func<FlightDataParser.FlightDataParserType, FlightDataParser.IFlightDataParser> factoryImpl
                    = (parserType) => _unityContainer.Resolve<FlightDataParser.IFlightDataParser>(parserType.ToString());

                var factory = new FlightDataParser.FlightDataParserFactory(factoryImpl);
                _unityContainer.RegisterInstance<FlightDataParser.IFlightDataParserFactory>(factory);
            }
        }

        private DependencyConfigurationManager()
        {
            Configure();
        }
        #endregion
        
        #region public
        public static DependencyConfigurationManager Instance() 
        {
            if (_instance == null)
            {
                _instance = new DependencyConfigurationManager();
            }
            return _instance;
        }

        public IUnityContainer Container
        {
            get
            {
                return _unityContainer;
            }
        }
        #endregion

    }
}
