using Entity.Model;
using Entity.Model.TaskResponse;
using Newtonsoft.Json;
using Plugin.Settings;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebService.Driver
{
    public class TaskService
    {
        //eminem river
        //Aka - Right now
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
        /// Получение фото.
        /// </summary>
        /// <returns></returns>
        public static async Task<ServiceResponseObject<SuccessResponse>> GetPhoto()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://smartboxcity.ru:8003/container/00000000002/image");
                request.Method = "GET";
                request.Credentials = new NetworkCredential(CrossSettings.Current.GetValueOrDefault("token", ""), "");

                var response = (HttpWebResponse)request.GetResponse();

                Stream responseStream = response.GetResponseStream();



                StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);

                string s_result = myStreamReader.ReadToEnd();

                myStreamReader.Close();
                responseStream.Close();

                response.Close();
                //HttpResponseMessage response = await _httpClient.GetAsync($"tasks");

                //string s_result;
                //using (HttpContent responseContent = response.Content)
                //{
                //    s_result = await responseContent.ReadAsStringAsync();
                //}

                ServiceResponseObject<SuccessResponse> o_data =
                    new ServiceResponseObject<SuccessResponse>();

                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        {
                            ErrorResponseObject error = new ErrorResponseObject();
                            error = JsonConvert.DeserializeObject<ErrorResponseObject>(s_result);
                            o_data.Status = response.StatusCode;
                            o_data.Message = error.Errors[0];
                            return o_data;
                        }
                    case HttpStatusCode.InternalServerError:
                        {
                            throw new Exception("Внутренняя ошибка сервера 500");
                        }

                    case HttpStatusCode.NotFound:
                        {
                            throw new Exception("Ресурс не найден 404");
                        }
                    case HttpStatusCode.OK:
                        {
                            var tasks = JsonConvert.DeserializeObject<SuccessResponse>(s_result);
                            o_data.Message = "Успешно!";
                            o_data.Status = response.StatusCode;
                            o_data.ResponseData = new SuccessResponse
                            {
                                Message = tasks.Message
                            };
                            return o_data;
                        }
                    default:
                        {
                            throw new Exception(response.StatusCode.ToString() + " Server Error");
                        }
                }


            }//can not access to close stream 
            catch (Exception ex)
            {
                ServiceResponseObject<SuccessResponse> o_data =
                   new ServiceResponseObject<SuccessResponse>();

                o_data.Message = ex.Message;
                o_data.Status = System.Net.HttpStatusCode.InternalServerError;
                return o_data;
            }
        }
        /// <summary>
        /// Получение задач.
        /// </summary>
        /// <returns></returns>
        public static async Task<ServiceResponseObject<ListTaskResponse>> GetTasks()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://smartboxcity.ru:8003/tasks");
                request.Method = "GET";
                request.Credentials = new NetworkCredential(CrossSettings.Current.GetValueOrDefault("token", ""), "");

                var response = (HttpWebResponse)request.GetResponse();

                Stream responseStream = response.GetResponseStream();



                StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);

                string s_result = myStreamReader.ReadToEnd();

                myStreamReader.Close();
                responseStream.Close();

                response.Close();
                //HttpResponseMessage response = await _httpClient.GetAsync($"tasks");

                //string s_result;
                //using (HttpContent responseContent = response.Content)
                //{
                //    s_result = await responseContent.ReadAsStringAsync();
                //}

                ServiceResponseObject<ListTaskResponse> o_data =
                    new ServiceResponseObject<ListTaskResponse>();

                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        {
                            ErrorResponseObject error = new ErrorResponseObject();
                            error = JsonConvert.DeserializeObject<ErrorResponseObject>(s_result);
                            o_data.Status = response.StatusCode;
                            o_data.Message = error.Errors[0];
                            return o_data;
                        }
                    case HttpStatusCode.InternalServerError:
                        {
                            throw new Exception("Внутренняя ошибка сервера 500");
                        }

                    case HttpStatusCode.NotFound:
                        {
                            throw new Exception("Ресурс не найден 404");
                        }
                    case HttpStatusCode.OK:
                        {
                            var tasks = JsonConvert.DeserializeObject<ListTaskResponse>(s_result);
                            o_data.Message = "Успешно!";
                            o_data.Status = response.StatusCode;// а почему переменная container_id пустая
                            o_data.ResponseData = new ListTaskResponse
                            {
                                DRIVER = tasks.DRIVER,
                                TASKS = tasks.TASKS,
                                MAP_WAYPOINTS = tasks.MAP_WAYPOINTS,
                                CONTAINERS = tasks.CONTAINERS
                            };
                            return o_data;
                        }
                    default:
                        {
                            throw new Exception(response.StatusCode.ToString() + " Server Error");
                        }
                }

               
            }//can not access to close stream 
            catch (Exception ex)
            {
                ServiceResponseObject<ListTaskResponse> o_data =
                   new ServiceResponseObject<ListTaskResponse>();

                o_data.Message = ex.Message;
                o_data.Status = System.Net.HttpStatusCode.InternalServerError;
                return o_data;
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
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://smartboxcity.ru:8003/task/" + task_id + "/done?container_id=" + box_id);
                //request.Method = "GET";
                //request.Credentials = new NetworkCredential(CrossSettings.Current.GetValueOrDefault("token", ""), "");

                //var response = (HttpWebResponse)request.GetResponse();

                //Stream responseStream = response.GetResponseStream();



                //StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);

                //string s_result = myStreamReader.ReadToEnd();

                //myStreamReader.Close();
                //responseStream.Close();

                //response.Close();
                HttpResponseMessage response = await _httpClient.GetAsync($"task/{task_id}/done?container_id={box_id}");

                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                ServiceResponseObject<SuccessResponse> o_data =
                    new ServiceResponseObject<SuccessResponse>();

                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        {
                            ErrorResponseObject error = new ErrorResponseObject();
                            error = JsonConvert.DeserializeObject<ErrorResponseObject>(s_result);
                            o_data.Status = response.StatusCode;
                            o_data.Message = error.Errors[0];
                            return o_data;
                        }
                    case HttpStatusCode.InternalServerError:
                        {
                            throw new Exception("Внутренняя ошибка сервера 500");
                        }

                    case HttpStatusCode.NotFound:
                        {
                            throw new Exception("Ресурс не найден 404");
                        }
                    case HttpStatusCode.OK:
                        {
                            var task = JsonConvert.DeserializeObject<SuccessResponse>(s_result);
                            o_data.Message = "Успешно!";
                            o_data.Status = response.StatusCode;
                            o_data.ResponseData = new SuccessResponse
                            {
                                Message = task.Message
                            };
                            return o_data;
                        }
                    default:
                        {
                            throw new Exception(response.StatusCode.ToString() + " Server Error");
                        }
                }

               
            }//can not access to close stream 
            catch (Exception ex)
            {
                ServiceResponseObject<SuccessResponse> o_data =
                                    new ServiceResponseObject<SuccessResponse>();
                o_data.ResponseData.Message = ex.Message;
                o_data.Message = ex.Message;
                o_data.Status = System.Net.HttpStatusCode.InternalServerError;
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
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://smartboxcity.ru:8003/task/abort?busy_comment=" + comment);
                request.Method = "GET";
                request.Credentials = new NetworkCredential(CrossSettings.Current.GetValueOrDefault("token", ""), "");

                var response = (HttpWebResponse)request.GetResponse();

                Stream responseStream = response.GetResponseStream();



                StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);

                string s_result = myStreamReader.ReadToEnd();

                myStreamReader.Close();
                responseStream.Close();

                response.Close();
                //HttpResponseMessage response = await _httpClient.GetAsync($"task/abort?busy_comment={comment}");

                //string s_result;
                //using (HttpContent responseContent = response.Content)
                //{
                //    s_result = await responseContent.ReadAsStringAsync();
                //}

                ServiceResponseObject<SuccessResponse> o_data =
                    new ServiceResponseObject<SuccessResponse>();

                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        {
                            ErrorResponseObject error = new ErrorResponseObject();
                            error = JsonConvert.DeserializeObject<ErrorResponseObject>(s_result);
                            o_data.Status = response.StatusCode;
                            o_data.Message = error.Errors[0];
                            return o_data;
                        }
                    case HttpStatusCode.InternalServerError:
                        {
                            throw new Exception("Внутренняя ошибка сервера 500");
                        }

                    case HttpStatusCode.NotFound:
                        {
                            throw new Exception("Ресурс не найден 404");
                        }
                    case HttpStatusCode.OK:
                        {
                            var task = JsonConvert.DeserializeObject<SuccessResponse>(s_result);
                            o_data.Message = "Успешно!";
                            o_data.Status = response.StatusCode;
                            o_data.ResponseData = new SuccessResponse
                            {
                                Message = task.Message
                            };
                            return o_data;
                        }
                    default:
                        {
                            throw new Exception(response.StatusCode.ToString() + " Server Error");
                        }
                }

               
            }//can not access to close stream 
            catch (Exception ex)
            {
                ServiceResponseObject<SuccessResponse> o_data =
                                    new ServiceResponseObject<SuccessResponse>();
                //o_data.ResponseData.Message = ex.Message;
                o_data.Message = ex.Message;
                o_data.Status = System.Net.HttpStatusCode.InternalServerError;
                return o_data;
            }
        }
    }
}
