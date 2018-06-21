//Includes
#include <SoftwareSerial.h>
#include <Time.h>
#include <TimeLib.h>
#include <OneWire.h>
#include <DallasTemperature.h>
#include <NewRemoteTransmitter.h>



//Define pins
#define unitCodeApa3        29362034
#define RFPin               4

#define echoPinKliko        5
#define trigPinKliko        6

#define tempPin             7

#define buzzer              8
#define magnet              A0

#define echoPinWasmand      9
#define trigPinWasmand      10




//Variables
int countKliko, countKoelkast, countWasmand, countVentilator, minutes, sound;

OneWire oneWire(tempPin);
DallasTemperature sensors(&oneWire);
float temperatuur = 0;
bool geluid = true;
bool unit1 = false;
 
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
}

void loop() {
  //Eerst alle methods laten lopen?
  //Of per onderdeel eerst de method en dan de if statement?
  //Of in je method al de if statement --> ziet er naar mijn mening beter uit.
  Kliko();
  delay(200);
  Koelkast();
  delay(200);
  Ventilator();
  delay(200);
  Wasmand();
  delay(200);
  //KoffieZetApparaat(); 
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
  }
  else {
   countKliko = 0;
   Serial.println("Kliko staat op zijn plek :D");
  }
  
  if(countKliko == 5 && weekday() == 2 && hour() >= 9)
  {
    Serial.println("Zet de kliko aan de weg!");
    Arduino.print('a'); 
  } 
 
}

void Koelkast(){
  if(digitalRead(magnet) == HIGH)
  {
    countKoelkast++;
    Serial.println(countKoelkast);
    if ( countKoelkast == 5)
    {
      Arduino.print('b');
      Serial.println("Doe de Koelkast dicht nerd");
    } 
     if ( countKoelkast > 5)
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
  if(digitalRead(magnet) == LOW)
  {
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
  if (temperatuur >= 27 && countVentilator >= 10)         //Als de temperatuur 20graden of hoger is EN 60s is gemeten > Ventilator aan
  {
    if(!unit1)
          {
            countVentilator = 0;
            apa3Transmitter.sendUnit(0, 1);               //Unit1 gaat AAN
            Arduino.print('c');                           //Geeft waarde 'AAN' naar ESP module
            unit1 = true;
                Serial.println("Ventilator is aan!");
          }

  }
  else if (temperatuur < 27 && countVentilator >= 10)     //Als de temperatuur 20graden of hoger is EN 60s is gemeten > Ventilator aan
  {
    if(unit1) 
    {
      countVentilator = 0;
      apa3Transmitter.sendUnit(0, 0);                     //Unit1 gaat UIT
      Arduino.print('f');                                 //Geeft waarde 'UIT' naar ESP module
      unit1 = false;                                      
      Serial.println("Ventilator is uit!");
    }
  }

  countVentilator++;                      
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

  if (distanceWasmand < 20){
    if (countKliko <= 60){
    Serial.print("De wasmand is ");
    Serial.print(countWasmand);
    Serial.println(" seconde vol");
    }
    if (countWasmand >= 60){
      minutes = countWasmand / 60;
      Serial.print("De wasmand is ");
      Serial.print(minutes);
      Serial.println(" minuut/minuten vol");
    }
    countWasmand++;
  }
  else {
   countWasmand = 0;
   Serial.println("Je hoeft nog niet te wassen :D");
  }
  
  if(countWasmand == 10 && weekday() == 2 && hour() >= 9)
  {
    Serial.println("De wasmand zit vol!");
    Arduino.print('d'); 
  } 
}

void KoffieZetApparaat(){


}


