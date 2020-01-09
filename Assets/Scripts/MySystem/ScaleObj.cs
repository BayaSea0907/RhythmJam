//=================================================================
//  ◆ ScaleObj.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi   2018/3/10
//  Description:
//    アタッチしているオブジェクトを任意のタイミングでスケーリング
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleObj : MonoBehaviour
{
    [SerializeField] private bool isMusicJustBeat = true;

    [SerializeField, Header("Max")] private Vector3 maxScale = Vector3.one;
    [SerializeField] private float maxAnimTime = 0.1f;
    [SerializeField] private iTween.EaseType maxEaseType = iTween.EaseType.easeOutCirc;

    [SerializeField, Header("Min")] private Vector3 minScale = Vector3.one;
    [SerializeField] private float minAnimTime = 0.25f;
    [SerializeField] private iTween.EaseType minEaseType = iTween.EaseType.easeInCubic;

    //----------------------------------------------------------
    // スタート
    //
    private void Start()
    {
        if (isMusicJustBeat)
        {
            // リズム（ビート）に合わせてスケーリング
            StartCoroutine(RhythmScalingUpdate());
        }
        else
        {
            // スケーリングアニメーション起動
            StartCoroutine(ScalingUpdate());
        }
    }

    //----------------------------------------------------------
    // ビートに合わせたスケールアニメーション
    //
    private IEnumerator RhythmScalingUpdate()
    {
        while (true)
        {
            while (true)
            {
                if (!Music.IsJustChangedBeat()) yield return 0;
                else break;
            }

            // 大きく
            iTween.ScaleTo(this.gameObject, iTween.Hash(
            "scale",    maxScale,
            "time",     maxAnimTime,
            "easeType", maxEaseType));

            // 終わるまで待つ
            yield return new WaitForSeconds(maxAnimTime);

            // 小さく
            iTween.ScaleTo(this.gameObject, iTween.Hash(
            "scale",    minScale,
            "time",     minAnimTime,
            "easeType", minEaseType));
        }
    }


    //----------------------------------------------------------
    // スケールアニメーション
    //
    private IEnumerator ScalingUpdate()
    {
        while (true)
        {
            // 大きく
            iTween.ScaleTo(this.gameObject, iTween.Hash(
            "scale",    maxScale,
            "time",     maxAnimTime,
            "easeType", maxEaseType));

            // 終わるまで待つ
            yield return new WaitForSeconds(maxAnimTime);

            // 小さく
            iTween.ScaleTo(this.gameObject, iTween.Hash(
            "scale",    minScale,
            "time",     minAnimTime,
            "easeType", minEaseType));

            // 終わるまで待つ
            yield return new WaitForSeconds(minAnimTime);
        }
    }
}
