// 送信するデータ形式（構造体）を定義
// このファイルはESPNowEz.hと同一フォルダにあること
// ArduinoIDEではスケッチファイルより上位の階層にあるファイルはincludeできない制約があるため、ここで定義
// 一意であるべき定義が書かれたファイルが通信対象のスケッチがある場所にコピーされるが、現状ではやむを得ず

// プロジェクトごとに必要な構造体を作る
// Device側が複数存在する場合は、idを含めておくことを推奨

//送るデータは加速度センサーのジャイロ(XとZ)、スイッチ(1つ)、ロータリーエンコーダー(or ボリューム)の値

#ifndef __ESPNOW_SEND_DATA_H_
#define __ESPNOW_SEND_DATA_H_

//デバイスからコントローラーに送るデータ
typedef struct struct_esp_now_d2c_data {
    uint8_t id; // 必ず被らないこと
    //加速度センサーのジャイロ
    int gyroX;
    int gyroZ;
    //スイッチ
    int8_t sw;
    //ロータリーエンコーダーの変化値
    int delta;   
} ESPNOW_Dev2ConData;

//コントローラーからデバイスに送るデータ
typedef struct struct_esp_now_c2d_data {
    uint8_t id; // 必ず被らないこと
    char cmd;
    int rate;
} ESPNOW_Con2DevData;

#endif
