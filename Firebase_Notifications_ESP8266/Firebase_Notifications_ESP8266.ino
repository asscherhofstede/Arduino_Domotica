#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>
#include <SoftwareSerial.h>

#define WIFI_SSID     "OnePlus 5T" //Naam van WIFI
#define WIFI_PASSWORD "kevinesp8266" //Wachtwoord van WIFI

WiFiClient client;
HTTPClient http;

String fcmServer = "AAAAEt_XRhE:APA91bEycJ4U7rC0fELTPHfBX77-XRtl4t1Wl_hAjMEVFCRqkdef_6WnpB0Sgh-B8r1FxyzJMS3E9JhI3igx2sblQoRtIu_igDxEHVzBTS_8T7ozFov5ywEpRs-7-q0piDi2h7NUtZhg";
String smartphoneId = "fQyxkSDGb1U:APA91bHAaCaUv26F1G3HQXfrYLWcwOnAlNsfjSByY77lXnLlbrqSz5DUWPLfsf5BffZNj43C2GQi3lbwZhbl6iovOiVxUW3qk_tZtlmvl750O_6npsTpgpkdCnHNpnfAZC-JLCtlHXnkuMATcjDoCcAo4N_lgPviVA";

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
      SendDataToFirebase("De Verstrooide Student", "Zet je kliko buiten", "Kliko");
      SendDataToFirebase("De Verstrooide Student", "Kliko: 0", "");
    }
    else if (input == 'b'){
      SendDataToFirebase("De Verstrooide Student", "Kliko: 1", "");
    }
    //Koelkast
    //else if(input == 'b')
    //{
    //  SendDataToFirebase("De Verstrooide Student", "Koelkast staat open!! ", "Koelkast");
    //}
    //Ventilator
    else if(input == 'c')
    {
      SendDataToFirebase("De Verstrooide Student", "Ventilator gaat aan!! ", "Ventilator");
    }
    //Wasmand
    else if(input == 'd')
    {
      SendDataToFirebase("De Verstrooide Student", "De wasmand zit vol!! ", "Wasmand");
    }
    //KoffieZetApparaat
    else if(input == 'e')
    {
      SendDataToFirebase("De Verstrooide Student", "Koffie gaat aan!! ", "KoffieZetApparaat");
    }
    else if(input == 'f')
    {
      SendDataToFirebase("De Verstrooide Student", "Koffie gaat uit!! ", "KoffieZetApparaat");
    }
  }  
  delay(250);
}

void SendDataToFirebase(String title, String msg, String activity, String stat) {
  //more info @ https://firebase.google.com/docs/cloud-messaging/http-server-ref

  String data = "{";
  data = data + "\"to\": \"" + smartphoneId + "\",";
  data = data + "\"notification\": {";
  data = data + "\"body\": \"" + msg + "\",";
  data = data + "\"title\" : \"" + title + "\",";
  data = data + "\"sound\" : \"default\" ";
  data = data + "}, ";
  data = data + "\"data\": {";
  data = data + "\"click_action\": \"" + activity + "\",";
  data = data + "\"status\" : \"" + stat + "\" "; 
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
