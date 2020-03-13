using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SmartBoxCity.Service
{
    public static class ClientHelper
    {
        public const string URL = "http://smartboxcity.ru:8003/";

        public static HttpClient GetClient()
        {
            
            var client = new HttpClient()
            { 
                BaseAddress = new Uri(URL)
                //Set some other client defaults like timeout / BaseAddress
            };
            return client;
        }
        public static HttpClient GetClient(string username, string password)
        {
            var authValue = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}")));

            var client = new HttpClient()
            {
                DefaultRequestHeaders = { Authorization = authValue },
                BaseAddress = new Uri(URL)
            //Set some other client defaults like timeout / BaseAddress
        };
            return client;
        }

        // Auth with bearer token
        public static HttpClient GetClient(string token)
        {
            var authValue = new AuthenticationHeaderValue("Bearer", token);

            var client = new HttpClient()
            {
                DefaultRequestHeaders = { Authorization = authValue },
                BaseAddress = new Uri(URL)
                //Set some other client defaults like timeout / BaseAddress
            };
            return client;
        }
    }
}