using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using SmartBoxCity.Model;
using SmartBoxCity.Model.AuthViewModel;

namespace SmartBoxCity.Service
{
    public class HomeService
    {
        private static HttpClient _httpClient;

        /// <summary>
        /// Инициализация экземпляра клиента
        /// </summary>
        /// <param name="client"></param>
        public static void InitializeClient(HttpClient client)
        {
            _httpClient = client;
        }
        public static async Task<ServiceResponseObject<RegisterResponseData>> About()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"about");

            string s_result;
            using (HttpContent responseContent = response.Content)
            {
                s_result = await responseContent.ReadAsStringAsync();
            }

            ServiceResponseObject<RegisterResponseData> o_data = new ServiceResponseObject<RegisterResponseData>();
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                ErrorResponseObject error = new ErrorResponseObject();
                error = JsonConvert.DeserializeObject<ErrorResponseObject>(s_result);
                o_data.Status = response.StatusCode;
                o_data.Message = error.Errors[0];
                return o_data;
            }
            var message = JsonConvert.DeserializeObject<RegisterResponseData>(s_result);
            o_data.ResponseData = message;
            o_data.Message = "Успешно!";
            o_data.Status = response.StatusCode;
            return o_data;
        }

        public static async Task<ServiceResponseObject<ContactsResponseData>> ContactUs()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"contacts");

            string s_result;
            using (HttpContent responseContent = response.Content)
            {
                s_result = await responseContent.ReadAsStringAsync();
            }

            ServiceResponseObject<ContactsResponseData> o_data = new ServiceResponseObject<ContactsResponseData>();
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                ErrorResponseObject error = new ErrorResponseObject();
                error = JsonConvert.DeserializeObject<ErrorResponseObject>(s_result);
                o_data.Status = response.StatusCode;
                o_data.Message = error.Errors[0];
                return o_data;
            }
            var message = JsonConvert.DeserializeObject<ContactsResponseData>(s_result);
            o_data.ResponseData = message;
            o_data.Message = "Успешно!";
            o_data.Status = response.StatusCode;
            return o_data;
        }

    }
}