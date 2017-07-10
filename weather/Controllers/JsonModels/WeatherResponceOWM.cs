using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace weather.Controllers.JsonModels
{
    public class WeatherResponceOWM
    {
        public string name { get; set; }
        public MainInfoOWM Main { get; set; }
        public WindInfoOWM Wind { get; set; }
        public WeatherInfoOWM[] weather { get; set; }
        public SysInfoOWM sys { get; set; }
    }
    public class MainInfoOWM
    {
        public float Temp { get; set; } 
        public float pressure { get; set; }
        public float humbity { get; set; }
    }
    public class WeatherInfoOWM
    {
        public string main { get; set; } 
    }
    public class WindInfoOWM
    {
        public float speed { get; set; } 
        public float deg { get; set; }   
    }
    public class SysInfoOWM
    {
        public string country { get; set; }
    }
}