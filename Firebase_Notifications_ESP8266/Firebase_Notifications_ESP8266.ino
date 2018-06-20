#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>
#include <SoftwareSerial.h>

#define WIFI_SSID     "OnePlus 5T" //Naam van WIFI
#define WIFI_PASSWORD "kevinesp8266" //Wachtwoord van WIFI

WiFiClient client;
HTTPClient http;

String fcmServer = "AAAAEt_XRhE:APA91bEycJ4U7rC0fELTPHfBX77-XRtl4t1Wl_hAjMEVFCRqkdef_6WnpB0Sgh-B8r1FxyzJMS3E9JhI3igx2sblQoRtIu_igDxEHVzBTS_8T7ozFov5ywEpRs-7-q0piDi2h7NUtZhg";
String smartphoneId = "c5DaGBBU9BQ:APA91bHzhTYk7BIdx0D8bKcu550PnHoFIQqX8OR_VPzX4nsWqRa7MOl017ZgIyfnaFtfqQK_Oge0I6-yNvqC57sxXlAi9T7tJ4Z1faxgwHmx6uXoIrijFEZ5T5EWBK9jFLWTBXecftBFnflF-RL9mnI_1mIbdlJ9-A";

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
}

void loop() {
  // put your main code here, to run repeatedly:
  while (Serial.available() > 0)
  {
    char input = Serial.read();
    //Kliko
    if(input == 'a')
    {
      SendDataToFirebase("De Verstrooide Student", "Je hebt op de knop gedrukt!", "Kliko");//Goed opletten met de hoofdletters voor de activity
    }
    //Koelkast
    else if(input == 'b')
    {
      SendDataToFirebase("De Verstrooide Student", "Zet de kliko aan de weg!! ", "Koelkast");//Goed opletten met de hoofdletters voor de activity
    }
    //Ventilator
    else if(input == 'c')
    {
      SendDataToFirebase("De Verstrooide Student", "Zet de kliko aan de weg!! ", "Ventilator");//Goed opletten met de hoofdletters voor de activity
    }
    //Wasmand
    else if(input == 'd')
    {
      SendDataToFirebase("De Verstrooide Student", "Zet de kliko aan de weg!! ", "Wasmand");//Goed opletten met de hoofdletters voor de activity
    }
    //KoffieZetApparaat
    else if(input == 'e')
    {
      SendDataToFirebase("De Verstrooide Student", "Zet de kliko aan de weg!! ", "KoffieZetApparaat");//Goed opletten met de hoofdletters voor de activity
    }
  }  
  delay(250);
}

void SendDataToFirebase(String title, String msg, String activity) {
  //more info @ https://firebase.google.com/docs/cloud-messaging/http-server-ref

  String data = "{";
  data = data + "\"to\": \"" + smartphoneId + "\",";
  data = data + "\"notification\": {";
  data = data + "\"body\": \"" + msg + "\",";
  data = data + "\"title\" : \"" + title + "\" ";
  data = data + "}, ";
  data = data + "\"data\": {";
  data = data + "\"click_action\": \"" + activity + "\" ";
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
