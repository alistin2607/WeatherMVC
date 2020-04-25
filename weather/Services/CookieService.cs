using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Weather.Models;

namespace Weather.Services
{
    public class CookieService
    {
        /// <summary>
        /// Get cookie in first page open 
        /// </summary>
        /// <param name="model">Index model</param>
        public void GetCookie(Index model, Controllers.HomeController homeController)
        {
            HttpCookie cookieReq = homeController.Request.Cookies["last city"];
            if (cookieReq != null)
            {
                string cookie = cookieReq.Value.ToString().Substring(5);
                model.Cookie = cookie.Split(',').ToList();
            }
        }

        /// <summary>
        /// Add city to cookies 
        /// </summary>
        /// <param name="model">Index model</param>
        /// <param name="addedcity">Adding city</param>
        public void CookieAddCity(Index model, string addedcity, Controllers.HomeController homeController)
        {
            HttpCookie cookieReq = homeController.Request.Cookies["last city"];
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
                    substr[n - 1] = addedcity;
                }
                if (n != 4)
                {
                    string cookiestr = substr[0];
                    model.Cookie.Add(substr[0]);
                    for (int i = 1; i < n + 1; i++)
                    {
                        cookiestr = cookiestr + "," + substr[i];
                        model.Cookie.Add(substr[i]);
                    }
                    var cook = new HttpCookie("last city");
                    cook["city"] = cookiestr;
                    homeController.Response.SetCookie(cook);
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
                    homeController.Response.SetCookie(cook);
                }
            }
            else
            {
                var cook = new HttpCookie("last city");
                cook["city"] = addedcity;
                homeController.Response.SetCookie(cook);
            }
        }
    }
}