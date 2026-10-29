#ifndef _LED_COLOR_FADE_CONTROLLER_H_
#define _LED_COLOR_FADE_CONTROLLER_H_

#include "LED12Ring.h"
#include "Color.h"

class LEDColorFadeController
{
  //fromColorsからtoColorsに変化させていく
  const Color* _fromColors;
  const Color* _toColors;

  //明るさもfromIntensityからtoIntensityに変化させていく
  const float _fromIntensity;
  const float _toIntensity;

  //何ms間の間で変化させていくか
  const int _fadeDuration_ms;

  bool _isFinished = false;
  unsigned long _previous_ms;

  //LED
  LED12Ring* _led12Ring;
public:
  //fromColorsからtoColorsへfadeDuration_msミリ秒かけて、だんだんと色が変化していく。色の配列の要素数はLEDの数と一致させ、かつ両方とも同じでなければならない
  LEDColorFadeController(LED12Ring &led12Ring,const Color* fromColors,const Color* toColors,float fromIntensity,float toIntensity,int fadeDuration_ms);

  void Initialize();
  void Tick();
  bool IsFinished() const {return _isFinished;}
};

#endif