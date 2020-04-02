using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using SmartBoxCity.Model.AuthViewModel;
using SmartBoxCity.Model;
using System.Threading.Tasks;
using SmartBoxCity.Model.OrderViewModel;
using System.Net;
using System.IO;
using Plugin.Settings;
using System.Net.Http.Headers;

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

        public static async Task<ServiceResponseObject<AmountResponse>> GetOrderPrice(MakeOrderModel model)
        {
            try
            {
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://smartboxcity.ru:8003/order/rate?inception_lat=" + model.inception_lat + "&inception_lng=" + model.inception_lng + "&destination_lat=" + model.destination_lat + "&destination_lng=" + model.destination_lng + "&weight=" + model.weight + "&qty=" + model.qty + "&cargo_type=" + model.cargo_type + "&cargo_class=" + model.cargo_class + "&insurance=" + model.insurance);
                //request.Method = "GET";
               
                //var myHttpWebResponse = (HttpWebResponse)request.GetResponse();

                //Stream responseStream = myHttpWebResponse.GetResponseStream();

                //StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);

                //string s_result = myStreamReader.ReadToEnd();

                //myStreamReader.Close();
                //responseStream.Close();

                //myHttpWebResponse.Close();
                HttpResponseMessage response = await _httpClient.GetAsync($"order/rate?inception_lat={model.inception_lat}&inception_lng={model.inception_lng}&destination_lat={model.destination_lat}&destination_lng={model.destination_lng}&weight={model.weight}&qty={model.qty}&cargo_type={model.cargo_type}&cargo_class={model.cargo_class}&insurance={model.insurance}");
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
                o_data.Status = response.StatusCode;
                return o_data;
            }//can not access to close stream 
            catch (Exception ex)
            {
                ServiceResponseObject<OrderSuccessResponse> o_data = new ServiceResponseObject<OrderSuccessResponse>();
                o_data.Message = ex.Message;
                return o_data;
            }
        }
    }
}