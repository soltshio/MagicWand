//杖本体とPCの中間役(PSにつける方)

#include "ESPNowEz.h"
#include <ctype.h>

//このコントローラのMACアドレス
//10:51:db:18:1f:bc

CESPNowEZ espnow(0);

uint8_t deviceMacAddr[] = { 0x34, 0xb7, 0xda, 0xf2, 0x1b, 0xf8 }; // ID1

//Unityからのデータを受け取るための機能
ESPNOW_Con2DevData controllerData;
const int _getCommandLength =6;////一度に送られてくるコマンドの文字の数
const int _sendCommandLength =7;//デバイス側に送る文字列の数(終端文字も入れる)

ESPNOW_Dev2ConData deviceData;

int outputFlag;
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
  }

  //PC(Unity)からデータを受け取ったら、それをデバイスに送る
  if(Serial.available() > 0)
  {
    char sendCommand[_sendCommandLength];

    for(int i=0; i<_getCommandLength ;i++)
    {
      sendCommand[i] = Serial.read();
    }
    sendCommand[_sendCommandLength-1] = '\0';//念のため最後を終端文字にする

    //文字列を送るcontrollerDataに入れる
    memcpy(controllerData.cmd, sendCommand, sizeof(controllerData.cmd));
    
    espnow.Send(1, &controllerData, sizeof(controllerData)); // id:1に送る
  }

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

