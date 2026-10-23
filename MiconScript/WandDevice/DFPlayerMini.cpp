#include "DFPlayerMini.h"
#include "Mathf.h"

DFPlayerMini::DFPlayerMini(int ss_RX_PIN,int ss_TX_PIN,int mp3_Busy_PIN):
_mySoftwareSerial(ss_RX_PIN,ss_TX_PIN),
_mp3_Busy_PIN(mp3_Busy_PIN)
{
 
}

DFPlayerMini::DFPlayerMini(int ss_RX_PIN,int ss_TX_PIN,int mp3_Busy_PIN,int defaultVolume):
_mySoftwareSerial(ss_RX_PIN,ss_TX_PIN),
_mp3_Busy_PIN(mp3_Busy_PIN),
_defaultVolume(Mathf::Clamp(defaultVolume,_minVolume,_maxVolume))
{
  
}

//メモ：音を流す前に音量を動かすと音が流れない場合があるらしい
void DFPlayerMini::PlaySound(ESoundNum soundNum)
{
  //_myDFPlayer.volume(_defaultVolume);

  int fn = static_cast<int>(soundNum);//音声ファイルの番号
  _myDFPlayer.playFolder(_dn, fn); // 曲を再生
}

void DFPlayerMini::PlaySound(ESoundNum soundNum,int volume)
{
  volume =Mathf::Clamp(volume,_minVolume,_maxVolume);
  _myDFPlayer.volume(volume);

  int fn = static_cast<int>(soundNum);//音声ファイルの番号
  _myDFPlayer.playFolder(_dn, fn); // 曲を再生
}

void DFPlayerMini::Stop()
{
  _myDFPlayer.stop();
}

void DFPlayerMini::Setup()
{
  _mySoftwareSerial.begin(9600);

  delay(1000); // これがないと動かない可能性大
  if (!_myDFPlayer.begin(_mySoftwareSerial, false)) {
    Serial.println("Unable to begin:");
    while(true)
      ; // モジュールが動作しなければ先に進めない
  }
  Serial.println("DFPlayer Mini online.");
  _myDFPlayer.volume(_defaultVolume);
}

