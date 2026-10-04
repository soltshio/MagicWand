using System.Security.Policy;
using UnityEngine;

//ゲームのバージョンの変換などのユーティリティクラス

public static class VersionUtility
{
    //string型からSystem.Version型に変換する
    public static bool StringToVersion(string versionString,out System.Version version)
    {
        // バージョン文字列がnullまたは空の場合のエラーハンドリング
        if (string.IsNullOrEmpty(versionString))
        {
            Debug.LogError("Version string is null or empty.");
            version = new System.Version(0, 0, 0, 0);
            return false;
        }

        try
        {
            //変換に成功
            version = new System.Version(versionString);
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to convert version string to System.Version: {e.Message}");
            version = new System.Version(0, 0, 0, 0);
            return false;
        }
    }

    //System.Version型からstring型に変換する
    public static bool VersionToString(System.Version version, out string versionString)
    {
        // バージョンがnullの場合のエラーハンドリング
        if (version == null)
        {
            Debug.LogError("Version is null.");
            versionString = "0.0.0.0";
            return false;
        }

        versionString = version.ToString();
        return true;
    }
}
