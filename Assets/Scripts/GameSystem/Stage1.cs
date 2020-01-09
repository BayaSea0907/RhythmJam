//=================================================================
//  ◆ Stage1.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 2018/2/14
//  Description:
//    Stage1管理。GameManagerのGameStateを引っ張ってきて管理します。
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1 : MonoBehaviour
{
    private bool isActive = true;
    [SerializeField] private FadeManager VRFm = null;
    [SerializeField] private FadeManager NotVRFm = null;
    private FadeManager fm;

    //----------------------------------------------------------
    // スタート
    //
    private void Start()
    {
        if (UnityEngine.XR.XRDevice.isPresent)
        {
            if (VRFm == null) Debug.Log("VRFadeManagerを設定してください");
            fm = VRFm;
        }
        else
        {
            if (NotVRFm == null) Debug.Log("NotVRFadeManagerを設定してください");
            fm = NotVRFm;
        }
        fm.FadeIn();
    }

    //----------------------------------------------------------
    // アップデート
    //
    private void Update ()
    {
		// Stage1終了のアニメーションを呼び出す
		if (GameManager.CurrentMusicBarOffset > GameManager.stage2StartTiming && isActive && GameManager.GameState == GameState.Stage2)
		{
			// 非表示にする前のアニメーション開始。  
			for (int i = 0; i < this.transform.childCount; i++)
			{
				iTween.ScaleTo(this.transform.GetChild(i).gameObject,
					iTween.Hash("scale", new Vector3(0.0f, 0.0f, 0.0f),
					"oncompletetarget", this.gameObject,
					"oncomplete", "EndActive"));
			}
			isActive = false;
		}
        else if (GameManager.CurrentMusicBarOffset > GameManager.stage2EndTiming && !isActive && GameManager.GameState == GameState.Stage3)
		{
            StartActive();
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
        isActive = true;
    }

    //----------------------------------------------------------
    // 非表示の処理
    //
    public void EndActive()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
