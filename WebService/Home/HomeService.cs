using Entity.Model;
using Entity.Model.AccountViewModel.AuthViewModel;
using Entity.Model.HomeViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebService.Home
{
    public class HomeService
    {
        /// <summary>
        /// Получение договора оферты.
        /// </summary>
        /// <returns></returns>
        public static async Task<ServiceResponseObject<AgreementResponse>> Offer()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"agreement/offer");

                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                ServiceResponseObject<AgreementResponse> o_data = new ServiceResponseObject<AgreementResponse>();
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ErrorResponseObject error = new ErrorResponseObject();
                    error = JsonConvert.DeserializeObject<ErrorResponseObject>(s_result);
                    o_data.Status = response.StatusCode;
                    o_data.Message = error.Errors[0];
                    return o_data;
                }
                var message = JsonConvert.DeserializeObject<AgreementResponse>(s_result);
                o_data.ResponseData = message;
                o_data.Message = "Успешно!";
                o_data.Status = response.StatusCode;
                return o_data;
            }
            catch (Exception ex)
            {
                ServiceResponseObject<AgreementResponse> o_data = new ServiceResponseObject<AgreementResponse>();
                o_data.Message = ex.Message;
                //Log.Debug("Error occuired getting offer response", ex.Message);
                return o_data;
            }
        }


        /// <summary>
        /// Получение соглашения.
        /// </summary>
        /// <returns></returns>
        public static async Task<ServiceResponseObject<AgreementResponse>> Privacy()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"agreement/privacy");

                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                ServiceResponseObject<AgreementResponse> o_data = new ServiceResponseObject<AgreementResponse>();
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ErrorResponseObject error = new ErrorResponseObject();
                    error = JsonConvert.DeserializeObject<ErrorResponseObject>(s_result);
                    o_data.Status = response.StatusCode;
                    o_data.Message = error.Errors[0];
                    return o_data;
                }
                var message = JsonConvert.DeserializeObject<AgreementResponse>(s_result);
                o_data.ResponseData = message;
                o_data.Message = "Успешно!";
                o_data.Status = response.StatusCode;
                return o_data;
            }
            catch (Exception ex)
            {
                ServiceResponseObject<AgreementResponse> o_data = new ServiceResponseObject<AgreementResponse>();
                o_data.Message = ex.Message;
                return o_data;
            }
        }

        private static HttpClient _httpClient;

        /// <summary>
        /// Инициализация экземпляра клиента
        /// </summary>
        /// <param name="client"></param>
        public static void InitializeClient(HttpClient client)
        {
            _httpClient = client;
        }
        public static async Task<ServiceResponseObject<SuccessResponse>> About()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"about");

                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                ServiceResponseObject<SuccessResponse> o_data = new ServiceResponseObject<SuccessResponse>();
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ErrorResponseObject error = new ErrorResponseObject();
                    error = JsonConvert.DeserializeObject<ErrorResponseObject>(s_result);
                    o_data.Status = response.StatusCode;
                    o_data.Message = error.Errors[0];
                    return o_data;
                }
                var message = JsonConvert.DeserializeObject<SuccessResponse>(s_result);
                o_data.ResponseData = message;
                o_data.Message = "Успешно!";
                o_data.Status = response.StatusCode;
                return o_data;
            }
            catch (Exception ex)
            {
                ServiceResponseObject<SuccessResponse> o_data = new ServiceResponseObject<SuccessResponse>();
                o_data.Message = ex.Message;
                return o_data;
            }
        }

        public static async Task<ServiceResponseObject<ContactsResponse>> ContactUs()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"contacts");

                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                ServiceResponseObject<ContactsResponse> o_data = new ServiceResponseObject<ContactsResponse>();
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ErrorResponseObject error = new ErrorResponseObject();
                    error = JsonConvert.DeserializeObject<ErrorResponseObject>(s_result);
                    o_data.Status = response.StatusCode;
                    o_data.Message = error.Errors[0];
                    return o_data;
                }
                var message = JsonConvert.DeserializeObject<ContactsResponse>(s_result);
                o_data.ResponseData = message;
                o_data.Message = "Успешно!";
                o_data.Status = response.StatusCode;
                return o_data;
            }
            catch (Exception ex)
            {
                ServiceResponseObject<ContactsResponse> o_data = new ServiceResponseObject<ContactsResponse>();
                o_data.Message = ex.Message;
                return o_data;
            }
        }

    }
}
