//=================================================================
//  ◆ TitleCameraMover.cs
//-----------------------------------------------------------------
//  Author:
//    K.Endo
//    H.Kobayashi
//  Description:
//   タイトル用のカメラ制御に使います。
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCameraMover : MonoBehaviour
{
    [SerializeField] private FadeManager fm = null;
    [SerializeField] private string currentPathName;
    public List<iTweenPath> _path;
    public List<float> times;
    private bool isInput = false;


    //----------------------------------------------------------
    // スタート
    private void Start()
    {
        if (fm == null)
        {
            Debug.Log("FadeManagerを設定してください");
        }

        // 移動処理開始
        StartCoroutine(CameraMoveITween(_path));
    }

    //----------------------------------------------------------
    // アップデート
    //
    private void Update()
    {
        // 入力があれば次のシーンへ
        if ((OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) ||
            OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) ||
            (Input.GetKeyDown(KeyCode.Return)) && !isInput))
        {
            isInput = true;
            StopCoroutine(CameraMoveITween(_path));

            fm.nextSceneName = "GameMain";
            StartCoroutine("NextScene");
        }
    }

    //----------------------------------------------------------
    // カメラ移動処理
    //
    private IEnumerator CameraMoveITween(List<iTweenPath> path)
    {
        int i = 0;

        while (true)
        {
            // パスが始まる前にフェードイン
            fm.FadeIn();

            //パスを通したオブジェクトのパスを取得
            currentPathName = path[i].GetComponent<iTweenPath>().pathName;

            //パスの一番目の座標にカメラを移動
            this.transform.position = path[i].nodes[0];

            //パスを通してカメラを移動
            iTween.MoveTo(this.gameObject, iTween.Hash("path", iTweenPath.GetPath(currentPathName),
                "time", times[i], "orienttopath", false,
                "easetype", iTween.EaseType.linear,
                "looptype", iTween.LoopType.none));

            yield return new WaitForSeconds(times[i] - fm.fadeOutTime);

            // 現在のパスが終わる前にフェードアウト
            fm.FadeOut();

            // フェードアウトが終わるまで待つ
            while (true)
            {
                if (fm.fadeState == FadeState.FadeOut) yield return 0;
                else break;
            }

            // 次のパスへ
            if (i < path.Count-1)
            {
                i++;
            }
            else
            {
                i = 0;
            }
        }
    }

    //----------------------------------------------------------
    // シーン遷移処理
    //
    private IEnumerator NextScene()
    {
        // すでにフェードアウトが走っていたら処理を抜ける
        if (fm.fadeState == FadeState.FadeOut) yield return 0;

        while (true)
        {
            // フェードイン中だった場合待機
            if (fm.fadeState == FadeState.FadeIn) yield return 0;
            else break;
        }

        fm.FadeOut();
    }
}