using Entity.Model;
using Entity.Model.OrderResponse;
using Entity.Model.OrderViewModel.OrderInfoViewModel;
using EntityLibrary.Model.OrderResponse;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Repository
{
    /// <summary>
    /// Информация о текущем заказе.
    /// </summary>
    public static class StaticOrder
    {
        #region Данные о заказе
        /// <summary>
        /// Идентификатор заказа.
        /// </summary>
        public static string Order_id { get; set; }

        /// <summary>
        /// Геоданные заказа.
        /// </summary>
        public static List<GeoLocation<double>> way_points { get; set; }
        /// <summary>
        /// Количество событий.
        /// </summary>
        public static string Event_Count { get; set; }
        /// <summary>
        /// Статус оплаты.
        /// </summary>
        public static string Payment_Status { get; set; }
        /// <summary>
        /// Стоимость.
        /// </summary>
        public static string Payment_Amount { get; set; }
        /// <summary>
        /// Город отправления.
        /// </summary>
        public static string Inception_city { get; set; }
        /// <summary>
        /// Город назначения.
        /// </summary>
        public static string Destination_city { get; set; }
        /// <summary>
        /// Стоимость заказа.
        /// </summary>
        public static string Amount { get; set; }
        /// <summary>
        /// Стоимость заказа без страховки.
        /// </summary>
        public static string Work_amount { get; set; }
        /// <summary>
        /// Стоимость страховки.
        /// </summary>
        public static string Insurance_amount { get; set; }
        /// <summary>
        /// Расстояние.
        /// </summary>
        public static string Distance { get; set; }
        /// <summary>
        /// Пункт отправления.
        /// </summary>
        public static string Inception_address { get; set; }
        /// <summary>
        /// Широта отправления.
        /// </summary>
        public static string Inception_lat { get; set; }
        /// <summary>
        /// Долгота отправления.
        /// </summary>
        public static string Inception_lng { get; set; }
        /// <summary>
        /// Пункт назначения.
        /// </summary>
        public static string Destination_address { get; set; }
        /// <summary>
        /// Широта назначения.
        /// </summary>
        public static string Destination_lat { get; set; }
        /// <summary>
        /// Долгота назначения.
        /// </summary>
        public static string Destination_lng { get; set; }
        /// <summary>
        /// Длина.
        /// </summary>
        public static string Length { get; set; }
        /// <summary>
        /// Ширина.
        /// </summary>
        public static string Width { get; set; }
        /// <summary>
        /// Высота.
        /// </summary>
        public static string Height { get; set; }
        /// <summary>
        /// Вес.
        /// </summary>
        public static string Weight { get; set; }
        /// <summary>
        /// Количество мест.
        /// </summary>
        public static string Qty { get; set; }
        /// <summary>
        /// Тип груза.
        /// </summary>
        public static string Cargo_type { get; set; }
        /// <summary>
        /// Класс опасности.
        /// </summary>
        public static string Cargo_class { get; set; }
        /// <summary>
        /// Объявленная ценность груза.
        /// </summary>
        public static string Insurance { get; set; }
        /// <summary>
        /// Дата доставки.
        /// </summary>
        public static string For_date { get; set; }
        /// <summary>
        /// Время доставки.
        /// </summary>
        public static string For_time { get; set; }
        /// <summary>
        /// Почта получателя.
        /// </summary>
        public static string Receiver { get; set; }

        /// <summary>
        /// Идентификатор контейнера.
        /// </summary>
        public static string Container_Id { get; set; }
        
        /// <summary>
        /// Номер текущего этапа заказа.
        /// </summary>
        public static string Order_Stage_Id { get; set; }
        /// <summary>
        /// Тип погрузки.
        /// </summary>
        public static string Cargo_loading { get; set; }
        #endregion

        /// <summary>
        /// Добавление информации о заказе.
        /// </summary>
        /// <param name="model"></param>
        public static void AddInfoOrder(MakeOrderModel model)
        {
            Inception_address = model.inception_address;
            Inception_lat = model.inception_lat;
            Inception_lng = model.inception_lng;
            Destination_address = model.destination_address;
            Destination_lat = model.destination_lat;
            Destination_lng = model.destination_lng;
            Length = model.length;
            Width = model.width;
            Height = model.height;
            Weight = model.weight;
            Qty = model.qty;
            Cargo_type = model.cargo_type;
            Cargo_class = model.cargo_class;
            Insurance = model.insurance;
            For_date = model.for_date;
            For_time = model.for_time;
            Receiver = model.receiver;
            Cargo_loading = model.cargo_loading;
        }

        public static void CreationOrderForCosteAgain(ref MakeOrderModel model)
        {
            model.inception_address = Inception_address;
            model.inception_lat = Inception_lat;
            model.inception_lng = Inception_lng;
            model.destination_address = Destination_address;
            model.destination_lat =  Destination_lat;
            model.destination_lng = Destination_lng;
            model.length = Length;
            model.width = Width;
            model.height = Height;
            model.weight = Weight;
            model.qty = Qty;
            model.cargo_type = Cargo_type;
            model.cargo_class = Cargo_class;
            model.insurance = Insurance;
            model.for_date = For_date;
            model.for_time = For_time;
            model.receiver = Receiver;
            model.cargo_loading = Cargo_loading;
        }


        /// <summary>
        /// Добавление информации о заказе.
        /// </summary>
        /// <param name="model"></param>
        public static void AddInfoOrder(OrderParameters response)
        {
            Inception_address = response.inception_address;
            Inception_lat = response.inception_lat;
            Inception_lng = response.inception_lng;
            Destination_address = response.destination_address;
            Destination_lat = response.destination_lat;
            Destination_lng = response.destination_lng;
            Length = response.length;
            Width = response.width;
            Height = response.height;
            Weight = response.weight;
            Qty = response.qty;
            Cargo_type = response.cargo_type;
            Cargo_class = response.cargo_class;
            Insurance = response.insurance;
            Container_Id = response.container_id;
            Event_Count = response.event_count;
            Payment_Amount = response.payment_amount;
            Payment_Status = response.payment_status;
            Order_id = response.id;
            Order_Stage_Id = response.order_stage_id;
        }

        /// <summary>
        /// Добавление информации о расчете доставки.
        /// </summary>
        /// <param name="response"></param>
        public static void AddInfoAmount(AmountResponse response)
        {
            Inception_address = response.inception_address;
            Destination_address = response.destination_address;
            Distance = response.distance;
            Amount = response.amount + " руб";
            Work_amount = response.work_amount;
            Insurance_amount = response.insurance_amount;
            Inception_city = response.inception_city;
            Destination_city = response.destination_city;
        }


        /// <summary>
        /// Добавление геоданных заказа.
        /// </summary>
        /// <param name="wp"></param>
        public static void AddWayPoints(List<GeoLocation<string>> wp)
        {
            way_points = new List<GeoLocation<double>>(wp.Capacity);

            for (int i = 0; i < wp.Count; i++)
            {
                way_points.Add(new GeoLocation<double>
                {
                    lat = Convert.ToDouble(wp[i].lat.Replace(".", ",")),
                    lng = Convert.ToDouble(wp[i].lng.Replace(".", ","))
                });
            }
        }
    }
}
