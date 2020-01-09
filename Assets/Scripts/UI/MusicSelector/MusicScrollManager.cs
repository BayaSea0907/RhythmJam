//=================================================================
//  ◆ MusicScrollManager.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 20180106
//  Description:
//    曲情報の扱いを管理。
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScrollManager : MonoBehaviour//, IScrollContent
{
    [SerializeField] private ScrollContentRoot contentsRoot;
    [SerializeField] List<MusicScrollContent> contentsList;

    //----------------------------------------------------------
    // スタート
    //
    private void Start()
    {
        if (contentsList == null)
        {
            contentsList = new List<MusicScrollContent>();
            Debug.Log("UI: 曲を設定してください");
        }

        //----------------------------------------------------------
        // リストビュー(Library)のコンテンツをイニシャライズ
        // 
        contentsRoot.SetContentsList(ref contentsList);
    }

    // コンテンツの取得
    public MusicScrollContent GetContent(int index)
    {
        return contentsList[index];
    }
}
