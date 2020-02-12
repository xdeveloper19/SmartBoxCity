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

namespace SmartBoxCity.Model.OrderViewModel
{
	public class Data : Java.Lang.Object
	{
		public string SensorName { get; set; }
		public string Unit { get; set; }
		public string Value { get; set; }

		public static Dictionary<string, List<Data>> listDataChild;
		public static List<string> listDataHeader;
		public Data()
		{
		}

		public static List<Data> SampleData()
		{
			var newDataList = new List<Data>();
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
			listDataChild = new Dictionary<string, List<Data>>();

			// Adding child data
			listDataHeader.Add("Параметры контейнера");
			listDataHeader.Add("Состояние контейнера");

			// Adding child data
			var lstCS = new List<Data>();
			lstCS.Add(new Data("Вес груза: ", "3400", "кг"));
			lstCS.Add(new Data("Температура: ", "15", "°C"));
			lstCS.Add(new Data("Влажность: ", "45", "%"));
			lstCS.Add(new Data("Датчик света: ", "234", "лм"));

			var lstEC = new List<Data>();
			lstEC.Add(new Data("Контейнер: ", "раскрыт", ""));
			lstEC.Add(new Data("Дверь: ", "закрыта", ""));

			// Header, Child data
			listDataChild.Add(listDataHeader[0], lstCS);
			listDataChild.Add(listDataHeader[1], lstEC);
		}

		public Data(string newId = "Temporary Id", string newValue = "Temporary Data", string unit = "кг")
		{
			SensorName = newId;
			Value = newValue;
			Unit = unit;
		}
	}
}