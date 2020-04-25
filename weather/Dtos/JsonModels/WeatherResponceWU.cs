namespace Weather.Controllers.JsonModels
{
    public class WeatherResponceWU
    {
        public CurrentObservationWU current_observation { get; set;}
    }
    public class CurrentObservationWU
    {
        public string temp_c { get; set; }
        public string pressure_mb { get; set; }
        public string relative_humidity { get; set; }
        public string wind_kph { get; set; }
        public string wind_degrees { get; set; }
        public string weather { get; set; }
        public DisplayLocation display_location { get; set; }
    }
    public class DisplayLocation
    {
        public string city { get; set; }
        public string full { get; set; }
    }
}