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
using Entity.Repository;
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
        Button btnNext, btnSkip, btnAddOrder;
        RefLayoutManager layoutManager;
        //private Button btn_calculate;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            StaticUser.IsContentMain = true;
            layoutManager = new RefLayoutManager(Activity);
            if (!layoutManager.isFirstTimeLauch())
            {
                lauchHomeScreen();
                //Finish();
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            var view = inflater.Inflate(Resource.Layout.content_main, container, false);
            try
            {

                layouts = new int[]
                {
                     Resource.Layout.LayoutStart,
                     Resource.Layout.LayoutSlide0,
                     Resource.Layout.LayoutSlide1,
                     Resource.Layout.LayoutSlide2,
                     Resource.Layout.LayoutSlide3,
                     Resource.Layout.LayoutSlide4,
                     Resource.Layout.LayoutSlide5
                };

                viewPager = view.FindViewById<ViewPager>(Resource.Id.viewPager);
                dotsLayout = view.FindViewById<LinearLayout>(Resource.Id.layoutPanel);
                btnNext = view.FindViewById<Button>(Resource.Id.btn_next);
                btnSkip = view.FindViewById<Button>(Resource.Id.btn_skip);
                

                addDots(0);
                Android.App.FragmentTransaction transaction = this.FragmentManager.BeginTransaction();
                ViewPagerAdapter adapter = new ViewPagerAdapter(layouts, ref transaction);
                viewPager.Adapter = adapter;

                viewPager.PageSelected += ViewPager_PageSelected;
                //viewPager.AddOnPageChangeListener(new ViewPager.IOnPageChangeListener());

                btnNext.Click += (sender, e) =>
                {
                    //int current = GetItem(+1);
                    //if (current < layouts.Length)
                    //    //move to next screen
                    //    viewPager.CurrentItem = current;
                    //else
                    //{

                    //}
                    Android.App.FragmentTransaction transaction2 = this.FragmentManager.BeginTransaction();
                    //lauch main screen here
                    MainPageActivity content = new MainPageActivity();
                    transaction2.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
                };

                btnSkip.Click += (sender, e) =>
                {
                    Android.App.FragmentTransaction transaction2 = this.FragmentManager.BeginTransaction();
                    //lauch main screen here
                    ContentMainActivity content = new ContentMainActivity();
                    transaction2.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
                    //Android.App.FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                    //ContentMainActivity content4 = new ContentMainActivity();
                    //transaction1.Replace(Resource.Id.framelayout, content4).AddToBackStack(null).Commit();
                };                
                 

            }
            catch (Exception ex)
            {
                Toast.MakeText(Activity, "" + ex.Message, ToastLength.Long).Show();
            }
            return view;
        }

        public override void OnDestroyView()
        {
            StaticUser.IsContentMain = false;
            base.OnDestroyView();
        }

        public override void OnStart()
        {
            StaticUser.IsContentMain = true;
            base.OnStart();
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
            else if(e.Position == layouts[1])
            {
                
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

            string[] colorsActive = { "#d1395c", "#14a895", "#2278d4", "#a854d4", "#a854d4", "#a854d4", "#a854d4" };
            string[] colorsInactive = { "#f98da5", "#8cf9eb", "#93c6fd", "#e4b5fc", "#e4b5fc", "#e4b5fc", "#e4b5fc" };

            dotsLayout.RemoveAllViews();
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i] = new TextView(Activity);
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
            Android.App.FragmentTransaction transaction2 = this.FragmentManager.BeginTransaction();
            //lauch main screen here
            ContentMainActivity content = new ContentMainActivity();
            transaction2.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
            //Finish();
        }


        public class ViewPagerAdapter : PagerAdapter
        {
            LayoutInflater layoutInflater;
            int[] _layout;
            Android.App.FragmentTransaction transaction;

            public ViewPagerAdapter(int[] layout, ref Android.App.FragmentTransaction transaction)
            {
                _layout = layout;
                this.transaction = transaction;
            }

            public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
            {
                if (position == 1)
                {
                    layoutInflater = (LayoutInflater)Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService);
                    View view = layoutInflater.Inflate(_layout[position], container, false);
                    var btn_cost = view.FindViewById<Button>(Resource.Id.Slide0BtnAddOrder);
                    btn_cost.Click += (sender, e) =>
                    {
                        AddOrderActivity content = new AddOrderActivity();
                        transaction.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
                    };
                    container.AddView(view);
                    return view;
                }
                else
                {
                    layoutInflater = (LayoutInflater)Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService);
                    View view = layoutInflater.Inflate(_layout[position], container, false);
                    container.AddView(view);
                    return view;
                }
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