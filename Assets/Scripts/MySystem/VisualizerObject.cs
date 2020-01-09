//=================================================================
//  ◆ VisualizerObject.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 2018/1/27
//  Description:
//    オーディオのSpectrumデータを反映させる。
//=================================================================
using System.Collections.Generic;
using UnityEngine;

public enum SpectrumType
{
    AudioListener, AudioSource
}
public class VisualizerObject : MonoBehaviour
{
    [SerializeField] private bool isColorAnim;
    [SerializeField] private bool isParticle;

    // アニメーション設定
    [SerializeField, Header("Audio")] private SpectrumType spectrumType;     // Listenerか、Sourceか
    [SerializeField] private AudioSource audioSource = null; 
    [SerializeField] private FFTWindow FFTType;             // 波形の滑らかさ
    [SerializeField] private int samples = 1024;

    // スケールサイズ
    [SerializeField] private float 　maxScaleSize;
    [SerializeField] private Vector3 minScaleSize;
    [SerializeField] private float scallingSpeed = 30.0f;


    // グラデーションカラー
    [SerializeField, Header("Coloring")] private int currentDefColor;
    [SerializeField] private List<Gradient> defColor;
    [SerializeField] private float gradationSpeed = 1.0f;

    private float[] spectrum;   // 波形データ


    //----------------------------------------------------------
    // スタート
    //
    private void Start()
    {
    }

    //----------------------------------------------------------
    // アップデート
    //
    private void Update ()
    {
        ScaleAnimation();
    }

    //----------------------------------------------------------
    // アニメーション各種
    //

    // スケールアニメーション
    private void ScaleAnimation()
    {
        if (spectrumType == SpectrumType.AudioSource)
        {
            // AudioSourceから波形データ取得
            spectrum = audioSource.GetSpectrumData(1024, 0, FFTType);
        }
        else
        {
            // AudioListenerから波形データ取得
            spectrum = AudioListener.GetSpectrumData(samples, 0, FFTType);
        }

        // 子に対してアニメーション
        for (int i = 0; i < this.transform.childCount; i++)
        {
            GameObject child = this.transform.GetChild(i).gameObject;
            if (spectrum[i] > 0.7) spectrum[i] *= 0.5f;

            // 現在のスケール値取得
            Vector3 scaleValue = child.transform.localScale;
            float colorValue = 0.0f;

            // 波形データをもとにアニメーション
            scaleValue.y = minScaleSize.y + Mathf.Lerp(scaleValue.y, spectrum[i] * maxScaleSize, Time.deltaTime * scallingSpeed);
            colorValue = scaleValue.y;

            // スケール値の適用
            child.transform.localScale = scaleValue;

            // カラーアニメーション
            if (isColorAnim)
            {
                child.GetComponentInChildren<ParticleSystem>().startColor = defColor[currentDefColor].Evaluate(colorValue / gradationSpeed);
            }

            if (isParticle)
            {
                // パーティクルの光を強調するアニメーション
                var main = child.GetComponentInChildren<ParticleSystem>().main;
                main.startSize = Music.MusicalCos(16.0f, 0.0f, 0.2f, 0.3f);
            }
        }

        // カラーアニメーション
        if (isColorAnim)
        {
            // 子のさらに子にたいしてアニメーション
            if (Music.Just.Bar % 8 == 0 && Music.IsJustChangedBar() )
            {
                // 次の色へ
                currentDefColor++;

                if (currentDefColor >= defColor.Count)
                {
                    currentDefColor = 0;
                }
            }
        }
    }
}
