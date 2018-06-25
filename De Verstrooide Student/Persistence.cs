using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace De_Verstrooide_Student
{
    class Persistence
    {
        public static string klikoStatus { get; set; }
        public static string koelkastStatus { get; set; }
        public static string koffieZetApparaatStatus { get; set; }
        public static string ventilatorStatus { get; set; }
        public static string wasmandStatus { get; set; }

    }
}