#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>
#include <SoftwareSerial.h>

#define WIFI_SSID     "" //Naam van WIFI
#define WIFI_PASSWORD "" //Wachtwoord van WIFI

WiFiClient client;
HTTPClient http;

String fcmServer = "AAAAEt_XRhE:APA91bEycJ4U7rC0fELTPHfBX77-XRtl4t1Wl_hAjMEVFCRqkdef_6WnpB0Sgh-B8r1FxyzJMS3E9JhI3igx2sblQoRtIu_igDxEHVzBTS_8T7ozFov5ywEpRs-7-q0piDi2h7NUtZhg";
String smartphoneId = "eOljqdoAmos:APA91bFUrGS85J86aOEGNyoEbNjEhHGy_1doHILwVqC6fS3o4Q8max56nAK7zzQToOh8jcsH05PaodC-9tgutiLx1TOnvqDw_KLQe456i5h6lmbe064Smsa0ungmT5DFgD4jgGw4K5UQ";

void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
  Serial.setDebugOutput(true);
  
  // connect to wifi.
  WiFi.begin(WIFI_SSID, WIFI_PASSWORD);
  Serial.print("connecting");
  while (WiFi.status() != WL_CONNECTED) {
    Serial.print(".");
    delay(500);
  }
  Serial.println();
  Serial.print("connected: ");
  Serial.println(WiFi.localIP());
  //SendDataToFirebase("De Verstrooide Student", "Test");
}

void loop() {
  // put your main code here, to run repeatedly:
  while (Serial.available() > 0)
  {
    //Serial.print(char(Serial.read()));
    if(Serial.read() == 'b')
    {
      SendDataToFirebase("De Verstrooide Student", "Je hebt op de knop gedrukt!", "mainactivity");//Goed opletten met de hoofdletters voor de activity
    }
  }  
  delay(250);
}

void SendDataToFirebase(String title, String msg, String activity) {
  //more info @ https://firebase.google.com/docs/cloud-messaging/http-server-ref

  String data = "{";
  data = data + "\"to\": \"" + smartphoneId + "\",";
  data = data + "\"data\": {";
  data = data + "\"body\": \"" + msg + "\",";
  data = data + "\"title\" : \"" + title + "\",";
  data = data + "\"click_action\" : \"" + activity + "\" ";
  data = data + "} }";

  http.begin("http://fcm.googleapis.com/fcm/send");
  http.addHeader("Authorization", "key=" + fcmServer);
  http.addHeader("Content-Type", "application/json");
  http.addHeader("Host", "fcm.googleapis.com");
  http.addHeader("Content-Length", String(msg.length()));
  http.POST(data);
  http.writeToStream(&Serial);
  http.end();
  Serial.println();
}
