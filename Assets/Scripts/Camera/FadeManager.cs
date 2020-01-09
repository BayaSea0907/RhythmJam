//=================================================================
//  ◆ FadeManager_old.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 2018/3/10
//  Description:
//    OculusVRを考慮する事を前提としている
//=================================================================
using System.Collections;
using UnityEngine;

public enum FadeState
{
    Disable,
    FadeIn,
    FadeOut,
}

public class FadeManager : MonoBehaviour
{
    public bool isFading = false;
    public FadeState fadeState = FadeState.Disable;
    public float fadeInTime;
    public float fadeOutTime;

    // フェードアウトに合わせて、シーンを読みこむ
    public string nextSceneName = "";

    // OVRのフェード用シェーダ
    private Material ovrFadeMaterial = null;

    //----------------------------------------------------------
    // アウェイク
    //
    private void Awake()
    {
        // OVRのフェード用シェーダを取得
        ovrFadeMaterial = new Material(Shader.Find("Oculus/Unlit Transparent Color"));
    }

    //----------------------------------------------------------
    // デストロイ時
    //
    private void OnDestroy()
    {
        // シェーダ破棄
        if (ovrFadeMaterial != null)
        {
            Destroy(ovrFadeMaterial);
        }
    }

    //----------------------------------------------------------
    // フェードイン（黒→透明）
    //
    public void FadeIn()
    {
        Debug.Log("フェードイン開始");
        StartCoroutine("FadeInCoroutine");
    }
    // フェードイン処理の本体
    private IEnumerator FadeInCoroutine()
    {
        while (true)
        {
            // 別のフェード使用中の場合は待機
            if (isFading) yield return 0;
            else break;
        }
        isFading = true;
        fadeState = FadeState.FadeIn;

        // フェード設定
        float time = 0.0f;
        ovrFadeMaterial.color = Color.black;

        // フェード更新
        while (time < fadeInTime)
        {
            yield return new WaitForFixedUpdate();
            time += Time.deltaTime;

            ovrFadeMaterial.color = new Color(
                ovrFadeMaterial.color.r,
                ovrFadeMaterial.color.g,
                ovrFadeMaterial.color.b,
                1.0f - Mathf.Clamp01(time / fadeInTime));
        }
        isFading = false;
        fadeState = FadeState.Disable;
    }

    //----------------------------------------------------------
    // フェードアウト（透明→黒）
    //
    public void FadeOut()
    {
        Debug.Log("フェーアウト開始");
        StartCoroutine("FadeOutCoroutine");
    }
    // フェードアウト処理の本体
    private IEnumerator FadeOutCoroutine()
    {
        while (true)
        {
            // 別のフェード使用中の場合は待機
            if (isFading) yield return 0;
            else break;
        }
        isFading = true;
        fadeState = FadeState.FadeOut;

        // フェード設定
        float time = 0.0f;
        ovrFadeMaterial.color = Color.clear;

        // フェード更新
        while (time <= fadeOutTime)
        {
            yield return new WaitForFixedUpdate();
            time += Time.deltaTime;

            ovrFadeMaterial.color = new Color(
                ovrFadeMaterial.color.r,
                ovrFadeMaterial.color.g,
                ovrFadeMaterial.color.b,
                Mathf.Clamp01(time / fadeOutTime));
        }

        // シーンがあればロード
        if (nextSceneName != "")
        {
            SceneLoader.LoadScene(nextSceneName);

            while (true)
            {
                // ロードが終わるまで待機
                if (SceneLoader.IsSceneLoadRunning) yield return 0;
                else break;
            }            
        }
        fadeState = FadeState.Disable;
        isFading = false;
    }


    //----------------------------------------------------------
    // 透明度更新し、フェードイン、フェードアウトを行う
    //
    // カメラがシーンのレンダリングを完了した後に呼び出される
    // このスクリプトに Camera がアタッチされていて有効である場合のみ呼び出される
    private void OnPostRender()
    {
        ovrFadeMaterial.SetPass(0);
        GL.PushMatrix();
        GL.LoadOrtho();
        GL.Color(ovrFadeMaterial.color);
        GL.Begin(GL.QUADS);
        GL.Vertex3(0f, 0f, -12f);
        GL.Vertex3(0f, 1f, -12f);
        GL.Vertex3(1f, 1f, -12f);
        GL.Vertex3(1f, 0f, -12f);
        GL.End();
        GL.PopMatrix();
    }
}
