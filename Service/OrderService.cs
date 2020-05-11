using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using SmartBoxCity.Model.AuthViewModel;
using SmartBoxCity.Model;
using System.Threading.Tasks;
using SmartBoxCity.Model.OrderViewModel;
using System.Net;
using System.IO;
using Plugin.Settings;
using System.Net.Http.Headers;
using SmartBoxCity.Model.BoxViewModel;

namespace SmartBoxCity.Service
{
    public class OrderService
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
        /// Получение предварительной стоимости заказа
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<ServiceResponseObject<AmountResponse>> GetOrderPrice(MakeOrderModel model)
        {
            try
            {
                #region Пример HttpWebRequest
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://smartboxcity.ru:8003/order/rate?inception_lat=" + model.inception_lat + "&inception_lng=" + model.inception_lng + "&destination_lat=" + model.destination_lat + "&destination_lng=" + model.destination_lng + "&weight=" + model.weight + "&qty=" + model.qty + "&cargo_type=" + model.cargo_type + "&cargo_class=" + model.cargo_class + "&insurance=" + model.insurance);
                //request.Method = "GET";

                //var myHttpWebResponse = (HttpWebResponse)request.GetResponse();

                //Stream responseStream = myHttpWebResponse.GetResponseStream();

                //StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);

                //string s_result = myStreamReader.ReadToEnd();

                //myStreamReader.Close();
                //responseStream.Close();

                //myHttpWebResponse.Close();
                #endregion
                HttpResponseMessage response = await _httpClient.GetAsync($"order/rate?inception_lat={model.inception_lat}" +
                    $"&inception_lng={model.inception_lng}" +
                    $"&destination_lat={model.destination_lat}&" +
                    $"destination_lng={model.destination_lng}&" +
                    $"weight={model.weight}&qty={model.qty}&" +
                    $"cargo_type={model.cargo_type}&" +
                    $"cargo_class={model.cargo_class}&" +
                    $"insurance={model.insurance}");
                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                ServiceResponseObject<AmountResponse> o_data = new ServiceResponseObject<AmountResponse>();
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ErrorResponseObject error = new ErrorResponseObject();
                    error = JsonConvert.DeserializeObject<ErrorResponseObject>(s_result);
                    o_data.Status = response.StatusCode;
                    o_data.Message = error.Errors[0];
                    return o_data;
                }
                o_data.ResponseData = JsonConvert.DeserializeObject<AmountResponse>(s_result);
                o_data.Message = "Успешно!";
                o_data.Status = response.StatusCode;
                return o_data;
            }
            catch (Exception ex)
            {
                ServiceResponseObject<AmountResponse> o_data = new ServiceResponseObject<AmountResponse>();
                o_data.Message = ex.Message;
                return o_data;
            }
        }


        /// <summary>
        /// Получение данных заказа
        /// </summary>
        /// <returns></returns>
        public static async Task<ServiceResponseObject<SensorResponse>> GetSensorParameters()
        {
            try
            {

                HttpResponseMessage response = await _httpClient.GetAsync($"order/HI9139663364");

                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                ServiceResponseObject<SensorResponse> o_data = new ServiceResponseObject<SensorResponse>();
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ErrorResponseObject error = new ErrorResponseObject();
                    error = JsonConvert.DeserializeObject<ErrorResponseObject>(s_result);
                    o_data.Status = response.StatusCode;
                    o_data.Message = error.Errors[0];
                    return o_data;
                }
                var order = JsonConvert.DeserializeObject<ListResponse<SensorResponse, ArchiveResponse>> (s_result);
                o_data.Message = "Успешно!";
                o_data.Status = response.StatusCode;
                o_data.ResponseData = JsonConvert.DeserializeObject<SensorResponse>(s_result);
                return o_data;
            }//can not access to close stream 
            catch (Exception ex)
            {
                ServiceResponseObject<SensorResponse> o_data = new ServiceResponseObject<SensorResponse>();
                o_data.Message = ex.Message;
                return o_data;
            }
        }

        /// <summary>
        /// Оформление заявки на заказ
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<ServiceResponseObject<OrderSuccessResponse>> AddOrder(MakeOrderModel model)
        {
            try
            {
                //status & message
                var formContent = new Dictionary<string, string>
                    {
                        { "inception_address", model.inception_address },
                        { "inception_lat", model.inception_lat},
                        { "inception_lng", model.inception_lng},
                        { "destination_address", model.destination_address},
                        { "destination_lng", model.destination_lng},
                        { "width", model.width},
                        { "length", model.length},
                        { "destination_lat", model.destination_lat},
                        { "height", model.height},
                        { "weight", model.weight},
                        { "qty", model.qty},
                        { "cargo_type", model.cargo_type},
                        { "cargo_class", model.cargo_class},
                        { "insurance", model.insurance},
                        { "for_date", model.for_date},
                        { "for_time", model.for_time},
                        { "receiver", model.receiver},
                        { "cargo_loading", model.cargo_loading}
                    };

                string newData = "";

                foreach (string key in formContent.Keys)
                {
                    newData += key + "="
                          + formContent[key] + "&";
                }

                var postData = newData.Remove(newData.Length - 1, 1);
                #region Пример HttpWebRequest
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://smartboxcity.ru:8003/order?" + postData);
                //request.Method = "GET";
                //request.Credentials = new NetworkCredential(CrossSettings.Current.GetValueOrDefault("token", ""), "");

                //byte[] data = Encoding.ASCII.GetBytes(postData);

                //request.ContentType = "application/x-www-form-urlencoded";
                //request.ContentLength = data.Length;

                //Stream requestStream = request.GetRequestStream();
                //requestStream.Write(data, 0, data.Length);
                //requestStream.Close();
                //HttpWebResponse myHttpWebResponse = (HttpWebResponse)request.GetResponse();

                //Stream responseStream = myHttpWebResponse.GetResponseStream();

                //StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);

                //string s_result = myStreamReader.ReadToEnd();

                //myStreamReader.Close();
                //responseStream.Close();

                //myHttpWebResponse.Close();

                #endregion
                var authValue = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{CrossSettings.Current.GetValueOrDefault("token", "")}:")));

                var client = new HttpClient()
                {
                    DefaultRequestHeaders = { Authorization = authValue },
                    BaseAddress = new Uri("https://smartboxcity.ru/order/add/")
                    //Set some other client defaults like timeout / BaseAddress
                };

                HttpResponseMessage response = await client.GetAsync($"?" + postData);
                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                ServiceResponseObject<OrderSuccessResponse> o_data = new ServiceResponseObject<OrderSuccessResponse>();
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ErrorResponseObject error = new ErrorResponseObject();
                    error = JsonConvert.DeserializeObject<ErrorResponseObject>(s_result);
                    o_data.Status = response.StatusCode;
                    o_data.Message = error.Errors[0];
                    return o_data;
                }
                var message = JsonConvert.DeserializeObject<OrderSuccessResponse>(s_result);
                o_data.Message = "Заявка успешна оформлена!";
                o_data.ResponseData = new OrderSuccessResponse
                {
                    order_id = message.order_id
                };
                o_data.Status = response.StatusCode;
                return o_data;
            }//can not access to close stream 
            catch (Exception ex)
            {
                ServiceResponseObject<OrderSuccessResponse> o_data = new ServiceResponseObject<OrderSuccessResponse>();
                o_data.Message = ex.Message;//Message	"Error converting value 401 to type 'SmartBoxCity.Model.OrderViewModel.OrderSuccessResponse'. Path ''…"	string

                return o_data;
            }
        }


        /// <summary>
        /// Получение заказов
        /// </summary>
        /// <returns></returns>
        public static async Task<ServiceResponseObject<ListResponse<OrderResponse,ArchiveResponse>>> GetOrders(HttpClient client)
        {
            try
            {
                //в клиентте уже прописан основной адрес
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://smartboxcity.ru/orders/?id=54654645");
                //request.Method = "GET";
                //request.Credentials = new NetworkCredential(CrossSettings.Current.GetValueOrDefault("token", ""), "");             
                //request.ContentType = "application/x-www-form-urlencoded";
                //request.ContentLength = data.Length;
                //Stream requestStream = request.GetRequestStream();
                //requestStream.Write(data, 0, data.Length);
                //requestStream.Close();
                //HttpWebResponse myHttpWebResponse = (HttpWebResponse)request.GetResponse();

                //Stream responseStream = myHttpWebResponse.GetResponseStream();

                //StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);

                //string s_result = myStreamReader.ReadToEnd();// "\n<!DOCTYPE html>\r\n<html>\r\n<head>\r\n  <meta charset=\"utf-8\">\r\n  <meta http-equiv=\"X-UA-Compatible\" con…"	string


                //myStreamReader.Close();
                //responseStream.Close();

                //myHttpWebResponse.Close();
                //HttpResponseMessage response = await _httpClient.GetAsync($"orders");// !! ?
                //string s_result;
                //using (HttpContent responseContent = response.Content)
                //{
                //    s_result = await responseContent.ReadAsStringAsync();
                //}

                
                HttpResponseMessage response = await client.GetAsync($"orders/?API");

                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                ServiceResponseObject<ListResponse<OrderResponse, ArchiveResponse>> o_data = 
                    new ServiceResponseObject<ListResponse<OrderResponse, ArchiveResponse>>();

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ErrorResponseObject error = new ErrorResponseObject();
                    error = JsonConvert.DeserializeObject<ErrorResponseObject>(s_result);
                    o_data.Status = response.StatusCode;
                    o_data.Message = error.Errors[0];
                    return o_data;
                }
                var order = JsonConvert.DeserializeObject<ListResponse<OrderResponse, ArchiveResponse>>(s_result);
                o_data.Message = "Успешно!";
                o_data.Status = response.StatusCode;// а почему переменная container_id пустая
                o_data.ResponseData = new ListResponse<OrderResponse, ArchiveResponse>
                {
                    ORDERS = order.ORDERS,
                    ARCHIVE = order.ARCHIVE
                };
                return o_data;
            }//can not access to close stream 
            catch (Exception ex)
            {
                ServiceResponseObject<ListResponse<OrderResponse, ArchiveResponse>> o_data =
                    new ServiceResponseObject<ListResponse<OrderResponse, ArchiveResponse>>();
                o_data.Message = ex.Message;
                return o_data;
            }
        }

    }
}