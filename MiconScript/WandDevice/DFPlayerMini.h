#ifndef _DFPLAYERMINI_H_
#define _DFPLAYERMINI_H_

#include <SoftwareSerial.h>
#include <DFPlayerMini_Fast.h>

// DFPlayer mini（MP3プレイヤー）の制御クラス
// ハードウェアシリアルは使用せず、ソフトウェアシリアルで実装
// Arduino IDEのライブラリマネージャーからDFPlayerMini_Fastをインストール
// 純正よりオススメ

// VCC-5V
// GND-GND
// RX-D19(SS_TX)
// TX-D18(SS_RX)
// Busy-G7

//音声ファイル構成
//音声ファイルを格納する上位のファイルは必ず「01」というように2桁であること
//フォルダ01の中に全ての音声ファイルを格納している
//音声ファイルの名前は必ず「001」というように3桁であること

//音声ファイルの番号
//１番上のものから番号順にファイル名が決められている
enum class ESoundNum
{
  Swing=1,//通常時の振った時の音
  Sun,//太陽魔法の音
  Star,//星魔法の音
  Thunder,//雷魔法の音
  Water,//水魔法の音
  Time,//時魔法の音
};



class DFPlayerMini
{
  SoftwareSerial _mySoftwareSerial;
  DFPlayerMini_Fast _myDFPlayer;

  //ピン
  const int _mp3_Busy_PIN;

  //デフォルトの音量(0～30まで)
  const int _minVolume =0;
  const int _maxVolume=30;
  const int _defaultVolume=30;

  //フォルダの番号(フォルダ01の中に全ての音声ファイルを入れている)
  const int _dn = 1; 

public:

  DFPlayerMini(int ss_RX_PIN,int ss_TX_PIN,int mp3_Busy_PIN);
  DFPlayerMini(int ss_RX_PIN,int ss_TX_PIN,int mp3_Busy_PIN,int defaultVolume);//defaultVolumeで音量の指定が可能だが0～30まで

  //効果音を再生
  void PlaySound(ESoundNum soundNum);
  void PlaySound(ESoundNum soundNum,int volume);//音量の指定が可能(音量は0～30)

  //効果音を止める
  void Stop();

  //効果音を流しているか
  bool IsPlaying() const
  {
    int mp3_busy=digitalRead(_mp3_Busy_PIN); // HIGHで再生していない
    return (mp3_busy==LOW);
  }

  void Setup();
};

#endif