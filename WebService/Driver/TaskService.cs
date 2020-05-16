using Entity.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebService.Driver
{
    public class TaskService
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
        /// Получение задач.
        /// </summary>
        /// <returns></returns>
        public static async Task<ServiceResponseObject<BaseResponseObject>> GetTasks()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"tasks");

                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                ServiceResponseObject<BaseResponseObject> o_data =
                    new ServiceResponseObject<BaseResponseObject>();

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    //ErrorResponseObject error = new ErrorResponseObject();
                    //error = JsonConvert.DeserializeObject<ErrorResponseObject>(s_result);
                    //o_data.Status = response.StatusCode;
                    //o_data.Message = error.Errors[0];
                    //return o_data;
                }
                //var order = JsonConvert.DeserializeObject<ListResponse<OrderResponse, ArchiveResponse>>(s_result);
                o_data.Message = "Успешно!";
                o_data.Status = response.StatusCode;// а почему переменная container_id пустая
                //o_data.ResponseData = new ListResponse<OrderResponse, ArchiveResponse>
                //{
                //    ORDERS = order.ORDERS,
                //    ARCHIVE = order.ARCHIVE
                //};
                return o_data;
            }//can not access to close stream 
            catch (Exception ex)
            {

                return null;
            }
        }


        /// <summary>
        /// Выполнение задачи водителем.
        /// </summary>
        /// <returns></returns>
        public static async Task<ServiceResponseObject<SuccessResponse>> CompleteTask(string task_id, string box_id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"task/{task_id}/done?container_id={box_id}");

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
        /// Отказаться от задачи водителем.
        /// </summary>
        /// <returns></returns>
        public static async Task<ServiceResponseObject<SuccessResponse>> Abort(string comment)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"task/abort?busy_comment={comment}");

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
