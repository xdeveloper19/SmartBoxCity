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
    public class AuthService
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

        public static async Task<ServiceResponseObject<AuthResponseData>> Login(AuthModel model)
        {
              HttpResponseMessage response = await _httpClient.GetAsync($"login?login={model.Login}&password={model.Password}");

                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                ServiceResponseObject<AuthResponseData> o_data = new ServiceResponseObject<AuthResponseData>();
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ErrorResponseObject error = new ErrorResponseObject();
                    error = JsonConvert.DeserializeObject<ErrorResponseObject>(s_result);
                    o_data.Status = response.StatusCode;
                    o_data.Message = error.Errors[0];
                    return o_data;
                }
                o_data.ResponseData = JsonConvert.DeserializeObject<AuthResponseData>(s_result);
                o_data.Message = "Успешно авторизован!";
                o_data.Status = response.StatusCode;
                return o_data;
        }

        public static async Task Login(string token)
        {
                HttpResponseMessage response = await _httpClient.GetAsync($"login?login={token}&password=");
                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ErrorResponseObject error = new ErrorResponseObject();
                    error = JsonConvert.DeserializeObject<ErrorResponseObject>(s_result);
                }
            
        }

        public static async Task LogOut()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"logout");
            string s_result;
            using (HttpContent responseContent = response.Content)
            {
                s_result = await responseContent.ReadAsStringAsync();
            }

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                ErrorResponseObject error = new ErrorResponseObject();
                error = JsonConvert.DeserializeObject<ErrorResponseObject>(s_result);
            }

        }
    }
}