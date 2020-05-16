using Entity.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebService.Driver
{
    public class DriverInfoService
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

        /// <summary>
        /// Отправка координат водителя.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<ServiceResponseObject<SuccessResponse>> PostGeoData(GeoModel model)
        {
            try
            {
                //status & message
                var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "lat", model.lat },
                        { "lng", model.lng},
                        { "gps_time", model.gps_time}
                    });

                HttpResponseMessage response = await _httpClient.PostAsync($"driver/geo", formContent);

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
                o_data.Message = message.Message;
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

        /// <summary>
        /// Готов к выполнению задач.
        /// </summary>
        /// <returns></returns>
        public static async Task<ServiceResponseObject<SuccessResponse>> Free()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"task/free");

                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                ServiceResponseObject<SuccessResponse> o_data =
                    new ServiceResponseObject<SuccessResponse>();

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ErrorResponseObject error = new ErrorResponseObject();
                    error = JsonConvert.DeserializeObject<ErrorResponseObject>(s_result);
                    o_data.Status = response.StatusCode;
                    o_data.Message = error.Errors[0];
                    return o_data;
                }

                var task = JsonConvert.DeserializeObject<SuccessResponse>(s_result);
                o_data.Message = "Успешно!";
                o_data.Status = response.StatusCode;
                o_data.ResponseData = new SuccessResponse
                {
                    Message = task.Message
                };
                return o_data;
            }//can not access to close stream 
            catch (Exception ex)
            {
                ServiceResponseObject<SuccessResponse> o_data =
                                    new ServiceResponseObject<SuccessResponse>();
                o_data.ResponseData.Message = ex.Message;
                o_data.Message = ex.Message;
                o_data.Status = System.Net.HttpStatusCode.BadRequest;
                return o_data;
            }
        }

        /// <summary>
        /// Занят для выполнения задач.
        /// </summary>
        /// <returns></returns>
        public static async Task<ServiceResponseObject<SuccessResponse>> Busy()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"task/busy");

                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                ServiceResponseObject<SuccessResponse> o_data =
                    new ServiceResponseObject<SuccessResponse>();

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ErrorResponseObject error = new ErrorResponseObject();
                    error = JsonConvert.DeserializeObject<ErrorResponseObject>(s_result);
                    o_data.Status = response.StatusCode;
                    o_data.Message = error.Errors[0];
                    return o_data;
                }

                var task = JsonConvert.DeserializeObject<SuccessResponse>(s_result);
                o_data.Message = "Успешно!";
                o_data.Status = response.StatusCode;
                o_data.ResponseData = new SuccessResponse
                {
                    Message = task.Message
                };
                return o_data;
            }//can not access to close stream 
            catch (Exception ex)
            {
                ServiceResponseObject<SuccessResponse> o_data =
                                    new ServiceResponseObject<SuccessResponse>();
                o_data.ResponseData.Message = ex.Message;
                o_data.Message = ex.Message;
                o_data.Status = System.Net.HttpStatusCode.BadRequest;
                return o_data;
            }
        }
    }
}
