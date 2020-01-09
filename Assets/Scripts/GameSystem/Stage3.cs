//=================================================================
//  ◆ Stage3.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 2018/2/14
//  Description:
//    Stage1管理。GameManagerのGameStateを引っ張ってきて管理します。
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3 : MonoBehaviour
{
    private bool isActive = false;

    //----------------------------------------------------------
    // アップデート
    //
    private void Update()
    {
        // 非表示時
        if (!isActive)
        {
            if(Music.Just.Bar > GameManager.stage2StartTiming + 2)
            {
                StartActive();
                isActive = true;
            }
        }
    }

    //----------------------------------------------------------
    // 表示するときの処理
    //
    private void StartActive()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).localScale = Vector3.one;
            this.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    //----------------------------------------------------------
    // 非表示の処理
    //
    private void EndActive()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
