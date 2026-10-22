//デバイス(杖本体)側のマイコン用

#include "ESPNowEz.h"
#include "MPU_AccGyro.h"
#include "DFPlayerMini.h"
#include "DeviceStateMachine.h"

//このデバイスのMACアドレス
//34:b7:da:f2:1b:f8

CESPNowEZ espnow(1); // Deviceは1以上で必ずかぶらないこと。
uint8_t controllerAddress[] = { 0x10, 0x51, 0xdb, 0x18, 0x1f, 0xbc };

//unity側への通信を送るための機能
ESPNOW_Dev2ConData deviceData;

//unity側からの通信を受け取るための機能
ESPNOW_Con2DevData controllerData;
int outputFlag;

//加速度センサー関係
#define SDA_PIN 1
#define SCL_PIN 0
MPU_AccGyro _mpu(SDA_PIN,SCL_PIN);

//DFPlayer(スピーカー)関係
#define MP3_BUSY_PIN 7//今音を流しているかが分かるようになるピン
//ピンは逆につなげる
#define SS_RX_PIN 5
#define SS_TX_PIN 4
DFPlayerMini _dfPlayerMini(SS_RX_PIN,SS_TX_PIN,MP3_BUSY_PIN);

//ステートマシン関係
DeviceStateMachine _deviceStateMachine;


void setup()
{
  //スピーカーの初期化処理
  _dfPlayerMini.Setup();

  //通信のセットアップ処理
  espnow.Initialize(OnDataReceived, nullptr);
  espnow.SetControllerMacAddr(controllerAddress);
  Serial.begin(115200);
}

void loop() 
{
  //データ受取
  // if(outputFlag)
  // {
  //   outputFlag = 0;

    
  // }


  //入力

  //データ送信
  espnow.Send(&deviceData, sizeof(deviceData));

  //スピーカーの処理
  _dfPlayerMini.PlaySound(ESoundNum::Water);
  delay(1000);

  //Serial.println("Sounding");
  // loop内に適切なdeleyを付加
  delay(5000);

  //MACアドレスを知るためのコード
  //Serial.println(espnow.GetMacAddrChar());
  //delay(1000);
}

void OnDataReceived(const esp_now_recv_info* info, const uint8_t* data, int data_len)
{
   // 受信時の処理
   memcpy(&controllerData, data, data_len);
   outputFlag = 1;
}
