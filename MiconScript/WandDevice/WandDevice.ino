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

//ステートマシン関係
DeviceStateMachine _deviceStateMachine(_mpu,_dfPlayerMini,_led12Ring,_commandManager);


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

  //ステートマシンの更新処理
  _deviceStateMachine.Update();

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
