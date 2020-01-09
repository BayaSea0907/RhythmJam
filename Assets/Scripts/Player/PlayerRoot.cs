//=================================================================
//  ◆ PlayerRoot.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 2018/2/14
//  Description:
//    Playerとして移動するオブジェクトを一括管理。
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoot : MonoBehaviour
{
    [SerializeField] private GameObject Stage2EffectObject;

    // Stage2
    [SerializeField, Header("Stage2")] private iTweenPath stage2Path;
    [SerializeField] private float stage2Time;

    // Stage3
    [SerializeField, Header("Stage3")] private iTweenPath stage3Path;
    [SerializeField] private float stage3Time;
    [SerializeField] private RotateObj rotateSystem;

    // Result
    [SerializeField, Header("Result")] private GameObject VRThankYouText;
    [SerializeField] private GameObject NotVRThankYouText;

    [SerializeField, Space(5)] private List<GameObject> VRObjects;
	[SerializeField] private List<GameObject> NotVRObjects;

	//----------------------------------------------------------
	// アウェイク
	//
	private void Awake()
	{
        //----------------------------------------------------------
        // オブジェクトの表示、非表示
		// VRデバイスを取得できた時
		if (UnityEngine.XR.XRDevice.isPresent)
		{
			for (int i = 0; i < VRObjects.Count; i++)
			{
				VRObjects[i].SetActive(true);
			}

			for(int i = 0; i < NotVRObjects.Count; i++)
			{
				NotVRObjects[i].SetActive(false);
			}
		}
		// VRデバイスを取得で着ないとき
		else
		{
			for (int i = 0; i < VRObjects.Count; i++)
			{
				VRObjects[i].SetActive(false);
			}

			for (int i = 0; i < NotVRObjects.Count; i++)
			{
				NotVRObjects[i].SetActive(true);
			}
		}
        Stage2EffectObject.SetActive(false);
    }

    //----------------------------------------------------------
    // スタート
    //
    private void Start()
    {
        // メインコルーチン起動
        StartCoroutine(PlayerRootUpdate());
    }

    //----------------------------------------------------------
    // アップデート
    // 一連のプレイヤーの流れを処理
    private IEnumerator PlayerRootUpdate()
    {
        while (true)
        {
            // Stage2であれば移動開始
            if (GameManager.CurrentMusicBarOffset > GameManager.stage2StartTiming)
            {
                Stage2EffectObject.SetActive(true);

                // iTweenで移動開始
                iTween.MoveTo(this.gameObject, iTween.Hash(
                "path", iTweenPath.GetPath(stage2Path.pathName),
                "time", stage2Time,
                "easeType", iTween.EaseType.linear));

                break;
            }
            // 待機
            else yield return 0;
        }
        // iTweenが終わるまで待機
        yield return new WaitForSeconds(stage2Time);

        // Stage3の演出を起動
        Stage2EffectObject.SetActive(false);
        rotateSystem.enabled = true;

        // iTweenPath3を起動
        iTween.MoveTo(this.gameObject, iTween.Hash(
        "path", iTweenPath.GetPath(stage3Path.pathName),
        "time", stage3Time,
        "easeType", iTween.EaseType.linear ));

        // iTweenが終わるまで待機
        yield return new WaitForSeconds(stage3Time);

        // ThankYouForPlaying!!
        if (UnityEngine.XR.XRDevice.isPresent)
        {
            VRThankYouText.SetActive(true);
        }
        else
        {
            NotVRThankYouText.SetActive(true);
        }
    }
}
