using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.RecyclerView.Extensions;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using static Android.Support.Design.Widget.AppBarLayout;
using static Android.Views.View;

namespace SmartBoxCity.Service
{
    public class HomeService
    {

        public static Android.Graphics.Bitmap GetImageBitmapFromUrl(string src)
        {
            Android.Graphics.Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(src);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }
            return imageBitmap;
        }

        public static void SetListViewHeightBasedOnChildren(ListView listView)
        {
            var listAdapter = listView.Adapter;
            if (listAdapter == null) return;
            int desiredWidth = MeasureSpec.MakeMeasureSpec(listView.Width,
                                                       MeasureSpecMode.Unspecified);
            int totalHeight = 0;
            View view = null;
            for (int i = 0; i < listAdapter.Count; i++)
            {
                view = listAdapter.GetView(i, view, listView);
                if (i == 0) view.LayoutParameters = new LayoutParams(desiredWidth, LayoutParams.WrapContent);

            view.Measure(desiredWidth, totalHeight);
                totalHeight += view.MeasuredHeight;
            }

            ViewGroup.LayoutParams params1 = listView.LayoutParameters;
            params1.Height = totalHeight + (listView.DividerHeight * (listAdapter.Count - 1));

            listView.LayoutParameters = params1;
            listView.RequestLayout();
        }
    }
}