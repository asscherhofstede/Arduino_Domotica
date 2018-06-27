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
    [Activity(Label = "KoffieZetApparaat")]
    public class KoffieZetApparaat : AppCompatActivity
    {
        TextView koffieZetApparaat;
        ImageView koffieFoto;
        Intent intent2;
        string statusKoffieZetApparaat;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.KoffieZetApparaat);
            koffieZetApparaat = FindViewById<TextView>(Resource.Id.koffieZetApparaat_status);
            koffieFoto = FindViewById<ImageView>(Resource.Id.koffieFoto);

            if (Intent.Extras != null)
            {
                foreach (var key in Intent.Extras.KeySet())
                {
                    var value = Intent.Extras.GetString(key);
                    if (key == "status")
                    {
                        statusKoffieZetApparaat = value;
                    }
                }
            }

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "De Verstrooide Student";

            if (Persistence.koffieZetApparaatValue == "0")
            {
                //foto van je kliko nog bij huis
                Persistence.koffieZetApparaatStatus = "Koffie is uit";
            }
            else if (Persistence.koffieZetApparaatValue == "1")
            {
                //foto van kliko aan de straat
                Persistence.koffieZetApparaatStatus = "Koffie is aan!";
            }
            else if (Persistence.koffieZetApparaatValue == null)
            {
                Persistence.koffieZetApparaatStatus = "Onze sensoren kunnen niet zien of de koffiezetapparaat aan of uit is!";
                koffieFoto.SetImageResource(Resource.Mipmap.MANDLeeg);
            }
            koffieZetApparaat.Text = Persistence.koffieZetApparaatStatus;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.toolbar_menu, menu);
            IMenuItem item = menu.FindItem(Resource.Id.menu_Kliko);
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