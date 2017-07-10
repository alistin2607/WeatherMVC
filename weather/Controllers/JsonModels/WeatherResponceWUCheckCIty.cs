using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace weather.Controllers.JsonModels
{
    public class WeatherResponceWUCheckCIty
    {
        public responseWUCheckCity response { get; set; }
    }
    public class responseWUCheckCity
    {
        public resultsWUCheckCity[] results { get; set; }
    }
    public class resultsWUCheckCity
    {
        public string city { get; set; }
        public string zmw { get; set; }
    }

}