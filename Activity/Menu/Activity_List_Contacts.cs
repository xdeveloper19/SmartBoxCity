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

namespace SmartBoxCity.Activity.Menu
{
    public class Activity_List_Contacts : Fragment
    {
        private EditText EditTextTopContact;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.activity_list_contacts, container, false);

            try
            {
                EditTextTopContact = view.FindViewById<EditText>(Resource.Id.EditTextTopContact);
                EditTextTopContact.Focusable = false;
                EditTextTopContact.LongClickable = false;



            }
            catch (Exception ex)
            {
                Toast.MakeText(Context, "" + ex.Message, ToastLength.Long).Show();
            }
            return view;
        }
    }
}