using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace De_Verstrooide_Student
{
    [Activity(Label = "Wasmand")]
    public class Wasmand : AppCompatActivity
    {
        Intent intent2;
        ImageView wasmandImg;
        TextView wasmandText;

        string statusWasmand;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Wasmand);
            wasmandImg = FindViewById<ImageView>(Resource.Id.wasmandStatus);
            wasmandText = FindViewById<TextView>(Resource.Id.textView1);

            //als je data stuurt(status van sensor) dan ontvangt hij het als intent.extras
            if (Intent.Extras != null)
            {
                foreach (var key in Intent.Extras.KeySet())
                {
                    var value = Intent.Extras.GetString(key);
                    if (key == "status")
                    {
                        statusWasmand = value;
                    }
                }
            }

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "De Verstrooide Student";


            if (statusWasmand == "0")
            {
                Persistence.wasmandStatus = "Wasmand is leeg";
            }
            else if (statusWasmand == "1")
            {
                Persistence.wasmandStatus = "Wasmand is 50% vol";
            }
            else if (statusWasmand == "2")
            {
                Persistence.wasmandStatus = "Wasmand is volledig gevuld.";
            }
            wasmandText.Text = Persistence.wasmandStatus;
            if (Persistence.wasmandStatus == "Wasmand is leeg")
            {
                //set image als lege wasmand
                wasmandImg.SetImageResource(Resource.Mipmap.MANDLeeg);
            }
            else if (Persistence.wasmandStatus == "Wasmand is 50% vol")
            {
                //set image als lege wasmand
                wasmandImg.SetImageResource(Resource.Mipmap.MANDHalf);
            }
            else if (Persistence.wasmandStatus == "Wasmand is volledig gevuld.")
            {
                //set image als lege wasmand
                wasmandImg.SetImageResource(Resource.Mipmap.MANDVol);
            }
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            
            MenuInflater.Inflate(Resource.Menu.toolbar_menu, menu);
            IMenuItem item = menu.FindItem(Resource.Id.menu_Wasmand);
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
           
            else if (item.TitleFormatted.ToString() == "Ventilator")
            {
                intent2 = new Intent(this, typeof(Ventilator));
                intent2.AddFlags(ActivityFlags.ClearTop);
                StartActivity(intent2);
            }
            else if (item.TitleFormatted.ToString() == "Home")
            {
                intent2 = new Intent(this, typeof(MainActivity));
                intent2.AddFlags(ActivityFlags.ClearTop);
                StartActivity(intent2);
            }
            
                
            
            return base.OnOptionsItemSelected(item);
        }
    }
}