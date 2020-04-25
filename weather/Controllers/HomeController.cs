using System.Web.Mvc;
using Weather.Models;
using Weather.Controllers.JsonModels;
using Weather.Services;

namespace Weather.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ViewResult Index()
        {
            var cookieService = new CookieService();
            var model = new Index();
            cookieService.GetCookie(model, this);
            ViewBag.space = " ";
            return View(model);
        }
        [HttpPost]
        public ViewResult Index(Index model)
        {
            
            if (ModelState.IsValid)
            {
                var forecast = new WeatherForecastService();
                var weather = forecast.GetWeather(model);
                ViewGen(weather.OWMForecast, weather.WUForecast);
                var cookieService = new CookieService();
                cookieService.CookieAddCity(model, model.ResCityName, this);
                ViewBag.space = " ";
                return View("ShowWeather", model);
            }
            else
            {
                return View("ShowWeather", model);
            }
        }

        //Previous city forecast
        public ViewResult PreviousCity(string city)
        {
            Index model = new Models.Index();
            model.ResCityName = city;
            model.last = true;
            WeatherForecastService forecast = new WeatherForecastService();
            var weather = forecast.GetWeather(model);
            ViewGen(weather.OWMForecast, weather.WUForecast);
            var cookieService = new CookieService();
            cookieService.CookieAddCity(model, model.ResCityName, this);
            ViewBag.space = " ";
            return View("ShowWeather", model);
        }

        public void ViewGen(WeatherResponceOWM OWM, WeatherResponceWU WU)
        {
            if (OWM != null)
            {
                if (OWM.name != "Not found")
                {
                    ViewBag.Portal1 = "OpenWeatherMap";
                    ViewBag.Name1 = OWM.name + ", " + OWM.sys.country;
                    ViewBag.temp1 = OWM.Main.Temp + "°C";
                    ViewBag.cloud1 = "Weather: " + OWM.weather[0].main;
                    ViewBag.wind1 = "Wind: " + OWM.Wind.speed + "m/sec, " + OWM.Wind.deg + " deg";
                    ViewBag.humb1 = "Humibity: " + OWM.Main.humbity + "%";
                    ViewBag.pres1 = "Pressure: " + OWM.Main.pressure + "hpa";
                }
                else
                {
                    ViewBag.Portal1 = "OpenWeatherMap";
                    ViewBag.Name1 = "City is not found";
                }
            }
            else
            {
                ViewBag.Portal1 = "OpenWeatherMap";
                ViewBag.Name1 = "City is not found";
            }
            if (WU?.current_observation != null)
            {
                if (WU.current_observation.display_location.city != "Not found")
                {
                    ViewBag.Portal2 = "Weather Underground";
                    ViewBag.Name2 = WU.current_observation.display_location.full.Replace("_", " ");
                    ViewBag.temp2 = WU.current_observation.temp_c + "°C";
                    ViewBag.cloud2 = "Weather: " + WU.current_observation.weather;
                    ViewBag.wind2 = "Wind: " + WU.current_observation.wind_kph + "m/sec, " + WU.current_observation.wind_degrees + " deg";
                    ViewBag.humb2 = "Humibity: " + WU.current_observation.relative_humidity;
                    ViewBag.pres2 = "Pressure: " + WU.current_observation.pressure_mb + "hpa";

                }
                else
                {
                    ViewBag.Portal2 = "Weather Underground";
                    ViewBag.Name2 = "City is not found";
                }
            }
            else
            {
                ViewBag.Portal2 = "Weather Underground";
                ViewBag.Name2 = "City is not found";
            }

        }
    }
}
