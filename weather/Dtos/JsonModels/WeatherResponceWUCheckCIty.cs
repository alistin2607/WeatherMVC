namespace Weather.Controllers.JsonModels
{
    public class WeatherResponceWUCheckCIty
    {
        public ResponseWUCheckCity response { get; set; }
    }
    public class ResponseWUCheckCity
    {
        public ResultsWUCheckCity[] results { get; set; }
    }
    public class ResultsWUCheckCity
    {
        public string city { get; set; }
        public string zmw { get; set; }
        public string country_iso3166 { get; set; }
    }

}