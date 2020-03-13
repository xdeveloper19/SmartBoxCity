using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Text;
using Android.Views;
using Android.Widget;
using SmartBoxCity.Activity.Auth;
using SmartBoxCity.Activity.Order;
using SmartBoxCity.Activity.Registration;
using static Android.Support.V7.Widget.RecyclerView;

namespace SmartBoxCity.Activity.Home
{
    public class ContentMainActivity : Android.App.Fragment
    {

        /// <summary>
        /// Конпка прехода на форму авторизации.
        /// </summary>
        ViewPager viewPager;
        LinearLayout dotsLayout;
        TextView[] dots;
        public int[] layouts;
        Button btnNext, btnSkip;
        LayoutManager layoutManager;
        //private Button btn_calculate;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            layoutManager = new LayoutManager(this);
            if (!layoutManager.isFirstTimeLauch())
            {
                lauchHomeScreen();
                Finish();
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            var view = inflater.Inflate(Resource.Layout.content_main, container, false);
            try
            {

                layouts = new int[]
                {
                     Resource.Layout.LayoutSlide1,
                     Resource.Layout.LayoutSlide2,
                     Resource.Layout.LayoutSlide3,
                     Resource.Layout.LayoutSlide4
                };

                viewPager = view.FindViewById<ViewPager>(Resource.Id.viewPager);
                dotsLayout = view.FindViewById<LinearLayout>(Resource.Id.layoutPanel);
                btnNext = view.FindViewById<Button>(Resource.Id.btn_next);
                btnSkip = view.FindViewById<Button>(Resource.Id.btn_skip);

                addDots(0);

                ViewPagerAdapter adapter = new ViewPagerAdapter(layouts);
                viewPager.Adapter = adapter;

                viewPager.PageSelected += ViewPager_PageSelected;
                //viewPager.AddOnPageChangeListener(new ViewPager.IOnPageChangeListener());

                btnNext.Click += (sender, e) =>
                {
                    int current = GetItem(+1);
                    if (current < layouts.Length)
                        //move to next screen
                        viewPager.CurrentItem = current;
                    else
                    {
                        //lauch main screen here
                        Intent intent = new Intent(this, typeof(ContentMainActivity));
                        StartActivity(intent);

                    }
                };

                btnSkip.Click += (sender, e) =>
                {
                    Intent intent = new Intent(this, typeof(MainPageActivity));
                    StartActivity(intent);
                    //Android.App.FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                    //ContentMainActivity content4 = new ContentMainActivity();
                    //transaction1.Replace(Resource.Id.framelayout, content4).AddToBackStack(null).Commit();
                };


                //btn_calculate = view.FindViewById<Button>(Resource.Id.btn_calculate);
                //Button btn_auth1 = view.FindViewById<Button>(Resource.Id.btn_auth1);

                //btn_auth1.Click += (s, e) =>
                //{
                //    Android.App.FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                //    AuthActivity content3 = new AuthActivity();
                //    transaction1.Replace(Resource.Id.framelayout, content3).AddToBackStack(null).Commit();
                //};

                //// Переход к форме регистрации.
                //btn_calculate.Click += (s, e) =>
                //{
                //    //set alert for executing the task
                //    try
                //    {
                //        Android.App.FragmentTransaction transaction2 = this.FragmentManager.BeginTransaction();
                //        AddOrderActivity content = new AddOrderActivity();
                //        transaction2.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
                //        //Android.App.FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                //        //AlertDialog.Builder alert = new AlertDialog.Builder(Context);
                //        //alert.SetTitle("Внимание!");
                //        //alert.SetMessage("Для оформления заказа необходимо авторизироваться или зарегистрироваться.");
                //        //alert.SetPositiveButton("Регистрация", (senderAlert, args) =>
                //        //{
                //        //    alert.Dispose();
                //        //    Android.App.AlertDialog.Builder alert1 = new Android.App.AlertDialog.Builder(Context);
                //        //    alert1.SetTitle("Внимание!");
                //        //    alert1.SetMessage("Необходимо выбрать вид регистрации.");
                //        //    Android.App.FragmentTransaction transaction2 = this.FragmentManager.BeginTransaction();
                //        //    alert1.SetPositiveButton("Для физ.лица", (senderAlert1, args1) =>
                //        //    {
                //        //        Activity_Registration_Individual_Person content4 = new Activity_Registration_Individual_Person();
                //        //        transaction2.Replace(Resource.Id.framelayout, content4).AddToBackStack(null).Commit();
                //        //    });
                //        //    alert1.SetNegativeButton("Для юр.лица", (senderAlert1, args1) =>
                //        //    {
                //        //        Activity_Legal_Entity_Registration content3 = new Activity_Legal_Entity_Registration();
                //        //        transaction2.Replace(Resource.Id.framelayout, content3).AddToBackStack(null).Commit();
                //        //    });
                //        //    Dialog dialog1 = alert1.Create();
                //        //    dialog1.Show();
                //        //});
                //        //alert.SetNegativeButton("Авторизация", (senderAlert, args) =>
                //        //{
                //        //    AuthActivity content3 = new AuthActivity();
                //        //    transaction1.Replace(Resource.Id.framelayout, content3).AddToBackStack(null).Commit();
                //        //});
                //        //Dialog dialog = alert.Create();
                //        //dialog.Show();
                //    }
                //    catch (Exception ex)
                //    {
                //        Toast.MakeText(Context, "" + ex.Message, ToastLength.Long).Show();
                //    }
                //};

                // Переход к форме авторизация

            }
            catch (Exception ex)
            {
                Toast.MakeText(Context, "" + ex.Message, ToastLength.Long).Show();
            }
            return view;
        }

