using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using weather.Models;
using System.Threading.Tasks;
using weather.Controllers.JsonModels;
using weather.Controllers;


namespace weather.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home

        [HttpGet]
        public ViewResult Index()
       {
            var model = new Index();
            cookieswork(model);
            return View(model);
        }
        [HttpPost]
        public ViewResult Index(Index model)
        {
            
            if (ModelState.IsValid)
            {
                getweather get = new getweather();
                WeatherResponceOWM weather1 = new WeatherResponceOWM();
                WeatherResponceWU weather2 = new WeatherResponceWU();
                //получаем объект класса с погодой 1
                weather1 = get.getWeatherOWM(model.ResCityName);
                //получаем объект класса с погодой 1
                weather2 = get.getWeatherWU(model.ResCityName.Replace(" ", "_"));
                ViewGen(weather1, weather2);
                //обрабатываем куки
                cookieswork(model, model.ResCityName);
                return View("ShowWeather", model);
            }
            else
            {
                return View("ShowWeather", model);
            }
        }

        //смотрим погоду по ранее просмотренному городу
        public ViewResult last(Index model, string city)
        {
            model.ResCityName = city;
            model.last = true;
            getweather get = new getweather();
            WeatherResponceOWM weather1 = new WeatherResponceOWM();
            WeatherResponceWU weather2 = new WeatherResponceWU();
            //получаем объект класса с погодой 1
            weather1 = get.getWeatherOWM(model.ResCityName);
            //получаем объект класса с погодой 1
            weather2 = get.getWeatherWU(model.ResCityName.Replace(" ", "_"));
            ViewGen(weather1, weather2);

            //обрабатываем куки
            cookieswork(model, model.ResCityName);
            return View("ShowWeather", model);
        }

        //вывод данных во view
        public void ViewGen(WeatherResponceOWM OWM, WeatherResponceWU WU)
        {
            // 1 сервис
            if (OWM != null)
            {
                if (OWM.name != "Not found")
                {
                    //отправляем все во вью
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
            //2 сервис
            if (WU.current_observation != null)
            {
                if (WU.current_observation.display_location.city != "Not found")
                {
                    //отправляем все во вью
                    ViewBag.Portal2 = "Weather Underground";
                    ViewBag.Name2 = WU.current_observation.display_location.full.Replace("_", " ");
                    ViewBag.temp2 = WU.current_observation.temp_c + "°C";
                    ViewBag.cloud2 = "Weather: " + WU.current_observation.weather;
                    ViewBag.wind2 = "Wind: " + WU.current_observation.wind_kph + "m/sec, " + WU.current_observation.wind_degrees + " deg";
                    ViewBag.humb2 = "Humibity: " + WU.current_observation.relative_humidity + "%";
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

        //работаем с куками - 1, получаем куки при первой загрузке страницы
        public void cookieswork(Index model)
        {
            HttpCookie cookieReq = Request.Cookies["last city"];
            if (cookieReq != null)
            {
                string cookie = cookieReq.Value.ToString().Substring(5);
                char d = ',';
                String[] substrings = cookie.Split(d);
                
                for (int i = 0; i < substrings.Length; i++)
                {
                    model.Cookie.Add(substrings[i]);
                }
            }
        }

        //работаем с куками - 2, добавляем город в куки при отправке формы
        public void cookieswork(Index model, string addedcity)
        {
            HttpCookie cookieReq = Request.Cookies["last city"];
            if (cookieReq != null)
            {
                string cookie = cookieReq.Value.ToString().Substring(5);
                char d = ',';
                String[] substrings = cookie.Split(d);
                String[] substr = new string[4];

                int n = substrings.Length;
                for (int i = 0; i < n; i++)
                {
                    substr[i] = substrings[i];
                }
                if (n < 4)
                {
                    substr[n] = addedcity;
                }
                else
                {
                    for (int i = 0; i < n - 1; i++)
                    {
                        substr[i] = substr[i + 1];
                    }
                    substr[n-1] = addedcity;
                }
                if (n != 4)
                {
                    string cookiestr = substr[0];
                    model.Cookie.Add(substr[0]);
                    for (int i = 1; i < n+1; i++)
                    {
                        cookiestr = cookiestr + "," + substr[i];
                        model.Cookie.Add(substr[i]);
                    }
                    var cook = new HttpCookie("last city");
                    cook["city"] = cookiestr;
                    Response.SetCookie(cook);
                }
                else
                {
                    string cookiestr = substr[0];
                    model.Cookie.Add(substr[0]);
                    for (int i = 1; i < n; i++)
                    {
                        cookiestr = cookiestr + "," + substr[i];
                        model.Cookie.Add(substr[i]);
                    }
                    var cook = new HttpCookie("last city");
                    cook["city"] = cookiestr;
                    Response.SetCookie(cook);
                }
            }
            else
            {
                var cook = new HttpCookie("last city");
                cook["city"] = addedcity;
                Response.SetCookie(cook);
            }
        }


    }
}
