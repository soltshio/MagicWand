#include "LEDColorFadeController.h"
#include "Mathf.h"
#include <Arduino.h>

LEDColorFadeController::LEDColorFadeController(LED12Ring &led12Ring,const Color* fromColors,const Color* toColors,float fromIntensity,float toIntensity,int fadeDuration_ms):
_led12Ring(&led12Ring),
_fromColors(fromColors),
_toColors(toColors),
_fromIntensity(fromIntensity),
_toIntensity(toIntensity),
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
  if(_isFinished) return;

  //経過時間の算出
  unsigned long now_ms = millis();
  int elapsed_ms = now_ms - _previous_ms;

  //色を変更
  for(int i=0; i<_led12Ring->LEDNum() ;i++)
  {
    Color fromColor = _fromColors[i];
    Color toColor = _toColors[i];

    float time = (float)elapsed_ms / _fadeDuration_ms;

    //色と明るさを計算
    Color newColor = Color::ColorLerp(fromColor,toColor,time);
    float intensity = Mathf::Lerp(_fromIntensity,_toIntensity,time);

    _led12Ring->SetColor(i,newColor,intensity);
  }

  //経過時間が_fadeDuration_ms超えていたらTick処理が呼ばれないようにする
  if(elapsed_ms < _fadeDuration_ms) return;

  _isFinished = true;
}