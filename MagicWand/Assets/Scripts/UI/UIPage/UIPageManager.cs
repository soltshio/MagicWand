using TMPro;
using UnityEngine;

//作成者:杉山
//UIのページを管理するマネージャー

[System.Serializable]
struct PageSetting
{
    public string pageLabelText;
    public GameObject pageObject;
}

public class UIPageManager : MonoBehaviour
{
    [Tooltip("ページの設定\n最初に設定された項目を１ページとする")] [SerializeField]
    PageSetting[] _pages;

    [SerializeField]
    TextMeshProUGUI _pageLabelText;

    int _currentPageIndex = 0;

    //左のページ(ページ数でいえば減る方向)に切り替える
    public void ChangeToLeftPage()
    {
        int nextPageIndex = MathfExtension.CircularWrapping(_currentPageIndex - 1, 0, _pages.Length - 1);
        ChangeToPage(nextPageIndex);
    }

    //右のページ(ページ数でいえば減る方向)に切り替える
    public void ChangeToRightPage()
    {
        int nextPageIndex = MathfExtension.CircularWrapping(_currentPageIndex + 1, 0, _pages.Length - 1);
        ChangeToPage(nextPageIndex);
    }

    public void ChangeToPage(int pageIndex)
    {
        if (pageIndex < 0 || pageIndex >= _pages.Length)
        {
            Debug.LogError("ページのインデックスが範囲外です");
            return;
        }

        //指定のページのUIオブジェクトだけをアクティブにする
        for (int i = 0; i < _pages.Length; i++)
        {
            _pages[i].pageObject.SetActive(i == pageIndex);
        }

        //ページラベルのテキストを更新
        _pageLabelText.text = _pages[pageIndex].pageLabelText;

        //現在のページインデックスを更新
        _currentPageIndex = pageIndex;
    }

    void Start()
    {
        _currentPageIndex = 0;
        ChangeToPage(_currentPageIndex);
    }
}
