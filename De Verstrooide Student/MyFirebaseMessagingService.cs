using Android.App;
using Android.Content;
using Android.Util;
using Firebase.Messaging;
using System.Collections.Generic;

namespace De_Verstrooide_Student
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseMsgService";
        public override void OnMessageReceived(RemoteMessage message)
        {
            string title = "";
            string body = "";
            string click_action = "";

            Log.Debug(TAG, "From: " + message.From);
            //Log.Debug(TAG, "Notification Message Body: " + message.GetNotification().Body);
            
            if (message.Data.Count > 0)
            {
                Log.Debug(TAG, "Message data payload: {0}", message.Data);
                IDictionary<string, string> data = message.Data;
                foreach (KeyValuePair<string, string> kvp in data)
                {
                    Log.Debug(TAG, "Key = {0}, Value = {1}", kvp.Key, kvp.Value);
                    if(kvp.Key == "title")
                    {
                        title = kvp.Value;
                    }
                    else if (kvp.Key == "body")
                    {
                        body = kvp.Value;
                    }
                    else if(kvp.Key == "click_action")
                    {
                        click_action = kvp.Value;
                    }
                }
            }
            if (title != "" && body != "" && click_action != "")
            {
                SendNotification(title, body, click_action);
            }
        }

        void SendNotification(string title, string body, string click_Action)
        {
            Intent intent;
            if (click_Action.Equals("SecondActivity"))
            {
                intent = new Intent(this, typeof(SecondActivity));
                intent.AddFlags(ActivityFlags.ClearTop);
            }
            else if (click_Action.Equals("MainActivity"))
            {
                intent = new Intent(this, typeof(MainActivity));
                intent.AddFlags(ActivityFlags.ClearTop);
            }
            else
            {
                intent = new Intent(this, typeof(MainActivity));
                intent.AddFlags(ActivityFlags.ClearTop);
            }
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

            var notificationBuilder = new Notification.Builder(this)
                .SetSmallIcon(Resource.Drawable.ic_stat_ic_notification)
                .SetContentTitle(title)
                .SetContentText(body)
                .SetAutoCancel(true)
                .SetContentIntent(pendingIntent);

            var notificationManager = NotificationManager.FromContext(this);
            notificationManager.Notify(0, notificationBuilder.Build());
        }
    }
}