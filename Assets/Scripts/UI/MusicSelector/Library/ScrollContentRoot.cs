//=================================================================
//  ◆ ScrollContentRoot.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 20180106
//  Description:
//    スクロールさせたいコンテンツの親。
//    コンテンツの配置と入力操作を管理。
//    実際にスクロールで動かすのはこのオブジェクト。
//=================================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

// コンポーネントの強制アタッチ
[RequireComponent(typeof(RectTransform))]
public class ScrollContentRoot : UIBehaviour
{
    [SerializeField] private int selectIdx;
    private List<MusicScrollContent> contentsList;

    //----------------------------------------------------------
    // スタート
    //
    protected override void Start()
    {
        selectIdx = 0;

        // var scrollRect = this.GetComponentInParent<ScrollRect>();
        // scrollRect.content = rectTransform;
    }

    //----------------------------------------------------------
    // アップデート
    //
    private void Update()
    {
        //----------------------------------------------------------
        // 曲選択
        //
        if (Input.GetKey(KeyCode.F1))
        {
            // 十字キーで曲選択処理
            // ↑
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                selectIdx--;
                ContentChoise();
            }
            // ↓
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                selectIdx++;
                ContentChoise();
            }

            // 決定
            if (Input.GetKeyDown(KeyCode.Return))
            {
				ReSelectAll();
				contentsList[selectIdx].Select();
            }
        }
    }

	// コンテンツの全選択解除
	public void ReSelectAll()
	{
		for (int i = 0; i < contentsList.Count; i++)
		{
			contentsList[i].ResetSelect();
		}
	}
    // コンテンツの選択操作
    private void ContentChoise()
    {
        // 選択中のリセット
        for(int i = 0; i < contentsList.Count; i++)
        {
            contentsList[i].ResetChoise();
        }

		// 添え字の制限
		if (selectIdx >= contentsList.Count)
		{
			selectIdx = contentsList.Count - 1;
		}
		else if (selectIdx < 0)
		{
			selectIdx = 0;
		}

        contentsList[selectIdx].Choise();
    }

    public void SetContentsList(ref List<MusicScrollContent> contents)
    {
        contentsList = contents;
    }
}
