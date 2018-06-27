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
    /// <summary>
    /// De homepage je je token kan loggen zodat je notifcatie en statussen kan krijgen
    /// </summary>
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        TextView msgText;
        const string TAG = "MainActivity";
        string status = "";
        string click_action = "";
        Intent intent;
        Intent intent2;

        /// <summary>
        /// Maakt de activity aan en zorgt voor de standaard foto en text, layout
        /// </summary>
        /// <param name="savedInstanceState"> onthoud de stand van de telefoon. Horizontaal of verticaal.</param>
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
        /// <summary>
        /// Kijkt of je wel connectie heb met firebace
        /// </summary>
        /// <returns>returnt of firebase  beschikbaar is of niet</returns>
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
        /// <summary>
        /// Maakt menu voor navigatie door de app. Staat in de toolbar.
        /// </summary>
        /// <param name="menu"> Opent juiste menu</param>
        /// <returns>stuurt het juiste menu terug</returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.toolbar_menu, menu);
            IMenuItem item = menu.FindItem(Resource.Id.menu_Home);
            item.SetVisible(false);
            return base.OnCreateOptionsMenu(menu);
        }
        /// <summary>
        /// Navigeer naar de layout waar je op klikt
        /// </summary>
        /// <param name="item"> Hij stuurt terug naar wat voor pagina de knop verwijst.</param>
        /// <returns> Hij navigeert naar de gedrukte pagina</returns>
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
            else if (item.TitleFormatted.ToString() == "KoffieZetApparaat")
            {
                intent2 = new Intent(this, typeof(KoffieZetApparaat));
                intent2.AddFlags(ActivityFlags.ClearTop);
                StartActivity(intent2);
            }

            return base.OnOptionsItemSelected(item);
        }

    }
}

