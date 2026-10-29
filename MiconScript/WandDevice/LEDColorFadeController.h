#ifndef _LED_COLOR_FADE_CONTROLLER_H_
#define _LED_COLOR_FADE_CONTROLLER_H_

#include "LED12Ring.h"
#include "Color.h"

class LEDColorFadeController
{
  //fromColorsからtoColorsに変化していく
  const Color* _fromColors;
  const Color* _toColors;
  //色の配列の数
  const int _colorCount;

  //何ms間の間で変化させていくか
  const int _fadeDuration_ms;

  bool _isFinished = false;
  unsigned long _previous_ms;

  //LED
  LED12Ring* _led12Ring;
public:
  //fromColorsからtoColorsへfadeDuration_msミリ秒かけて、だんだんと色が変化していく。colorCountは色の配列の要素数で両方とも同じでなければならない
  LEDColorFadeController(LED12Ring &led12Ring,int colorCount,const Color* fromColors,const Color* toColors,int fadeDuration_ms);

  void Initialize();
  void Tick();
  bool IsFinished() const {return _isFinished;}
};

#endif