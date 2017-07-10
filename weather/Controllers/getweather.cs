using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using weather.Controllers.JsonModels;
using System.Web.Mvc;
using weather.Models;
using System.Net;
using Newtonsoft.Json;
using System.IO;


namespace weather.Controllers
{
    public class getweather
    {
        //берем погоду с OWM
        public WeatherResponceOWM getWeatherOWM(string city)
        {
            string url;
            var model = new Index();

            url = "http://api.openweathermap.org/data/2.5/weather?q=" + city + "&units=metric&appid=1ae53a9fe9ca665c0eec2d741c52bf83";

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

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

                if (WR.name.ToLower() != city.ToLower())
                {
                    WR.name = "Not found";
                }
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

        //get forecast from WU service, get forecast
        public WeatherResponceWU getWeatherWU(string city)
        {
            string url;
            var model = new Index();

            url = "http://api.wunderground.com/api/c4bb30b97572d3ed/conditions/q/CA/" + city + ".json";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            try
            {
                //сначала проверяем, вернется нам список городов или сразу прогноз. 
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                string response;
                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    response = streamReader.ReadToEnd();
                }

                WeatherResponceWUCheckCIty WRCC = new WeatherResponceWUCheckCIty();
                WRCC = JsonConvert.DeserializeObject<WeatherResponceWUCheckCIty>(response);

                if (WRCC.response.results != null)
                {
                    //если список городов, берем из него первую ссылку и по ней тянем сам прогноз
                    url = "http://api.wunderground.com/api/c4bb30b97572d3ed/conditions/q/CA/zmw:" + WRCC.response.results[0].zmw + ".json";
                    
                    try
                    {
                        HttpWebRequest httpWebRequestGF = (HttpWebRequest)WebRequest.Create(url);
                        HttpWebResponse httpWebResponseGF = (HttpWebResponse)httpWebRequest.GetResponse();
                        string responseGF;

                        using (HttpWebResponse resp = (HttpWebResponse)httpWebRequestGF.GetResponse())
                        {
                            using (var reader = new StreamReader(resp.GetResponseStream()))
                            {
                                responseGF = reader.ReadToEnd();
                            }
                        }

                        WeatherResponceWU WRGF = new WeatherResponceWU();
                        WRGF = JsonConvert.DeserializeObject<WeatherResponceWU>(responseGF);
                        if (WRGF.current_observation.display_location.city.ToLower() != city.ToLower())
                        {
                            WRGF.current_observation.display_location.city = "Not found";
                        }
                        return WRGF;
                    }
                    catch (WebException ex)
                    {
                        var resp = (HttpWebResponse)ex.Response;
                        if (resp.StatusCode == HttpStatusCode.NotFound)
                        {
                            WeatherResponceWU WRGF = new WeatherResponceWU();
                            WRGF.current_observation.display_location.city = "Not found";
                            return null;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                else
                {
                    //если пришел прогноз
                    WeatherResponceWU WRGF = new WeatherResponceWU();
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
                        // WRWU.display_location.City = "Not found";
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