#define CHAN_KEY 0
#define CHAN_ANGLE 1
#define CHAN_LIGHT 5

#include <M5Atom.h>

CRGB pixel;

unsigned long monChronoMessages;

#include <MicroOscSlip.h>
MicroOscSlip<128> monOsc(&Serial);

#include <M5_PbHub.h>
M5_PbHub myPbHub;

#include <VL53L0X.h>
VL53L0X myTOF;

#include "Unit_Encoder.h"
Unit_Encoder myEncoder;

int myEncoderPreviousRotation;

int maLectureKeyPrecedente;
int etatPlay;

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
  myPbHub.setPixelCount(CHAN_KEY, 1);

  myTOF.init();
  myTOF.setTimeout(500);
  myTOF.startContinuous();

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

bool isPlaying = false;

void loop() {
  // put your main code here, to run repeatedly:
  M5.update();

  //monOsc.onOscMessageReceived(maReceptionMessageOsc);

  // À CHAQUE 20 MS I.E. 50x PAR SECONDE
  if (millis() - monChronoMessages >= 20) {
    monChronoMessages = millis();

    int maLectureKey = myPbHub.digitalRead(CHAN_KEY);

    if (maLectureKeyPrecedente != maLectureKey) {
      if (maLectureKey == 0 && isPlaying == false) {
        // /vkb_midi/@/note/# i
        monOsc.sendInt("/vkb_midi/9/note/64", 127);
        isPlaying = !isPlaying;
      } else if (maLectureKey == 0 && isPlaying == true) {
        monOsc.sendInt("/vkb_midi/9/note/64", 0);
        isPlaying = !isPlaying;
      }
    }
    maLectureKeyPrecedente = maLectureKey;
    
    uint16_t value = myTOF.readRangeContinuousMillimeters();
    int tofPosition = map(value, 0, 1200, 0, 127);

    int error = myTOF.timeoutOccurred();
    if (myTOF.timeoutOccurred()) {
      monOsc.sendInt("/TOFTIMEOUT", 1);
    } else {
      monOsc.sendInt("/tof", value);
      if (value <= 1200) {
        monOsc.sendInt("/vkb_midi/9/cc/13", tofPosition);
      } 
      monOsc.sendInt("/TOFERROR", error);
    }

    int maLectureLight = myPbHub.analogRead(CHAN_LIGHT);
    int compressedLight = map(maLectureLight, 1000, 4100, 0, 64);
    monOsc.sendInt("/vkb_midi/9/cc/12", compressedLight);

  }
}
