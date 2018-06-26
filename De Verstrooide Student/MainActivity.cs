using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Gms.Common;
using Firebase.Messaging;
using Firebase.Iid;
using Android.Util;
using System;
using Android.Content;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;

namespace De_Verstrooide_Student
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        TextView msgText;
        const string TAG = "MainActivity";
        string status = "";
        string click_action = "";
        Intent intent;
        Intent intent2;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "De Verstrooide Student";
            msgText = FindViewById<TextView>(Resource.Id.msgText);

            if (Intent.Extras != null)
            {
                foreach (var key in Intent.Extras.KeySet())
                {
                    var value = Intent.Extras.GetString(key);
                    Log.Debug(TAG, "Key: {0} Value: {1}", key, value);
                    if (key == "click_action")
                    {
                        click_action = value;
                    }
                    else if (key == "status")
                    {
                        status = value;
                    }
                }
                if (click_action.Equals("Kliko"))
                {
                    intent = new Intent(this, typeof(Kliko));
                    intent.AddFlags(ActivityFlags.ClearTop);
                    intent.PutExtra("status", status);
                    StartActivity(intent);
                }
                else if (click_action.Equals("Koelkast"))
                {
                    intent = new Intent(this, typeof(Koelkast));
                    intent.AddFlags(ActivityFlags.ClearTop);
                    intent.PutExtra("status", status);
                    StartActivity(intent);
                }
                else if (click_action.Equals("Ventilator"))
                {
                    intent = new Intent(this, typeof(Ventilator));
                    intent.AddFlags(ActivityFlags.ClearTop);
                    intent.PutExtra("status", status);
                    StartActivity(intent);
                }
                else if(click_action.Equals("Wasmand"))
                {
                    intent = new Intent(this, typeof(Wasmand));
                    intent.AddFlags(ActivityFlags.ClearTop);
                    intent.PutExtra("status", status);
                    StartActivity(intent);
                }
                else if(click_action.Equals("KoffieZetApparaat"))
                {
                    intent = new Intent(this, typeof(KoffieZetApparaat));
                    intent.AddFlags(ActivityFlags.ClearTop);
                    intent.PutExtra("status", status);
                    StartActivity(intent);
                }
            }

            IsPlayServicesAvailable();

            var logTokenButton = FindViewById<Button>(Resource.Id.logTokenButton);
            logTokenButton.Click += delegate
            {
                Log.Debug(TAG, "InstanceID token: {0}", FirebaseInstanceId.Instance.Token);
            };

            var subscribeButton = FindViewById<Button>(Resource.Id.subscribeButton);
            subscribeButton.Click += delegate
            {
                FirebaseMessaging.Instance.SubscribeToTopic("news");
                Log.Debug(TAG, "Subscribed to remote notifications");
            };
        }

        bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if(resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    msgText.Text = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                }
                else
                {
                    msgText.Text = "This device is not supported";
                    Finish();
                }
                return false;
            }
            else
            {
                msgText.Text = "Google Play Services is available";
                return true;
            }
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.toolbar_menu, menu);
            IMenuItem item = menu.FindItem(Resource.Id.menu_Home);
            item.SetVisible(false);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.TitleFormatted.ToString() == "Koelkast")
            {
                intent2 = new Intent(this, typeof(Koelkast));
                intent2.AddFlags(ActivityFlags.ClearTop);
                StartActivity(intent2);
            }
            else if (item.TitleFormatted.ToString() == "Kliko")
            {
                intent2 = new Intent(this, typeof(Kliko));
                intent2.AddFlags(ActivityFlags.ClearTop);
                StartActivity(intent2);
            }
            else if (item.TitleFormatted.ToString() == "Wasmand")
            {
                intent2 = new Intent(this, typeof(Wasmand));
                intent2.AddFlags(ActivityFlags.ClearTop);
                StartActivity(intent2);
            }
            else if (item.TitleFormatted.ToString() == "Ventilator")
            {
                intent2 = new Intent(this, typeof(Ventilator));
                intent2.AddFlags(ActivityFlags.ClearTop);
                StartActivity(intent2);
            }

            return base.OnOptionsItemSelected(item);
        }

    }
}

