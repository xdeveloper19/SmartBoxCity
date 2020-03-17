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

namespace SmartBoxCity.Activity.Home
{
    public class RefLayoutManager
    {
        ISharedPreferences sharePref;
        ISharedPreferencesEditor editor;
        Context context;

        // mode

        //shared preferene file name
        private static string pref_name = "Назад";
        private static string is_first_time_lauch = "Пропустить";



        public RefLayoutManager(Context context)
        {
            this.context = context;
            sharePref = this.context.GetSharedPreferences(pref_name, FileCreationMode.Private);
            editor = sharePref.Edit();
        }

        public void setFirstTimeLauch(bool isFirstTime)
        {
            editor.PutBoolean(is_first_time_lauch, isFirstTime);
            editor.Commit();

        }

        public bool isFirstTimeLauch()
        {
            return sharePref.GetBoolean(is_first_time_lauch, true);
        }
    }
}