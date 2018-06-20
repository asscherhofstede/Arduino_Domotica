//Includes
#include <SoftwareSerial.h>
#include <Time.h>
#include <TimeLib.h>

//Define pins
#define trigPin 10
#define echoPin 9

#define ledPin 11 //moet nog even kijken welke pins dit zijn
#define magnet 12 //moet nog even kijken welke pins dit zijn

//Variables
int countKliko = 0;
int minutes = 0;

//Instantiate serial communication between arduino and esp8266 module
SoftwareSerial Arduino(2, 3); //RX || TX


void setup() {
  Serial.begin(115200);
  Arduino.begin(115200);

  //pinmodes
  pinMode(trigPin, OUTPUT);
  pinMode(echoPin, INPUT);
  
  pinMode(ledPin, OUTPUT);
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
  //Koelkast();
  //Ventilator();
  //Wasmand();
  //KoffieZetApparaat();
  if(countKliko == 10 && weekday() == 2 && hour() >= 9)
  {
    Serial.println("Zet de kliko aan de weg!");
    Arduino.print('a'); 
    delay(500);  
  } 
}

void Kliko(){  
  long duration, distance;
  digitalWrite(trigPin, LOW); 
  delayMicroseconds(2);
  digitalWrite(trigPin, HIGH);
  delayMicroseconds(10);
  digitalWrite(trigPin, LOW);
  duration = pulseIn(echoPin, HIGH);
  distance = (duration/2) / 29.1;

  if (distance > 30 || distance <= 0){
    if (count <= 60){
    Serial.print("Kliko is al ");
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
  }
  delay(900);  
}

void Koelkast(){
  if(digitalRead(magnet) == HIGH)
  {
    count++;
    if ( count >= 10)
    {
    digitalWrite(ledPin, HIGH);
    } 
  }
  else
  {
    digitalWrite(ledPin, LOW);
    count = 0;
  }
  delay(1000);
}

void Ventilator(){
  
}

void Wasmand(){
  
}

void KoffieZetApparaat(){
  
}

