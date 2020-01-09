//=================================================================
//  ◆ Opening.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi   2018/2/23
//  Description:
//   オープニングシーンの管理
//=================================================================
using System.Collections;
using UnityEngine;

public class Opening : MonoBehaviour
{
	// フェードシステム
    [SerializeField] private FadeManager fm = null;

	// ロゴ
    [SerializeField] private SpriteRenderer jecLogo;
    [SerializeField] private SpriteRenderer tukumoLogo;
	[SerializeField] private float tukumologoVisibleTime;
	[SerializeField] private float jeclogoVisibleTime;
	private bool isInput = false;


    //----------------------------------------------------------
    // スタート
    //
    private void Start ()
    {
        // フェードの初期化と、フェードイン
        if (fm == null)
        {
            Debug.Log("FadeManagerを設定してください");            
        }
        fm.nextSceneName = "Title";

        // オープニングアニメーション処理開始
        StartCoroutine("OpeningLoop");
	}

    //----------------------------------------------------------
    // アップデート
    //
    private void Update()
    {
        // 入力があれば次のシーンへ
        if ((OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger)) ||
            Input.GetKeyDown(KeyCode.Return) && !isInput)
        {
            isInput = true;

            // アニメーションをストップ
            StopCoroutine(OpeningLoop());

            // 強制シーン遷移コルーチン呼び出し
            StartCoroutine(NextScene());
        }        
    }
	//----------------------------------------------------------
	// オープニングアニメーションの処理
	//
	private IEnumerator OpeningLoop()
    {
		// フェードイン
		fm.FadeIn();

        // フェードインが終わるまで待つ
        yield return new WaitForSeconds(fm.fadeInTime);

		// 日電ロゴを指定秒表示
		yield return new WaitForSeconds(jeclogoVisibleTime);

		// ツクモロゴと入れ替え
		while (true)
        {
            jecLogo.color = new Color(jecLogo.color.r, jecLogo.color.g, jecLogo.color.b, (jecLogo.color.a - (0.5f * Time.deltaTime)));
            tukumoLogo.color = new Color(jecLogo.color.r, jecLogo.color.g, jecLogo.color.b, (tukumoLogo.color.a + (0.5f * Time.deltaTime)));

            // 入れ替えが終わればループを抜ける
            if (tukumoLogo.color.a >= 1.0f) break;
            else yield return 0;
        }

        // ツクモロゴを指定秒表示
        yield return new WaitForSeconds(jeclogoVisibleTime);
		
		// フェードアウト
        fm.FadeOut();
	}

    //----------------------------------------------------------
    // 強制シーンロード処理
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
