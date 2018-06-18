using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace De_Verstrooide_Student
{
    [Activity(Label = "Second Activity", Theme = "@style/AppTheme", MainLauncher = false)]
    public class SecondActivity : AppCompatActivity
    {
        const string TAG = "SecondActivity";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.SecondActivity);
        }
    }
}