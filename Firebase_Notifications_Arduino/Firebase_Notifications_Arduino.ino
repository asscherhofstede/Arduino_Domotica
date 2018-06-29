//Includes
#include <SoftwareSerial.h>
#include <Time.h>
#include <TimeLib.h>
#include <OneWire.h>
#include <DallasTemperature.h>
#include <NewRemoteTransmitter.h>
#include <Ethernet.h>
#include <SPI.h>


//Define pins
#define unitCodeApa3        29362034
#define RFPin               4

#define tempPin             5

#define buzzer              6
#define magnet              A0

#define echoPinKliko        7
#define trigPinKliko        8

#define echoPinWasmand      9
#define trigPinWasmand      10




//Variables
int countKliko, countKoelkast, countWasmand, countVentilator, minutes, sound;

byte mac[] = {0x40, 0x6c, 0x8f, 0x36, 0x84, 0x8a};
EthernetServer server(50007);
bool connected = false;

int ventilatorSetting = 25;
int koelkastSetting = 10;

String temp;

OneWire oneWire(tempPin);
DallasTemperature sensors(&oneWire);
float temperatuur = 0;
bool geluid = true;
bool unit1 = false;
bool wasmandVol = false;
bool koffie = false;
bool koelkast = false;
 
NewRemoteTransmitter apa3Transmitter(unitCodeApa3, RFPin, 260, 3);    //transmitter


//Instantiate serial communication between arduino and esp8266 module
SoftwareSerial Arduino(2, 3); //RX || TX


void setup() {
  Serial.begin(115200);
  Arduino.begin(115200);
  sensors.begin();

  //pinmodes
  pinMode(trigPinKliko, OUTPUT);
  pinMode(echoPinKliko, INPUT);

  pinMode(trigPinWasmand, OUTPUT);
  pinMode(echoPinWasmand, INPUT);
  
  pinMode(buzzer, OUTPUT);
  pinMode(magnet, INPUT);


  //Functions
  setTime(9,0,0,1,1,18);
  digitalWrite(magnet, HIGH);

  if(Ethernet.begin(mac) == 0){
    return;
  }

  Serial.print("Listening on address: ");
  Serial.println(Ethernet.localIP());
  server.begin();
  connected = true;
}

void loop() {
  Settings();
  Kliko();
  delay(200);
  Koelkast();
  delay(200);
  Ventilator();
  delay(200);
  Wasmand();
  delay(200);
  KoffieZetApparaat(); 
  delay(200);


}

void Kliko(){  
  long durationKliko, distanceKliko;
  digitalWrite(trigPinKliko, LOW); 
  delayMicroseconds(2);
  digitalWrite(trigPinKliko, HIGH);
  delayMicroseconds(10);
  digitalWrite(trigPinKliko, LOW);
  durationKliko = pulseIn(echoPinKliko, HIGH);
  distanceKliko = (durationKliko/2) / 29.1;

  if (distanceKliko > 30 || distanceKliko <= 0){
    if (countKliko <= 60){
    Serial.print("Kliko is ");
    Serial.print(countKliko);
    Serial.println(" seconde weg");
    }
    if (countKliko >= 60){
      minutes = countKliko / 60;
      Serial.print("Kliko is al ");
      Serial.print(minutes);
      Serial.println(" minuut/minuten weg");
    }
    countKliko++;
    Arduino.print('l'); 
  }
  else {
   countKliko = 0;
   Serial.println("Kliko staat op zijn plek :D");
   Arduino.print('k');
  }
  
  if(countKliko == 5 && weekday() == 2 && hour() >= 9)
  {
    Serial.println("Zet de kliko aan de weg!");
    Arduino.print('j'); 
  } 
 
}

void Koelkast(){
  if(digitalRead(magnet) == HIGH)
  {
    countKoelkast++;
    if (countKoelkast == koelkastSetting && koelkast == false)
    {
      Arduino.print('h');
      koelkast = true;
    } 
     if ( countKoelkast > koelkastSetting)
    {
      if (geluid == true){
        sound = 1500;
        tone(buzzer, sound);
        geluid = false;
      }
      else {
        sound = 2500;
        tone(buzzer, sound);
        geluid = true;
      }
    }
  }
  else if(digitalRead(magnet) == LOW && koelkast == true)
  {

    Arduino.print('i');
    koelkast = false;
    countKoelkast = 0;
    noTone(buzzer);
  }
}

