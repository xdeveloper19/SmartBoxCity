using Entity.Model;
using Entity.Model.AlarmResponse;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebService.Driver
{
    public class AlarmService
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
        /// Список всех тревог.
        /// </summary>
        /// <returns></returns>
        public static async Task<ServiceResponseObject<ListAlarmResponse>> GetAlarms()
        {
            try
            {
                //HttpResponseMessage response = await _httpClient.GetAsync($"alarms");

                //string s_result;
                //using (HttpContent responseContent = response.Content)
                //{
                //    s_result = await responseContent.ReadAsStringAsync();
                //}

                ServiceResponseObject<ListAlarmResponse> o_data =
                    new ServiceResponseObject<ListAlarmResponse>();

                //if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                //{
                //    ErrorResponseObject error = new ErrorResponseObject();
                //    error = JsonConvert.DeserializeObject<ErrorResponseObject>(s_result);
                //    o_data.Status = response.StatusCode;
                //    o_data.Message = error.Errors[0];
                //    return o_data;
                //}

                var result = new ListAlarmResponse();
                result.ALARMS.Add(new AlarmResponseData
                {
                    acknowledged = "1",
                    container_id = "00000000003",
                    id = "2",
                    name = "Протечка жидкости",
                    raised_at = DateTime.Now
                });

                result.ALARMS.Add(new AlarmResponseData
                {
                    acknowledged = "1",
                    container_id = "00000000122",
                    id = "1",
                    name = "Превышение веса",
                    raised_at = DateTime.Now
                });

                result.ALARMS.Add(new AlarmResponseData
                {
                    acknowledged = "0",
                    container_id = "00000000122",
                    id = "3",
                    name = "Несанкционированное срабатывание датчиков дверей ",
                    raised_at = DateTime.Now
                });

                o_data.Message = "Успешно!";
                o_data.Status = System.Net.HttpStatusCode.OK;// а почему переменная container_id пустая
                o_data.ResponseData = new ListAlarmResponse
                {
                   ALARMS = result.ALARMS
                };
                return o_data;
            }//can not access to close stream 
            catch (Exception ex)
            {
                ServiceResponseObject<ListAlarmResponse> o_data = new ServiceResponseObject<ListAlarmResponse>();
                o_data.Message = ex.Message;
                return o_data;
            }
        }

        /// <summary>
        /// Признать тревогу на контейнере.
        /// </summary>
        /// <returns></returns>
        public static async Task<ServiceResponseObject<SuccessResponse>> Acknowledge(string CONTAINER_ID, string ALARM_ID)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"container/{CONTAINER_ID}/alarm/{ALARM_ID}/acknowledge");

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

                var box = JsonConvert.DeserializeObject<SuccessResponse>(s_result);
                o_data.Message = "Успешно!";
                o_data.Status = response.StatusCode;// а почему переменная container_id пустая
                o_data.ResponseData = new SuccessResponse
                {
                    Message = box.Message
                };
                return o_data;
            }//can not access to close stream 
            catch (Exception ex)
            {
                ServiceResponseObject<SuccessResponse> o_data = new ServiceResponseObject<SuccessResponse>();
                o_data.Message = ex.Message;
                return o_data;
            }
        }
    }
}
