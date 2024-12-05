#define CHAN_WHITE 0
#define CHAN_BLUE 1
#define CHAN_RED 2
#define CHAN_GREEN 3
#define CHAN_LIGHT 5

#include <M5Atom.h>

#include "Unit_Encoder.h"
Unit_Encoder myEncoder;
int myEncoderPreviousRotation;

CRGB pixel;

unsigned long monChronoMessages;

#include <MicroOscSlip.h>
MicroOscSlip<128> monOsc(&Serial);

#include <M5_PbHub.h>
M5_PbHub myPbHub;

void setup() {
  // put your setup code here, to run once:
  M5.begin(false, false, false);
  FastLED.addLeds<WS2812, DATA_PIN, GRB>(&pixel, 1);  // Ajouter le pixel du M5Atom à FastLED
  Serial.begin(115200);

  unsigned long chronoDepart = millis();
  while (millis() - chronoDepart < 5000) {
    pixel = CRGB(255, 255, 255);
    FastLED.show();
    delay(100);

    pixel = CRGB(0, 0, 0);
    FastLED.show();
    delay(100);
  }

  pixel = CRGB(0, 0, 0);
  FastLED.show();

  Wire.begin();
  myPbHub.begin();
  myPbHub.setPixelCount(CHAN_WHITE, 1);
  myPbHub.setPixelCount(CHAN_BLUE, 1);
  myPbHub.setPixelCount(CHAN_RED, 1);
  myPbHub.setPixelCount(CHAN_GREEN, 1);

  myEncoder.begin();
}
/*
void maReceptionMessageOsc(MicroOscMessage& oscMessage) {

  if ( oscMessage.checkOscAddress("/master/vu")) {
    float vu = oscMessage.nextAsFloat();
    int niveau = floor(vu*255.0);
    pixel = CRGB(niveau,niveau,niveau);
    FastLED.show();
  }
}*/


void loop() {
  // put your main code here, to run repeatedly:
  M5.update();

  //monOsc.onOscMessageReceived(maReceptionMessageOsc);

  // À CHAQUE 20 MS I.E. 50x PAR SECONDE
  if (millis() - monChronoMessages >= 20) {
    monChronoMessages = millis();

    int confirmKey = myPbHub.digitalRead(CHAN_WHITE);
    int redKey = myPbHub.digitalRead(CHAN_RED);
    int greenKey = myPbHub.digitalRead(CHAN_GREEN);
    int blueKey = myPbHub.digitalRead(CHAN_BLUE);

      if (confirmKey == 0) {
        monOsc.sendInt("/Key", 1);
      } else {
        monOsc.sendInt("/Key", 0);
      }
    
    myPbHub.setPixelColor(CHAN_WHITE, 0, 255, 255, 255);
    myPbHub.setPixelColor(CHAN_RED, 0, 255, 0, 0);
    myPbHub.setPixelColor(CHAN_GREEN, 0, 0, 255, 0);
    myPbHub.setPixelColor(CHAN_BLUE, 0, 0, 0, 255);

    int maLectureLight = myPbHub.analogRead(CHAN_LIGHT);
    int compressedLight = map(maLectureLight, 0, 4100, 0, 100);
    monOsc.sendInt("/Light", compressedLight);

    int encoderRotation = myEncoder.getEncoderValue();
    int encoderButton = myEncoder.getButtonStatus();
    monOsc.sendInt("/rotation", encoderRotation);
    monOsc.sendInt("/button", encoderButton);
    

  }
}
