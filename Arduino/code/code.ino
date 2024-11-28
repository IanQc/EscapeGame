#define CHAN_KEY 0
#define CHAN_LIGHT 3
#define MA_BROCHE_ANGLE 32

#include <M5Atom.h>

// Initialisation des variables
CRGB pixel;

unsigned long chronoMessage;

#include <MicroOscSlip.h>
MicroOscSlip<128> monOsc(&Serial);

#include <M5_PbHub.h>
M5_PbHub myPbHub;

int maLectureKeyPrecedente;
bool maLectureToggle = false;

void setup() {
  // put your setup code here, to run once:
  M5.begin(false, false, false);
  FastLED.addLeds<WS2812, DATA_PIN, GRB>(&pixel, 1);  //ajoute pixel
  Serial.begin(115200);

  unsigned long chronoDepart=millis();

  while (millis() - chronoDepart < 5000) {
    // Rouge
    pixel = CRGB(255, 0, 0);
    FastLED.show();
    delay(2250);

    // Jaune
    pixel = CRGB(255, 40, 0);
    FastLED.show();
    delay(2000);

    // Vert
    pixel = CRGB(0, 255, 0);
    FastLED.show();
    delay(750);
  }
  
  // Éteindre
  pixel = CRGB(0, 0, 0);
  FastLED.show();
  delay(50);

  Wire.begin();

  myPbHub.begin();
}

void loop() {
  // put your main code here, to run repeatedly:
  M5.update();

  // Fait ce code à chaque 50ms
  if (millis() - chronoMessage >= 50) {
    chronoMessage = millis();
    int maLectureLight = myPbHub.analogRead(CHAN_LIGHT);
    int maLectureKey = myPbHub.digitalRead(CHAN_KEY);
    monOsc.sendInt("/lightMap", maLectureLight);

    float maLectureGesture = analogRead(MA_BROCHE_ANGLE);
    monOsc.sendFloat("/gesture", maLectureGesture);

    if (maLectureKey != maLectureKeyPrecedente) {
      if (maLectureKey == 0) {
        maLectureToggle = !maLectureToggle;
        if (maLectureToggle) {
          monOsc.sendInt("/keyUnit", 127);
        } else {
          monOsc.sendInt("/keyUnit", 0);
        }
      }
    }
    maLectureKeyPrecedente = maLectureKey;
  }
}