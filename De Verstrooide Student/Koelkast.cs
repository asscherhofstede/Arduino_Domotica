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
    /// <summary>
    /// Koelkast activity die de status aangeeft van de Koelkast
    /// </summary>
    [Activity(Label = "Koelkast")]
    public class Koelkast : AppCompatActivity
    {
        TextView koelkast;
        ImageView koelkastFoto;
        Intent intent2;

        /// <summary>
        /// Maakt de activity aan en zorgt voor de standaard foto en text, layout
        /// </summary>
        /// <param name="savedInstanceState"> onthoud de stand van de telefoon. Horizontaal of verticaal.</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
           
            // Create your application here
            SetContentView(Resource.Layout.Koelkast);
            koelkast = FindViewById<TextView>(Resource.Id.koelkast_status);
            koelkastFoto = FindViewById<ImageView>(Resource.Id.koelkastFoto);

            if (Intent.Extras != null)
            {
                foreach (var key in Intent.Extras.KeySet())
                {
                    var value = Intent.Extras.GetString(key);
                    if (key == "status")
                    {
                        Persistence.koelkastValue = value;
                    }
                }
            }

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "De Verstrooide Student";

            if (Persistence.koelkastValue == "0")
            {
                //foto van je kliko nog bij huis
                Persistence.koelkastStatus = "Je koelkast staat nog open!!";
                koelkastFoto.SetImageResource(Resource.Drawable.Koelkast_Open);
            }
            else if (Persistence.koelkastValue == "1")
            {
                //foto van kliko aan de straat
                Persistence.koelkastStatus = "Je koelkast is dicht.";
                koelkastFoto.SetImageResource(Resource.Drawable.Koelkast_Dicht);
            }
            else if (Persistence.koelkastValue == null)
            {
                Persistence.koelkastStatus = "Onze sensoren kunnen niet zien of de koelkast open of dicht is!";
                koelkastFoto.SetImageResource(Resource.Drawable.Koelkast_Dicht);
            }
            koelkast.Text = Persistence.koelkastStatus;
        }





        /// <summary>
        /// Maakt menu voor navigatie door de app. Staat in de toolbar.
        /// </summary>
        /// <param name="menu"> Opent juiste menu</param>
        /// <returns>stuurt het juiste menu terug</returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.toolbar_menu, menu);
            IMenuItem item = menu.FindItem(Resource.Id.menu_Koelkast);
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
            if (item.TitleFormatted.ToString() == "Kliko")
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