        void ViewPager_PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            addDots(e.Position);

            //changing the next button text
            // Next or Got it

            if (e.Position == layouts.Length - 1)
            {
                // if it is a last page. make button text to "Got it"
                btnNext.Text = (GetString(Resource.String.start));
                btnSkip.Visibility = ViewStates.Gone;

            }
            else
            {
                // if it is not a last page.
                btnNext.Text = (GetString(Resource.String.next));
                btnSkip.Visibility = ViewStates.Visible;
            }
        }


        private void addDots(int currentPage)
        {
            dots = new TextView[layouts.Length];


            string[] colorsActive = { "#d1395c", "#14a895", "#2278d4", "#a854d4" };
            string[] colorsInactive = { "#f98da5", "#8cf9eb", "#93c6fd", "#e4b5fc" };


            dotsLayout.RemoveAllViews();
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i] = new TextView(this);
                dots[i].Text = (Html.FromHtml("•")).ToString();
                dots[i].TextSize = 35;
                dots[i].SetTextColor(Color.ParseColor(colorsActive[currentPage]));
                dotsLayout.AddView(dots[i]);
            }

            if (dots.Length > 0)
            {
                dots[currentPage].SetTextColor(Color.ParseColor(colorsInactive[currentPage]));
            }
        }

        int GetItem(int i)
        {
            return viewPager.CurrentItem + i;
        }

        private void lauchHomeScreen()
        {
            layoutManager.setFirstTimeLauch(false);
            Intent intent = new Intent(this, typeof(ContentMainActivity));
            StartActivity(intent);
            Finish();
        }



        public class ViewPagerAdapter : PagerAdapter
        {
            LayoutInflater layoutInflater;
            int[] _layout;

            public ViewPagerAdapter(int[] layout)
            {
                _layout = layout;
            }

            public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
            {
                layoutInflater = (LayoutInflater)Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService);
                View view = layoutInflater.Inflate(_layout[position], container, false);
                container.AddView(view);

                return view;
            }

            public override int Count
            {
                get
                {
                    return _layout.Length;
                }
            }

            public override bool IsViewFromObject(View view, Java.Lang.Object objectValue)
            {
                return view == objectValue;
            }

            public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object objectValue)
            {
                View view = (View)objectValue;

                container.RemoveView(view);
            }
        }
        
    }
}