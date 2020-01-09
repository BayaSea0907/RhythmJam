//=================================================================
//  ◆ FadeManager_old.cs
//-----------------------------------------------------------------
//  Author:
//    M.Higuchi - 2018/2/14
//  Description:
//    フェードマネージャー
//=================================================================
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeManager_old : MonoBehaviour
{    
	private OVRScreenFade ovrFade;	
    public  FadeState fadeState { get { return _fadeState; } }
    private FadeState _fadeState = FadeState.Disable;
    private FadeState fadeStatePrev = FadeState.Disable;
    public bool isRunning = false;

    public string nextSceneName = "";
    public float fadeInTime = 5.0f;
    public float fadeOutTime = 1.0f;
    public float fadeLevel = 1.0f;   // 透明度

    private Texture2D blackTexture; // テクスチャ設定用

    // blackTexture黒テクスチャ設定
    void Awake()
    {
        // テクスチャを設定
        blackTexture = new Texture2D(32, 32, TextureFormat.RGB24, false);
        blackTexture.SetPixel(0, 0, Color.white);
        blackTexture.Apply();

		// OVRフェード
		// ovrFade = this.GetComponent<OVRScreenFade>();        
    }

    //----------------------------------------------------------
    // アップデート
    //
    private void Update()
    {
        // fadeStateに変更があればフェードを掛ける
        if(fadeState != fadeStatePrev)
        {            
            fadeStatePrev = fadeState;

            if (fadeState == FadeState.FadeIn)
            {
                FadeIn();
            }
            else if (fadeState == FadeState.FadeOut)
            {
                FadeOut();
            }
        }
    }
   
    //----------------------------------------------------------
    // フェードイン (黒→透明)
    //
    public void FadeIn()
    {
        Debug.Log("フェードイン開始");
        StartCoroutine("FadeInCoroutine");
    }
    // フェードイン処理の本体
    private IEnumerator FadeInCoroutine()
    {
        // 実行中であれば処理しない
        if (isRunning) yield break;
        isRunning = true;

        fadeLevel = 1.0f;

        // フェード更新
        while (fadeLevel >= 0.0f)
        {
            fadeLevel -= Time.deltaTime / fadeInTime;
            // fadeLevel -= fadeInTime * Time.deltaTime;
            yield return 0;
        }

        // フェード終わり
        _fadeState = FadeState.Disable;
        isRunning = false;
    }

    //----------------------------------------------------------
    // フェードアウト（透明→黒）
    //
    public void FadeOut()
    {
        Debug.Log("フェードアウト開始");
        StartCoroutine("FadeOutCoroutine", nextSceneName);
    }

    // フェードアウト処理の本体
    private IEnumerator FadeOutCoroutine(string sceneName = "")
    {
        // 実行中であれば処理しない
        if (isRunning) yield break;
        isRunning = true;

        fadeLevel = 0.0f;

        while (fadeLevel <= 1)
        {
            fadeLevel += Time.deltaTime / fadeOutTime;
            // fadeLevel += fadeOutTime * Time.deltaTime;
            yield return 0;
        }


        // シーンがあればロード
        if (sceneName != "")
        {
            SceneLoader.LoadScene(sceneName);

            // シーンロード中であれば待機
            if (SceneLoader.IsSceneLoadRunning)
            {
                yield return new WaitForFixedUpdate();
            }
            nextSceneName = "";
        }

        // フェード終了
        _fadeState = FadeState.Disable;
        isRunning = false;
    }

    //----------------------------------------------------------
    // 透明度更新し、フェードイン、フェードアウトを行う
    //
    private void OnGUI()
    {
        // 透明度を更新
        GUI.color = new Color(0, 0, 0, fadeLevel);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackTexture);
    }
}