void Ventilator(){
  sensors.requestTemperatures();
  temperatuur = sensors.getTempCByIndex(0);
  Serial.print("Temp = ");
  Serial.print(temperatuur);
  Serial.print("°");  
  Serial.println("C");
  if (temperatuur >= ventilatorSetting && countVentilator >= 10)         //Als de temperatuur 20graden of hoger is EN 60s is gemeten > Ventilator aan
  {
    if(!unit1)
    {
      countVentilator = 0;
      apa3Transmitter.sendUnit(0, 1);                     //Unit1 gaat AAN
      //delay(100);
      Arduino.print('d');                                 //Geeft waarde 'AAN' naar ESP module
      unit1 = true;
      Serial.println("Ventilator is aan!");
    }
  }
  else if (temperatuur < ventilatorSetting && countVentilator >= 10)     //Als de temperatuur 20graden of hoger is EN 60s is gemeten > Ventilator aan
  {
    if(unit1) 
    {
      countVentilator = 0;
      apa3Transmitter.sendUnit(0, 0);                     //Unit1 gaat UIT
      //delay(100);
      Arduino.print('e');                                 //Geeft waarde 'UIT' naar ESP module
      unit1 = false;                                      
      Serial.println("Ventilator is uit!");
    }
  }
  countVentilator++;                      
  Serial.println(countVentilator);
}

void Wasmand(){
  long durationWasmand, distanceWasmand;
  digitalWrite(trigPinWasmand, LOW); 
  delayMicroseconds(2);
  digitalWrite(trigPinWasmand, HIGH);
  delayMicroseconds(10);
  digitalWrite(trigPinWasmand, LOW);
  durationWasmand = pulseIn(echoPinWasmand, HIGH);
  distanceWasmand = (durationWasmand/2) / 29.1;
  countWasmand++;
  


  if((distanceWasmand < 100 && distanceWasmand > 75) && countWasmand > 10){
    Serial.println("De wasmand is leeg!");
    Arduino.print('a');
    wasmandVol = false;  
      
    if(countWasmand > 10){
      countWasmand = 0;

    }
  }
  else if((distanceWasmand < 75 && distanceWasmand > 25) && countWasmand > 10){
    Serial.println("Je wasmand is voor de helft vol!");
    Arduino.print('b');
    wasmandVol = false;
    
     if(countWasmand > 10){
      countWasmand = 0;
    }
  }
  else if((distanceWasmand < 25) && (countWasmand > 10) && (wasmandVol == false)){
    Serial.print("Je wasmand is vol! Wassen maar!");
    Arduino.print('c');
    wasmandVol = true;
    
    if(countWasmand > 10){
      countWasmand = 0;
    }
  }
}


void KoffieZetApparaat(){
 if(hour() >= 8 && koffie == false)
  {
     apa3Transmitter.sendUnit(1, 1); 
     Arduino.print('f');
     koffie = true;
  } 
  else if (hour() > 10 && koffie == true)
  {
     apa3Transmitter.sendUnit(1, 0); 
     Arduino.print('g');
     koffie = false;
  }

}

void Settings(){
  if(!connected) {
    return;
  }
  
  EthernetClient ethernetClient = server.available();

  if(!ethernetClient){
    return;
  }
  while(ethernetClient.connected())
  {
    char buffer[128];
    int count = 0;
    Serial.println(count);
    while(ethernetClient.available())
    {
      buffer[count++] = ethernetClient.read();
    }
    buffer[count] = '\0';

    if(count > 0)
    {
      Serial.println(buffer);

      if(strstr(buffer, "Ventilator") != NULL){
        if(buffer[11] != NULL){
          String temp = String(buffer[10]);
          temp = temp + String(buffer[11]);          
          ventilatorSetting = temp.toInt();
          Serial.println(ventilatorSetting);          
        }
        else{
          ventilatorSetting = buffer[10] - '0';
          Serial.println(ventilatorSetting);    
        }
      }
      else if(strstr(buffer, "Koelkast") != NULL)
      {
        if(buffer[9] != NULL)
        {
          String temp = String(buffer[8]);
          temp = temp + String(buffer[9]);          
          koelkastSetting = temp.toInt();
          Serial.println(koelkastSetting);          
        }
        else
        {
          koelkastSetting = buffer[8] - '0';
          Serial.println(koelkastSetting);    
        }
      }
    }
  }
}

