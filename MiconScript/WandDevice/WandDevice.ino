//デバイス(杖本体)側のマイコン用

#include "ESPNowEz.h"
#include "MPU_AccGyro.h"
#include "DFPlayerMini.h"
#include "DeviceStateMachine.h"
#include "LED12Ring.h"
#include "Color.h"
#include "CommandManager.h"

//このデバイスのMACアドレス
//34:b7:da:f2:1b:f8

CESPNowEZ espnow(1); // Deviceは1以上で必ずかぶらないこと。
uint8_t controllerAddress[] = { 0x10, 0x51, 0xdb, 0x18, 0x1f, 0xbc };

//unity側への通信を送るための機能
ESPNOW_Dev2ConData deviceData;

//unity側からの通信を受け取るための機能
ESPNOW_Con2DevData controllerData;
int outputFlag;
CommandManager _commandManager;//受け取ったコマンドを管理する機能

//加速度センサー関係
#define SDA_PIN 1
#define SCL_PIN 0
MPU_AccGyro _mpu(SDA_PIN,SCL_PIN);

//DFPlayer(スピーカー)関係
#define MP3_BUSY_PIN 7//今音を流しているかが分かるようになるピン
//以下の二つのピンは逆につなげる
#define SS_RX_PIN 5
#define SS_TX_PIN 4
const int _volume = 15;
DFPlayerMini _dfPlayerMini(SS_RX_PIN,SS_TX_PIN,MP3_BUSY_PIN,_volume);

//LED関係
#define LED_PIN 6
LED12Ring _led12Ring(LED_PIN);

//LEDのステートごとのプロパティ関係

//TODO:AIで色の指定をこの表記に変換させる
//---通常状態---
const StateLEDProperty _normalLEDProperty =
{
  .ledColors =
  {
    Color(255,   0,   0), // 赤
    Color(255,  80,   8), // 赤とオレンジの間
    Color(255, 160,  16), // オレンジ
    Color(255, 224,  32), // 黄
    Color(130, 255,   0), // 黄緑
    Color(  0, 192,   0), // 緑
    Color( 40, 200, 127), // 緑と水色の間
    Color( 80, 208, 255), // 水色
    Color(  0,  32, 255), // 青
    Color( 80,  32, 255), // 青紫
    Color(160,  32, 255), // 紫
    Color(255,  96, 208)  // ピンク
  },
  ._lowLEDIntensity = 0.04f,
  ._highLEDIntensity = 0.2f
};

//---太陽魔法---
const StateLEDProperty _sunLEDProperty =
{
  .ledColors =
  {
    Color(255,   0,   0), // 赤
    Color(255,  53,   5),
    Color(255, 106,  10),
    Color(255, 160,  16), // オレンジ
    Color(255, 181,  21),
    Color(255, 202,  26),
    Color(255, 224,  32), // 黄
    Color(255, 202,  26),
    Color(255, 181,  21),
    Color(255, 160,  16), // オレンジ
    Color(255, 106,  10),
    Color(255,  53,   5)  // 赤の一歩手前の色
  },
  ._lowLEDIntensity = 0.01f,
  ._highLEDIntensity = 0.2f
};

//---星魔法---
const StateLEDProperty _starLEDProperty =
{
  .ledColors =
  {
    Color(160,  32, 255), // 紫
    Color(134,  32, 255),
    Color(107,  32, 255),
    Color( 80,  32, 255), // 青紫
    Color( 54,  32, 255),
    Color( 27,  32, 255),
    Color(  0,  32, 255), // 青
    Color( 27,  32, 255),
    Color( 54,  32, 255),
    Color( 80,  32, 255), // 青紫
    Color(107,  32, 255),
    Color(134,  32, 255)  // 紫の一歩手前の色
  },
  ._lowLEDIntensity = 0.01f,
  ._highLEDIntensity = 0.2f
};

