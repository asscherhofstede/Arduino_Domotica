using Android.App;
using Android.Content;
using Android.Media;
using Android.Net;
using Android.Util;
using Firebase.Messaging;
using System;
using System.Collections.Generic;

namespace De_Verstrooide_Student
{
    /// <summary>
    /// Activity die de noticaties binnenkrijgt en een intent stuurt naar de juiste activity
    /// </summary>
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseMsgService";
        string status = "";
        string title = "";
        string body = "";
        string click_action = "";

        Intent intent;
        /// <summary>
        /// Als firebase een message doorstuurt dan word deze method geactiveerd
        /// </summary>
        /// <param name="message">De complete message vanuit firebase</param>
        public override void OnMessageReceived(RemoteMessage message)
        {

            //bool sensorStatus = false;
            Log.Debug(TAG, "From: " + message.From);
            
            if(message.GetNotification() != null)
            {
                title = message.GetNotification().Title;
                body = message.GetNotification().Body;
            }

            //check als er wat in de data staat
            if (message.Data.Count > 0)
            {
                Log.Debug(TAG, "Message data payload: {0}", message.Data);
                IDictionary<string, string> data = message.Data;
                foreach (KeyValuePair<string, string> kvp in data)
                {
                    if(kvp.Key == "click_action")
                    {
                        click_action = kvp.Value;
                    }
                    else if (kvp.Key == "status")
                    {
                        status = kvp.Value;
                    }
                }
            }

            if (title != "" && body != "" && click_action != "")
            {
                SendNotification(title, body, click_action);
            }
            else if (status != "")
            {
                //check welke body hij mee stuurt.
                if (body.Contains("Kliko"))
                {
                    Persistence.klikoValue = status;
                }
                else if (body.Contains("Koelkast"))
                {
                    Persistence.koelkastValue = status;
                }
                else if (body.Contains("KoffieZetApparaat"))
                {
                    Persistence.koffieZetApparaatValue = status;
                }
                else if (body.Contains("Ventilator"))
                {
                    Persistence.ventilatorValue = status;
                }
                else if (body.Contains("Wasmand"))
                {
                    Persistence.wasmandValue = status;
                }
            }
        }
        /// <summary>
        /// Stuurt notificatie naar telefoon
        /// </summary>
        /// <param name="title">titel van notificatie. Standaar: de verstrooide student.</param>
        /// <param name="body">De text in melding en tevens hetgene wat de status van de app aanstuurt</param>
        /// <param name="click_Action">Naar welke activity de melding verwijst als je er op klikt</param>
        void SendNotification(string title, string body, string click_Action)
        {            
            if (click_Action.Equals("Kliko"))
            {
                intent = new Intent(this, typeof(Kliko));
                intent.AddFlags(ActivityFlags.ClearTop);
                intent.PutExtra("status", status);
            }
            else if (click_Action.Equals("Koelkast"))
            {
                intent = new Intent(this, typeof(Koelkast));
                intent.AddFlags(ActivityFlags.ClearTop);
                intent.PutExtra("status", status);
            }
            else if (click_Action.Equals("Ventilator"))
            {
                intent = new Intent(this, typeof(Ventilator));
                intent.AddFlags(ActivityFlags.ClearTop);
                intent.PutExtra("status", status);
            }
            else if (click_Action.Equals("Wasmand"))
            {
                intent = new Intent(this, typeof(Wasmand));
                intent.AddFlags(ActivityFlags.ClearTop);
                intent.PutExtra("status", status);
            }
            else if (click_Action.Equals("KoffieZetApparaat"))
            {
                intent = new Intent(this, typeof(KoffieZetApparaat));
                intent.AddFlags(ActivityFlags.ClearTop);
                intent.PutExtra("status", status);
            }
            else
            {
                intent = new Intent(this, typeof(MainActivity));
                intent.AddFlags(ActivityFlags.ClearTop);
            }
            
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

            Notification notification;
            var notificationManager = NotificationManager.FromContext(this);

            if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.O)
            {
                notification = new Notification.Builder(this)
                                            .SetContentTitle(title)
                                            .SetContentText(body)
                                            .SetAutoCancel(true)
                                            .SetSmallIcon(Resource.Drawable.Icon)
                                            .SetDefaults(NotificationDefaults.All)
                                            .SetContentIntent(pendingIntent)
                                            .Build();
            }
            
            else
            {
                // Setup a NotificationChannel, Go crazy and make it public, urgent with lights, vibrations & sound.
                var myUrgentChannel = this.PackageName;
                const string channelName = "De Verstrooide Student";

                NotificationChannel channel;
                channel = notificationManager.GetNotificationChannel(myUrgentChannel);
                if (channel == null)
                {
                    channel = new NotificationChannel(myUrgentChannel, channelName, NotificationImportance.High);
                    channel.EnableVibration(true);
                    channel.EnableLights(true);
                    channel.SetSound(
                        RingtoneManager.GetDefaultUri(RingtoneType.Notification),
                        new AudioAttributes.Builder().SetUsage(AudioUsageKind.Notification).Build()
                    );
                    channel.LockscreenVisibility = NotificationVisibility.Public;
                    notificationManager.CreateNotificationChannel(channel);
                }
                channel?.Dispose();

                notification = new Notification.Builder(this)
                                            .SetChannelId(myUrgentChannel)
                                            .SetContentTitle(title)
                                            .SetContentText(body)
                                            .SetAutoCancel(true)
                                            .SetSmallIcon(Resource.Drawable.Icon)
                                            .SetContentIntent(pendingIntent)
                                            .Build();
            }
            notificationManager.Notify(1331, notification);
            notification.Dispose();
        }
    }
}