//=================================================================
//  ◆ FlangeKnob.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 20171218
//  Description:
//    フレンジのノブ操作に使います
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class FlangeKnob : MonoBehaviour
{
    [SerializeField] private AudioMixer myAudioMixer;

    // Bypass適用判定用
    [SerializeField, Space(5), Header("Flange")] private MyButton enableButton;
    [SerializeField] private Knob drymixKnob;
    [SerializeField] private Knob wetmixKnob;
    [SerializeField] private Knob depthKnob;
    [SerializeField] private Knob rateKnob;

    [Range(0.0f, 100.0f)]   private float drymix;
    [Range(0.0f, 100.0f)]   private float wetmix;
    [Range(0.01f, 1.0f)]    private float depth;
    [Range(0.0f, 20.0f)]    private float rate;

    //----------------------------------------------------------
    // スタート
    // 
    private void Start()
    {
        Initialize();
    }

    //----------------------------------------------------------
    // アップデート
    //
    private void Update()
    {
        // キーボード操作でノブを回す
        if (enableButton.IsCurrentKeyDown())
        {
            if      (Input.GetKey(KeyCode.LeftArrow))   { depthKnob.RotateKnob(-100.0f * Time.deltaTime); }
            else if (Input.GetKey(KeyCode.RightArrow))  { depthKnob.RotateKnob( 100.0f * Time.deltaTime); }
        }

        // ノブの値を適用
        drymix  = (100.0f   - 0.0f)     * drymixKnob.GetcurrentRotateValue();
        wetmix  = (100.0f   - 0.0f)     * wetmixKnob.GetcurrentRotateValue();
        depth   = (1.0f     - 0.0f)     * depthKnob.GetcurrentRotateValue();
        rate    = (20.0f    - 0.0f)     * rateKnob.GetcurrentRotateValue();

        // エフェクトを適用するか
        if (enableButton.IsPushed())
        {
            myAudioMixer.SetFloat("bgm_Flange_Wet", 0.0f);

            myAudioMixer.SetFloat("bgm_Flange_Drymix",  drymix);
            myAudioMixer.SetFloat("bgm_Flange_Wetmix",  wetmix);
            myAudioMixer.SetFloat("bgm_Flange_Depth",   depth);
            myAudioMixer.SetFloat("bgm_Flange_Rate",    rate);
        }
        else
        {
            Initialize();
            myAudioMixer.SetFloat("bgm_Flange_Wet", -80.0f);     
        }
    }

    //----------------------------------------------------------
    // アウェイク
    //
    private void Initialize()
    {
        drymix = 100.0f;
        wetmix = 0.0f;
        depth = 1.0f;
        myAudioMixer.SetFloat("bgm_Flange_Drymix", drymix);
        myAudioMixer.SetFloat("bgm_Flange_Wetmix", wetmix);
        myAudioMixer.SetFloat("bgm_Flange_Depth", depth);
        myAudioMixer.SetFloat("bgm_Flange_Rate", rate);
    }

    // 状態判別用
    public bool IsEnabled() { return enableButton.IsPushed(); }

    // 0.0~1.0で返す。初期値は 1.0
    public float GetEffectLevel()
    {
        if (IsEnabled())
            return depthKnob.GetcurrentRotateValue();
        else
            return 0.0f;
    }
}
