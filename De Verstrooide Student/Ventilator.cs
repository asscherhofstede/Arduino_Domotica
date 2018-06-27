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
    /// Ventilator activity die de status aangeeft van de Ventilator
    /// </summary>
    [Activity(Label = "Ventilator")]
    public class Ventilator : AppCompatActivity
    {
        TextView ventilatorStatus, ventilatorSensor;
        Intent intent2;
        ImageView tempImage;
        /// <summary>
        /// Maakt de activity aan en zorgt voor de standaard foto en text, layout
        /// </summary>
        /// <param name="savedInstanceState"> onthoud de stand van de telefoon. Horizontaal of verticaal.</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Ventilator);
            ventilatorStatus = FindViewById<TextView>(Resource.Id.ventilator_status);
            ventilatorSensor = FindViewById<TextView>(Resource.Id.tempOutputView);
            tempImage = FindViewById<ImageView>(Resource.Id.weatherView);

            if (Intent.Extras != null)
            {
                foreach (var key in Intent.Extras.KeySet())
                {
                    var value = Intent.Extras.GetString(key);
                    if (key == "status")
                    {
                        Persistence.ventilatorValue = value;
                    }
                }
            }

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "De Verstrooide Student";

            if (Persistence.ventilatorValue == "0")
            {
                Persistence.ventilatorStatus = "Ventilator is uit";
                Persistence.ventilatorSensor = "Temperatuur is onder 20 Graden Celsius";
                tempImage.SetImageResource(Resource.Drawable.cloud);
            }
            else if (Persistence.ventilatorValue == "1")
            {
                Persistence.ventilatorStatus = "Ventilator is aan";
                Persistence.ventilatorSensor = "Temperatuur is boven de 20 Graden Celsius";
                tempImage.SetImageResource(Resource.Drawable.contrast);
            }
            else if (Persistence.ventilatorValue == null)
            {
                Persistence.ventilatorStatus = "Onze sensoren kunnen niet zien of de ventilator aan of uit staat";
                tempImage.SetImageResource(Resource.Drawable.Geen_Ventilator);
            }

            ventilatorSensor.Text = Persistence.ventilatorSensor;
            ventilatorStatus.Text = Persistence.ventilatorStatus;
        }
        /// <summary>
        /// Maakt menu voor navigatie door de app. Staat in de toolbar.
        /// </summary>
        /// <param name="menu"> Opent juiste menu</param>
        /// <returns>stuurt het juiste menu terug</returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.toolbar_menu, menu);
            IMenuItem item = menu.FindItem(Resource.Id.menu_Ventilator);
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