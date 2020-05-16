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
using SmartBoxCity.Service;

namespace SmartBoxCity.Activity.Box
{
    public class ExpandableSensorDataAdapter: BaseExpandableListAdapter
    {
		readonly Context _context;
		private Dictionary<string, List<SensorData>> _listDataChild;
		public ExpandableSensorDataAdapter(Context newContext, List<string> newList, Dictionary<string, List<SensorData>> childList) : base()
		{
			_listDataChild = childList;
			_context = newContext;
			_listDataHeader = newList;
		}

		private List<string> _listDataHeader { get; set; }


		public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
		{
			View header = convertView;

			if (header == null)
			{
				header = LayoutInflater.From(_context).Inflate(Resource.Layout.list_group, null);
			}
			string textGroup = (string)GetGroup(groupPosition);
			header.FindViewById<TextView>(Resource.Id.DataHeader).Text = textGroup;

			return header;
		}

		public override void OnGroupExpanded(int groupPosition)
		{
			base.OnGroupExpanded(groupPosition);
		}
		public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
		{
			View row = convertView;
			var child = _listDataChild[_listDataHeader[groupPosition]][childPosition];
			string content = (string)GetChild(groupPosition, childPosition);
			if (row == null)
			{
				row = LayoutInflater.From(_context).Inflate(Resource.Layout.list_box_item, null);
			}
			//string newId = "", newValue = "";
			//GetChildViewHelper(groupPosition, childPosition, out newId, out newValue);
			row.FindViewById<TextView>(Resource.Id.txtSensor).Text = child.SensorName;
			row.FindViewById<TextView>(Resource.Id.txtValue).Text = child.Value;
			row.FindViewById<ImageView>(Resource.Id.img_label).SetImageResource(child.Icon);

			return row;
			//throw new NotImplementedException ();
		}

		public override int GetChildrenCount(int groupPosition)
		{
			var result = new List<SensorData>();
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
			var result = new List<SensorData>();
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