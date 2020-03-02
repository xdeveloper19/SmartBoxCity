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

namespace SmartBoxCity.Model.BoxViewModel
{
    public class SensorData: Java.Lang.Object
    {
		public string SensorName { get; set; }
		public int Icon { get; set; }
		public string Value { get; set; }

		public static Dictionary<string, List<SensorData>> listDataChild;
		public static List<string> listDataHeader;
		public SensorData()
		{
		}

		public static List<SensorData> SampleData()
		{
			var newDataList = new List<SensorData>();
			//newDataList.Add(new Data("Alabama", "Montegomery"));
			//newDataList.Add(new Data("Alaska", "Juneau"));
			//newDataList.Add(new Data("Arizona", "Pheonix"));
			//newDataList.Add(new Data("Arkansas", "Little Rock"));
			//newDataList.Add(new Data("California", "Sacramento"));
			//newDataList.Add(new Data("Colorado", "Denver"));
			//newDataList.Add(new Data("Connecticut", "Hartford"));
			return newDataList;
		}

		public static void SampleChildData(View view)
		{

			listDataHeader = new List<string>();
			listDataChild = new Dictionary<string, List<SensorData>>();

			// Adding child data
			listDataHeader.Add("Больше информации");

			// Adding child data
			var lstCS = new List<SensorData>();
			lstCS.Add(new SensorData("Вес груза, кг ", Resource.Drawable.business, "3400"));
			lstCS.Add(new SensorData("Влажность, %", Resource.Drawable.drop, "15"));
			lstCS.Add(new SensorData("Состояние", Resource.Drawable.state, "сложен"));
			lstCS.Add(new SensorData("Герметичность ", Resource.Drawable.germet, "герметичен"));
			lstCS.Add(new SensorData("Двери", Resource.Drawable.door, "закрыт"));
			lstCS.Add(new SensorData("Замок", Resource.Drawable.padlock, "закрыт"));
			lstCS.Add(new SensorData("События", Resource.Drawable.caution,"0 штук"));
			lstCS.Add(new SensorData("Тревога", Resource.Drawable.notification, "нет"));

			// Header, Child data
			listDataChild.Add(listDataHeader[0], lstCS);
		}

		public SensorData(string newId = "Temporary Id", int IconId = 1, string newValue = "Temporary Data")
		{
			SensorName = newId;
			Value = newValue;
			Icon = IconId;
		}
	}
}