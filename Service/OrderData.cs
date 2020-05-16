using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SmartBoxCity.Service
{
    public class OrderData: Java.Lang.Object
    {
		public string SensorName { get; set; }
		public string Unit { get; set; }
		public string Value { get; set; }

		public static Dictionary<string, List<OrderData>> listDataChild;
		public static List<string> listDataHeader;
		public OrderData()
		{
		}

		public static List<OrderData> SampleData()
		{
			var newDataList = new List<OrderData>();
			//newDataList.Add(new Data("Alabama", "Montegomery"));
			//newDataList.Add(new Data("Alaska", "Juneau"));
			//newDataList.Add(new Data("Arizona", "Pheonix"));
			//newDataList.Add(new Data("Arkansas", "Little Rock"));
			//newDataList.Add(new Data("California", "Sacramento"));
			//newDataList.Add(new Data("Colorado", "Denver"));
			//newDataList.Add(new Data("Connecticut", "Hartford"));
			return newDataList;
		}

		public static void SampleChildData()
		{

			listDataHeader = new List<string>();
			listDataChild = new Dictionary<string, List<OrderData>>();

			// Adding child data
			listDataHeader.Add("Параметры контейнера");
			listDataHeader.Add("Состояние контейнера");

			// Adding child data
			var lstCS = new List<OrderData>();
			lstCS.Add(new OrderData("Вес груза: ", "3400", "кг"));
			lstCS.Add(new OrderData("Температура: ", "15", "°C"));
			lstCS.Add(new OrderData("Влажность: ", "45", "%"));
			lstCS.Add(new OrderData("Датчик света: ", "234", "лм"));

			var lstEC = new List<OrderData>();
			lstEC.Add(new OrderData("Контейнер: ", "раскрыт", ""));
			lstEC.Add(new OrderData("Дверь: ", "закрыта", ""));

			// Header, Child data
			listDataChild.Add(listDataHeader[0], lstCS);
			listDataChild.Add(listDataHeader[1], lstEC);
		}

		public OrderData(string newId = "Temporary Id", string newValue = "Temporary Data", string unit = "кг")
		{
			SensorName = newId;
			Value = newValue;
			Unit = unit;
		}
	}
}