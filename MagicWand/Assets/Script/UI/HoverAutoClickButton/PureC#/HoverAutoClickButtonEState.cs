using UnityEngine;

//作成者:杉山
//数秒間カーソルを合わせていると、自動クリックするボタンの状態

public enum HoverAutoClickButtonEState
{
   Idle,//待機状態
   Hovering,//カーソルを合わせている状態
   Clicked,//クリックされた状態
}
