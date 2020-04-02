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
using Android.Util;
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
            try
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
            catch (Exception ex)
            {
                ServiceResponseObject<AuthResponseData> o_data = new ServiceResponseObject<AuthResponseData>();
                o_data.Message = ex.Message;
                return o_data;
            }
        }

        public static async Task Login(string token)
        {
            try
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
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<ServiceResponseObject<RegisterResponseData>> RegisterIndividual(RegisterIndividualModel model)
        {
            try
            {
                //status & message
                var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "login", model.Login },
                        { "password", model.Password},
                        { "email", model.Email},
                        { "phone", model.Phone},
                        { "client_type", model.ClientType},
                        { "client_last_name", model.ClientLastName},
                        { "client_name", model.ClientName},
                        { "client_patronymic", model.ClientPatronymic},
                        { "client_birthday", model.ClientBirthday},
                        { "client_passport_serie", model.ClientPassportSerie},
                        { "client_passport_id", model.ClientPassportId},
                        { "client_passport_code", model.ClientPassportCode}
                    });

                HttpResponseMessage response = await _httpClient.PostAsync($"client", formContent);

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
                o_data.Message = message.Message;
                o_data.Status = response.StatusCode;
                return o_data;
            }
            catch (Exception ex)
            {
                ServiceResponseObject<RegisterResponseData> o_data = new ServiceResponseObject<RegisterResponseData>();
                o_data.Message = ex.Message;
                return o_data;
            }
        }

        public static async Task<ServiceResponseObject<RegisterResponseData>> RegisterLegal(RegisterLegalModul model)
        {
            try
            {
                //status & message
                var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "login", model.Login },
                        { "password", model.Password},
                        { "email", model.Email},
                        { "org_phone", model.OrgPhone},
                        { "client_type", model.ClientType},
                        { "client_last_name", model.ClientLastName},
                        { "client_name", model.ClientName},
                        { "client_patronymic", model.ClientPatronymic},
                        { "org_postal_address", model.OrgPostalAddress},
                        { "org_name", model.OrgName},
                        { "org_kpp", model.OrgKpp},
                        { "org_inn", model.OrgInn},
                        { "org_ogrn", model.OrgOgrn},
                        { "org_bank", model.OrgBank},
                        { "org_bank_payment", model.OrgBankpayment},
                        { "org_bank_correspondent", model.OrgBankCorrespondent},
                        { "org_bank_bik", model.OrgBankBik},
                        { "org_legal_address", model.OrgLegalAddress}
                    });

                HttpResponseMessage response = await _httpClient.PostAsync($"client", formContent);

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
                o_data.Message = message.Message;
                o_data.Status = response.StatusCode;
                return o_data;
            }
            catch (Exception ex)
            {
                ServiceResponseObject<RegisterResponseData> o_data = new ServiceResponseObject<RegisterResponseData>();
                o_data.Message = ex.Message;
                return o_data;
            }
        }

        public static async Task<ServiceResponseObject<AgreementResponseData>> Offer()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"agreement/offer");

                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                ServiceResponseObject<AgreementResponseData> o_data = new ServiceResponseObject<AgreementResponseData>();
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ErrorResponseObject error = new ErrorResponseObject();
                    error = JsonConvert.DeserializeObject<ErrorResponseObject>(s_result);
                    o_data.Status = response.StatusCode;
                    o_data.Message = error.Errors[0];
                    return o_data;
                }
                var message = JsonConvert.DeserializeObject<AgreementResponseData>(s_result);
                o_data.ResponseData = message;
                o_data.Message = "Успешно!";
                o_data.Status = response.StatusCode;
                return o_data;
            }
            catch (Exception ex)
            {
                ServiceResponseObject<AgreementResponseData> o_data = new ServiceResponseObject<AgreementResponseData>();
                o_data.Message = ex.Message;
                Log.Debug("Error occuired getting offer response", ex.Message);
                return o_data;
            }
        }

        public static async Task<ServiceResponseObject<AgreementResponseData>> Privacy()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"agreement/privacy");

                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                ServiceResponseObject<AgreementResponseData> o_data = new ServiceResponseObject<AgreementResponseData>();
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ErrorResponseObject error = new ErrorResponseObject();
                    error = JsonConvert.DeserializeObject<ErrorResponseObject>(s_result);
                    o_data.Status = response.StatusCode;
                    o_data.Message = error.Errors[0];
                    return o_data;
                }
                var message = JsonConvert.DeserializeObject<AgreementResponseData>(s_result);
                o_data.ResponseData = message;
                o_data.Message = "Успешно!";
                o_data.Status = response.StatusCode;
                return o_data;
            }
            catch (Exception ex)
            {
                ServiceResponseObject<AgreementResponseData> o_data = new ServiceResponseObject<AgreementResponseData>();
                o_data.Message = ex.Message;
                return o_data;
            }
        }
        public static async Task LogOut()
        {
            try
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
            catch (Exception ex)
            {
                Log.Debug("Error log out", ex.Message); 
                throw;
            }
        }
    }
}