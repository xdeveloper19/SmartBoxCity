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
using Entity.Model.BoxViewModel;

namespace SmartBoxCity.Activity.Box
{
    public class BoxListActivity: Fragment
    {
        private ListView lstBox;
        private EditText editEnterOrder;
        public static List<BoxBookModel> boxlist;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.driver_container_book, container, false);
            lstBox = view.FindViewById<ListView>(Resource.Id.boxlistview);
            //recycleView.SetLayoutManager(new LinearLayoutManager(this));
            
            //editEnterOrder.TextChanged += EtSearch_TextChanged;
            boxlist = new List<BoxBookModel>();
            BoxBookModel p1 = new BoxBookModel()
            {
                Id = 1,
                ImageView = Resource.Drawable.opened_box,
                BoxId = "Контейнер С12378А30",
                OrderId = "нет заказа"
            };
            BoxBookModel p2 = new BoxBookModel()
            {
                Id = 2,
                ImageView = Resource.Drawable.not_opened,
                BoxId = "Контейнер 312378А30",
                OrderId = "нет заказа"
            };
            BoxBookModel p3 = new BoxBookModel()
            {
                Id = 3,
                ImageView = Resource.Drawable.opened_box,
                BoxId = "Контейнер F1G37SА30",
                OrderId = "нет заказа"
            };
            boxlist.Add(p1);
            boxlist.Add(p2);
            boxlist.Add(p3);
            UpdateList();
            lstBox.ItemClick += ListBoxes_ItemClick;
            return view;
        }

        private void ListBoxes_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Toast.MakeText(Activity, "Выбран контейнер №" + (e.Position + 1).ToString(), ToastLength.Long).Show();
        }

        public override void OnResume()
        {
            // UpdateList();
            base.OnResume();
        }

        public void UpdateList()
        {
            BoxListAdapter adapter = new BoxListAdapter(Activity, boxlist, this.FragmentManager);
            lstBox.Adapter = adapter;
        }
    }
}