//Includes
#include <SoftwareSerial.h>

//Define pins
int button = 7;
int buttonState = 0;

//Instantiate serial communication between arduino and esp8266 module
SoftwareSerial Arduino(2, 3); //RX || TX


void setup() {
  Serial.begin(115200);
  Arduino.begin(115200);
}

void loop() {
  //Check if button was pressed
  buttonState = digitalRead(button);
  if(buttonState == HIGH)
  {
    Serial.println("Button was pressed");
    //Arduino.write('b');
    Arduino.print('b');   
  }  
  delay(500);
}
