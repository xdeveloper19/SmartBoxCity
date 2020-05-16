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
using Android.Support.V7.Widget;
using SmartBoxCity.Service;

namespace SmartBoxCity.Activity.Order
{
    public class ExpandableDataAdapter: BaseExpandableListAdapter
    {
		readonly Context Context;
		private Dictionary<string, List<OrderData>> _listDataChild;
		public ExpandableDataAdapter(Context newContext, List<string> newList, Dictionary<string, List<OrderData>> childList) : base()
		{
			_listDataChild = childList;
			Context = newContext;
			_listDataHeader = newList;
		}

		private List<string> _listDataHeader { get; set; }


		public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
		{
			View header = convertView;

			if (header == null)
			{
				header = LayoutInflater.From(Context).Inflate(Resource.Layout.list_group, null);
			}
			//var cardView = header.FindViewById<CardView>(Resource.Id.cardViewBox);
			//cardView.SetMinimumHeight(300);

			string textGroup = (string)GetGroup(groupPosition);
			header.FindViewById<TextView>(Resource.Id.DataHeader).Text = textGroup;

			return header;
		}

		public override void OnGroupExpanded(int groupPosition)
		{
			base.OnGroupExpanded(groupPosition);
		}

		public override void OnGroupCollapsed(int groupPosition)
		{
			base.OnGroupCollapsed(groupPosition);
		}

		
		public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
		{
			View row = convertView;
			var child = _listDataChild[_listDataHeader[groupPosition]][childPosition];
			string content = (string)GetChild(groupPosition, childPosition);
			if (row == null)
			{
				row = LayoutInflater.From(Context).Inflate(Resource.Layout.list_data_item, null);
			}
			//string newId = "", newValue = "";
			//GetChildViewHelper(groupPosition, childPosition, out newId, out newValue);
			row.FindViewById<TextView>(Resource.Id.s_sensor_data).Text = child.Value;
			row.FindViewById<TextView>(Resource.Id.sensor_name).Text = child.SensorName;
			row.FindViewById<TextView>(Resource.Id.units).Text = child.Unit;

			return row;
			//throw new NotImplementedException ();
		}

		public override int GetChildrenCount(int groupPosition)
		{
			var result = new List<OrderData>();
			_listDataChild.TryGetValue(_listDataHeader[groupPosition], out result);
			return result.Count;
		}

		public override int GroupCount
		{
			get
			{
				return _listDataHeader.Count;
			}
		}

		//private void GetChildViewHelper(int groupPosition, int childPosition, out string Id, out string Value)
		//{
		//	char letter = (char)(65 + groupPosition);
		//	List<Data> results = _listDataChild.FindAll((Data obj) => obj.Id[0].Equals(letter));
		//	Id = results[childPosition].Id;
		//	Value = results[childPosition].Value;
		//}

		#region implemented abstract members of BaseExpandableListAdapter

		public override Java.Lang.Object GetChild(int groupPosition, int childPosition)
		{
			var result = new List<OrderData>();
			_listDataChild.TryGetValue(_listDataHeader[groupPosition], out result);
			return result[childPosition];
		}

		public override long GetChildId(int groupPosition, int childPosition)
		{
			return childPosition;
		}

		public override Java.Lang.Object GetGroup(int groupPosition)
		{
			return _listDataHeader[groupPosition];
		}

		public override long GetGroupId(int groupPosition)
		{
			return groupPosition;
		}

		public override bool IsChildSelectable(int groupPosition, int childPosition)
		{
			return false;
		}

		public override bool HasStableIds
		{
			get
			{
				return false;
			}
		}

		#endregion
	}
}