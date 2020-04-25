using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Weather.Controllers.JsonModels;
using System.Web.Mvc;
using Weather.Models;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Web.Configuration;

namespace Weather.Controllers
{
    public class WeatherForecastService
    {
        /// <summary>
        /// Get forecast from wunderground.com and openweathermap.org
        /// </summary>
        /// <param name="model">index model</param>
        /// <returns></returns>
        public (WeatherResponceOWM OWMForecast, WeatherResponceWU WUForecast) GetWeather(Index model)
        {
            var forecastOWM = GetWeatherOWM(model.ResCityName);
            var forecastWU = new WeatherResponceWU();
            if (forecastOWM.sys != null)
            {
                forecastWU = GetWeatherWU(model.ResCityName.Replace(" ", "_"), forecastOWM.sys.country, WebConfigurationManager.AppSettings["WUConnectingString"]);
            }
            return (forecastOWM, forecastWU);
        }

        //Get weather from OWM
        private WeatherResponceOWM GetWeatherOWM(string city)
        {
            var url = WebConfigurationManager.AppSettings["OWMConnectingString"] + city + "&units=metric&appid=1ae53a9fe9ca665c0eec2d741c52bf83";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                string response;
                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    response = streamReader.ReadToEnd();
                }
                WeatherResponceOWM WR = new WeatherResponceOWM();

                WR = JsonConvert.DeserializeObject<WeatherResponceOWM>(response);
                return WR;
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError &&
                    ex.Response != null)
                {
                    var resp = (HttpWebResponse)ex.Response;
                    if (resp.StatusCode == HttpStatusCode.NotFound)
                    {
                        WeatherResponceOWM WR = new WeatherResponceOWM();
                        WR.name = "Not found";
                        return WR;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        //Get weather from WU 
        private WeatherResponceWU GetWeatherWU(string city, string testcountry, string url)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url + city + ".json");
            try
            {
                var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                string response;
                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    response = streamReader.ReadToEnd();
                }

                var WRCC = new WeatherResponceWUCheckCIty();
                WRCC = JsonConvert.DeserializeObject<WeatherResponceWUCheckCIty>(response);

                if (WRCC.response.results != null)
                {
                    string strcode = WRCC.response.results[0].zmw;
                    if (testcountry != null)
                    {
                        for (int i = 0; i < WRCC.response.results.Length; i++)
                        {
                            if (WRCC.response.results[i].country_iso3166 == testcountry)
                            {
                                strcode = WRCC.response.results[i].zmw;
                                break;
                            }
                        }
                    }
                    return GetWeatherWU(city, testcountry, WebConfigurationManager.AppSettings["WUConnectingString"] + "/zmw:");
                }
                else
                {
                    var WRGF = new WeatherResponceWU();
                    WRGF = JsonConvert.DeserializeObject<WeatherResponceWU>(response);
                    if (WRGF.current_observation != null) { 
                        if (WRGF.current_observation.display_location.city.ToLower() != city.ToLower())
                        {
                            WRGF.current_observation.display_location.city = "Not found";
                        }
                    }
                    return WRGF;
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError &&
                    ex.Response != null)
                {
                    var resp = (HttpWebResponse)ex.Response;
                    if (resp.StatusCode == HttpStatusCode.NotFound)
                    {
                        WeatherResponceWU WRWU = new WeatherResponceWU();
                        WRWU.current_observation.display_location.city = "Not found";
                        return null;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }
    }
}