#include "LED12Ring.h"
#include "Mathf.h"
#include <Arduino.h>

LED12Ring::LED12Ring(int ledPin):
_ledPin(ledPin),
_pixels(_numLEDs,_ledPin, NEO_GRB + NEO_KHZ800)
{

}

void LED12Ring::Setup()
{
  _pixels.begin();
  _pixels.show();
}

void LED12Ring::Tick()
{
  Serial.println(l);
  l=0;
}

void LED12Ring::Clear()
{
  _pixels.clear();
}

void LED12Ring::SetColor(int ledIndex,Color color,float intensity)
{
  intensity = Mathf::Clamp(intensity,_minIntensity,_maxIntensity);

  int r,g,b;

  color.GetRGB(r,g,b);

  //明るさの倍率を掛ける
  r = static_cast<int>(r * intensity);
  g = static_cast<int>(g * intensity);
  b = static_cast<int>(b * intensity);

  _pixels.setPixelColor(ledIndex, _pixels.Color(r, g, b));

  l++;
}

void LED12Ring::Show()
{
  _pixels.show();
}