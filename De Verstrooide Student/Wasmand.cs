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
    /// Wasmand activity die de status aangeeft hoe vol de wasmand is
    /// </summary>
    [Activity(Label = "Wasmand")]
    public class Wasmand : AppCompatActivity
    {
        Intent intent2;
        ImageView wasmandImg;
        TextView wasmandText;

        /// <summary>
        /// Maakt de activity aan en zorgt voor de standaard foto en text, layout
        /// </summary>
        /// <param name="savedInstanceState"> onthoud de stand van de telefoon. Horizontaal of verticaal.</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Wasmand);
            wasmandImg = FindViewById<ImageView>(Resource.Id.wasmandStatus);
            wasmandText = FindViewById<TextView>(Resource.Id.text_wasmand);

            //als je data stuurt(status van sensor) dan ontvangt hij het als intent.extras
            if (Intent.Extras != null)
            {
                foreach (var key in Intent.Extras.KeySet())
                {
                    var value = Intent.Extras.GetString(key);
                    if (key == "status")
                    {
                        Persistence.wasmandValue = value;
                    }
                }
            }

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "De Verstrooide Student";


            if (Persistence.wasmandValue == "0")
            {
                Persistence.wasmandStatus = "Wasmand is leeg";
                wasmandImg.SetImageResource(Resource.Mipmap.MANDLeeg);
            }
            else if (Persistence.wasmandValue == "1")
            {
                Persistence.wasmandStatus = "Wasmand is 50% vol";
                wasmandImg.SetImageResource(Resource.Mipmap.MANDHalf);
            }
            else if (Persistence.wasmandValue == "2")
            {
                Persistence.wasmandStatus = "Wasmand is volledig gevuld.";
                wasmandImg.SetImageResource(Resource.Mipmap.MANDVol);
            }
            else if (Persistence.wasmandValue == null)
            {
                Persistence.wasmandStatus = "Onze sensoren vangen de wasmand nog niet op!";
                wasmandImg.SetImageResource(Resource.Mipmap.Geen_Mand);
            }
            wasmandText.Text = Persistence.wasmandStatus;
        }
        /// <summary>
        /// Maakt menu voor navigatie door de app. Staat in de toolbar.
        /// </summary>
        /// <param name="menu"> Opent juiste menu</param>
        /// <returns>stuurt het juiste menu terug</returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            
            MenuInflater.Inflate(Resource.Menu.toolbar_menu, menu);
            IMenuItem item = menu.FindItem(Resource.Id.menu_Wasmand);
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