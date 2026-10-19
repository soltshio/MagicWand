//杖本体とPCの中間役(PSにつける方)

#include "ESPNowEz.h"
#include <ctype.h>

//このコントローラのMACアドレス
//10:51:db:18:1f:bc

CESPNowEZ espnow(0);

uint8_t deviceMacAddr[] = { 0x34, 0xb7, 0xda, 0xf2, 0x1b, 0xf8 }; // ID1

ESPNOW_Con2DevData controllerData;

ESPNOW_Dev2ConData deviceData;

int outputFlag;

char inputChar;
const int _rateDigit=3;

char outputText[16];

void setup()
{
  outputFlag = 0;
  espnow.Initialize(OnDataReceived, nullptr);
  espnow.SetDeviceMacAddr(deviceMacAddr);
  Serial.begin(115200);
}

void loop()
{
  //デバイスからデータを受け取ったら、それをPC(Unity)側に送る
  if(outputFlag)
  {
    outputFlag = 0;
    Serial.println(deviceData.sw);
  }

  //PC(Unity)からデータを受け取ったら、それをデバイスに送る
  // if(Serial.available() > 0)
  // {
  //   inputChar = Serial.read();
    
  //   espnow.Send(1, &controllerData, sizeof(controllerData)); // id:1に送る
  // }
  controllerData.cmd = 'l';
  espnow.Send(1, &controllerData, sizeof(controllerData)); // id:1に送る

  //MACアドレスを知るためのコード
  //Serial.println(espnow.GetMacAddrChar());

  delay(1000);
}

void OnDataReceived(const esp_now_recv_info* info, const uint8_t* data, int data_len)
{
  // 受信時の処理
  memcpy(&deviceData, data, data_len);
  outputFlag = 1;
}

