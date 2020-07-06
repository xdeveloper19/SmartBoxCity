using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Entity.Model;
using Entity.Model.BoxResponse;
using Entity.Model.BoxViewModel;
using Entity.Repository;
using Plugin.Settings;
using SmartBoxCity.Activity.Order;
using WebService;
using WebService.Driver;

namespace SmartBoxCity.Activity.Box
{
    public class BoxListActivity: Fragment
    {
        private ListView lstBox;
        private TextView txt_title_lst_box;
        private EditText editEnterOrder;
        private Boolean isDepot;
        public static List<BoxBookModel> boxlist;
        private Android.App.FragmentTransaction transaction;
        public override void OnCreate(Bundle savedInstanceState)
        {
            StaticUser.NamePadeAbsenceSomething = "BoxListActivity";
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.driver_container_book, container, false);
            lstBox = view.FindViewById<ListView>(Resource.Id.boxlistview);
            txt_title_lst_box = view.FindViewById<TextView>(Resource.Id.txt_title_lst_box);
            transaction = this.FragmentManager.BeginTransaction();

            if (Arguments != null)
            {
                isDepot = Arguments.GetBoolean("isDepot");
            }

            if (isDepot)
                txt_title_lst_box.Text = "Контейнеры на складе";
            else
                txt_title_lst_box.Text = "Контейнеры на машине";
;

            GetBoxes();
           
            return view;
        }
        private async void GetBoxes()
        {
            try
            {
                var o_data = new ServiceResponseObject<ListBoxResponse>();
                using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
                {
                    //надо было сначала клиента указать, а потом вызывать метод
                    //и обязательно с токеном
                    BoxService.InitializeClient(client);
                    o_data = await BoxService.GetContainers();

                    if (o_data.Status == HttpStatusCode.OK)
                    {

                        Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                        boxlist = new List<BoxBookModel>();
                        List<ContainerResponse> containers = new List<ContainerResponse>();
                        if (isDepot)
                            containers = o_data.ResponseData.DEPOT_CONTAINERS;
                        else
                            containers = o_data.ResponseData.CONTAINERS;

                        if (containers == null || containers.Count == 0)
                        {
                            StaticUser.NamePadeAbsenceSomething = "BoxListActivity";
                            Android.App.FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                            NotFoundOrdersActivity content = new NotFoundOrdersActivity();
                            transaction1.Replace(Resource.Id.frameDriverlayout, content);
                            transaction1.Commit();
                        }

                        int id = 1;
                        foreach (var box in containers)
                        {
                            boxlist.Add(new BoxBookModel
                            {
                                Id = box.id,
                                ImageView = (box.sensors_status.fold == "0") ? Resource.Drawable.opened_box : Resource.Drawable.close_box,
                                BoxId = "Контейнер: " + box.id,
                                AlarmDescription = (box.alarms_status.Count == 0) ? "" : "На контейнере обнаружена тревога!",
                                OrderId = (box.order_id == null) ? "нет заказа" : box.order_id
                            }
                            );
                        }

                        UpdateList();
                        lstBox.ItemClick += ListBoxes_ItemClick;
                    }
                    else
                    {
                        StaticUser.NamePadeAbsenceSomething = "BoxListActivity";
                        Android.App.FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                        NotFoundOrdersActivity content = new NotFoundOrdersActivity();
                        transaction1.Replace(Resource.Id.frameDriverlayout, content);
                        transaction1.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(Application.Context, ex.Message, ToastLength.Long);
            }
            
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