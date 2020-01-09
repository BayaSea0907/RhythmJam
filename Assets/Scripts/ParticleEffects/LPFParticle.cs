//=================================================================
//  ◆ LPFParticle.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi 2018/1/9
//  Description:
//    LowPassFilterの状態で変化するパーティクル
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPFParticle : MonoBehaviour
{
    [SerializeField] private List<Gradient> colorGradient;

    private ParticleSystem particleSystem;
    private int colorIdx;

    //----------------------------------------------------------
    // スタート
    //
    private void Start ()
    {
        colorIdx = 1;
        particleSystem = this.GetComponent<ParticleSystem>();
        particleSystem.Stop();
    }
	
	//----------------------------------------------------------
    // アップデート
    //
	private void Update ()
    {
        if (Music.CurrentSection.Name != "Wait")
        {
            // エフェクト適用中であれば
            if (AudioEffectsManager.LPF_Enabled)
            {
                if (!particleSystem.isPlaying) particleSystem.Play();
                TrailsUpdate();
            }
            else
            {
                particleSystem.Stop();
            }
        }
    }

    //----------------------------------------------------------
    // パーティクルの更新
    private void TrailsUpdate()
    {
        // Trailsモジュール
        var trails = particleSystem.trails;

        // LPFの値に応じてエフェクトの量を変更
        if(AudioEffectsManager.LPF_Level > 0.4f)
            trails.lifetime = 1.0f - AudioEffectsManager.LPF_Level;
        else
            trails.lifetime = 1.0f - AudioEffectsManager.LPF_Level * 0.5f;

        // リズムに合わせて色変え
        if (Music.Just.Bar > 0 && Music.Just.Bar % 8 == 0 && Music.IsJustChangedBar())
        {
            trails.colorOverLifetime = colorGradient[colorIdx];

            colorIdx++;
            if (colorIdx > colorGradient.Count - 1) colorIdx = 0;
        }
    }
}