//---雷魔法---
const StateLEDProperty _thunderLEDProperty =
{
  .ledColors =
  {
    Color(255, 224,  32), // 黄
    Color(170, 214,  22),
    Color( 85, 203,  11),
    Color(  0, 192,   0), // 緑
    Color( 27, 197,  85),
    Color( 54, 202, 170),
    Color( 80, 208, 255), // 水色
    Color( 54, 202, 170),
    Color( 27, 197,  85),
    Color(  0, 192,   0), // 緑
    Color( 85, 203,  11),
    Color(170, 214,  22)  // 黄の一歩手前の色
  },
  ._lowLEDIntensity = 0.01f,
  ._highLEDIntensity = 0.2f
};

//---水魔法---
const StateLEDProperty _waterLEDProperty =
{
  .ledColors =
  {
    Color( 80, 208, 255), // 水色
    Color( 54, 150, 255),
    Color( 27,  91, 255),
    Color(  0,  32, 255), // 青
    Color( 27,  32, 255),
    Color( 54,  32, 255),
    Color( 80,  32, 255), // 青紫
    Color( 54,  32, 255),
    Color( 27,  32, 255),
    Color(  0,  32, 255), // 青
    Color( 27,  91, 255),
    Color( 54, 150, 255)  // 水色の一歩手前の色
  },
  ._lowLEDIntensity = 0.01f,
  ._highLEDIntensity = 0.2f
};

//---時魔法---
const StateLEDProperty _timeLEDProperty =
{
  .ledColors =
  {
    Color(130, 255,   0), // 黄緑
    Color( 86, 234,   0),
    Color( 43, 213,   0),
    Color(  0, 192,   0), // 緑
    Color( 13, 195,  42),
    Color( 26, 198,  84),
    Color( 40, 200, 127), // 緑と水色の間
    Color( 26, 198,  84),
    Color( 13, 195,  42),
    Color(  0, 192,   0), // 緑
    Color( 43, 213,   0),
    Color( 86, 234,   0)  // 黄緑の一歩手前の色
  },
  ._lowLEDIntensity = 0.01f,
  ._highLEDIntensity = 0.2f
};


//ステートマシン関係
DeviceStateMachine _deviceStateMachine(
  _mpu,
  _dfPlayerMini,
  _led12Ring,
  _commandManager,
  _normalLEDProperty,
  _sunLEDProperty,
  _starLEDProperty,
  _thunderLEDProperty,
  _waterLEDProperty,
  _timeLEDProperty
);





void setup()
{
  //スピーカーの初期化処理
  _dfPlayerMini.Setup();
  //加速度センサーの初期化処理
  _mpu.Setup();
  //LEDの初期化処理
  _led12Ring.Setup();

  //ステートマシンの初期化処理
  _deviceStateMachine.Initialize();

  //通信のセットアップ処理
  espnow.Initialize(OnDataReceived, nullptr);
  espnow.SetControllerMacAddr(controllerAddress);
  Serial.begin(115200);
}

void loop() 
{
  //ステートマシンの更新処理
  _deviceStateMachine.Update();

  _led12Ring.Tick();

  //データ受取
  if(outputFlag)
  {
    _commandManager.SetCommand(controllerData.cmd);
    outputFlag = 0;
  }

  //データ送信
  espnow.Send(&deviceData, sizeof(deviceData));

  //加速度センサーのTick処理
  _mpu.Tick();

  //DFPlayerのTick処理
  _dfPlayerMini.Tick();

  //ShowMACAddress();//MACアドレスをシリアルモニタに表示

  // loop内に適切なdeleyを付加
  delay(30);
}

void OnDataReceived(const esp_now_recv_info* info, const uint8_t* data, int data_len)
{
   // 受信時の処理
   memcpy(&controllerData, data, data_len);
   outputFlag = 1;
}

void ShowMACAddress()//MACアドレスをシリアルモニタに表示
{
  Serial.println(espnow.GetMacAddrChar());
}
