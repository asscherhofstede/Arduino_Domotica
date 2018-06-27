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
    /// <summary>
    /// Kliko activity die de status aangeeft van de kliko
    /// </summary>
    [Activity(Label = "Kliko")]
    public class Kliko : AppCompatActivity
    {
        Intent intent2;
        TextView kliko;
        ImageView fotokliko;
        /// <summary>
        /// Maakt de activity aan en zorgt voor de standaard foto en text, layout
        /// </summary>
        /// <param name="savedInstanceState"> onthoud de stand van de telefoon. Horizontaal of verticaal.</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Kliko);
            kliko = FindViewById<TextView>(Resource.Id.kliko_status);
            fotokliko = FindViewById<ImageView>(Resource.Id.imageView1);
            //als je data stuurt(status van sensor) dan ontvangt hij het als intent.extras
            if (Intent.Extras != null)
            {
                foreach (var key in Intent.Extras.KeySet())
                {
                    var value = Intent.Extras.GetString(key);
                    if (key == "status")
                    {
                        Persistence.klikoValue = value;
                    }
                }
            }

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "De Verstrooide Student";


            if (Persistence.klikoValue == "0")
            {
                Persistence.klikoStatus = "De kliko staat op zijn plek!";
                fotokliko.SetImageResource(Resource.Drawable.Red_Trash);
            }
            else if (Persistence.klikoValue == "1")
            {
                Persistence.klikoStatus = "De kliko is aan de weg!";
                fotokliko.SetImageResource(Resource.Drawable.Green_Trash);
            }
            else if (Persistence.klikoValue == "2")
            {
                Persistence.klikoStatus = "Zet de kliko aan de weg! Morgen word hij opgehaald!";
            }
            else if (Persistence.klikoValue == null)
            {
                Persistence.klikoStatus = "Onze sensoren kunnen niet zien waar de kliko staat!";
                fotokliko.SetImageResource(Resource.Drawable.Geen_Trash);
            }

            kliko.Text = Persistence.klikoStatus;

        }
        /// <summary>
        /// Maakt menu voor navigatie door de app. Staat in de toolbar.
        /// </summary>
        /// <param name="menu"> Opent juiste menu</param>
        /// <returns>stuurt het juiste menu terug</returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.toolbar_menu, menu);
            IMenuItem item = menu.FindItem(Resource.Id.menu_Kliko);
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