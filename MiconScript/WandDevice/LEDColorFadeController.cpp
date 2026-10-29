#include "LEDColorFadeController.h"
#include "Mathf.h"
#include <Arduino.h>

LEDColorFadeController::LEDColorFadeController(LED12Ring &led12Ring,int colorCount,const Color* fromColors,const Color* toColors,int fadeDuration_ms):
_led12Ring(&led12Ring),
_colorCount(colorCount),
_fromColors(fromColors),
_toColors(toColors),
_fadeDuration_ms(fadeDuration_ms)
{

}

void LEDColorFadeController::Initialize()
{
  _isFinished = false;
  _previous_ms = millis();
}

void LEDColorFadeController::Tick()
{
  //経過時間の算出
  unsigned long now_ms = millis();
  int elapsed_ms = now_ms - _previous_ms;

  //色を求める

  //色を設定
}