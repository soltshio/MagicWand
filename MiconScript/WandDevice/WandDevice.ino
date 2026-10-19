//デバイス(杖本体)側のマイコン用

#include "ESPNowEz.h"

//このデバイスのMACアドレス
//34:b7:da:f2:1b:f8

CESPNowEZ espnow(1); // Deviceは1以上で必ずかぶらないこと。
uint8_t controllerAddress[] = { 0x10, 0x51, 0xdb, 0x18, 0x1f, 0xbc };

//unity側への通信を送るための機能
ESPNOW_Dev2ConData deviceData;

//unity側からの通信を受け取るための機能
ESPNOW_Con2DevData controllerData;
int outputFlag;

//ボタン
#define BUTTON_PIN 9

//LED
#include <Adafruit_NeoPixel.h>

#define LED_PIN 2
#define NUM_LEDS 1

Adafruit_NeoPixel pixels(
    NUM_LEDS,
    LED_PIN,
    NEO_GRB + NEO_KHZ800
);

void setup()
{
  //LED
  pixels.begin();
  pixels.clear();
  pixels.show();

  //ボタン
  pinMode(BUTTON_PIN, INPUT_PULLUP);

  espnow.Initialize(OnDataReceived, nullptr);
  espnow.SetControllerMacAddr(controllerAddress);
  Serial.begin(115200);
}

void loop() 
{
  //データ受取
  if(outputFlag)
  {
    outputFlag = 0;

    //出力(LED)
    if(controllerData.cmd == 'l')
    {
      pixels.setPixelColor(0, pixels.Color(255, 0, 0));
      pixels.show();
    }
  }


  //入力
  if (digitalRead(BUTTON_PIN) == LOW) 
  {
    deviceData.sw=1;
  }
  else
  {
    deviceData.sw=0;
  }

  //データ送信
  espnow.Send(&deviceData, sizeof(deviceData));

  // loop内に適切なdeleyを付加
  delay(10);

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
