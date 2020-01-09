//=================================================================
//  ◆ LPFKnob.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 20171218
//  Description:
//    ローパスカットのノブ操作に使います
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;


public class LPFKnob : MonoBehaviour
{
    [SerializeField] private AudioMixer myAudioMixer;

    // Bypass適用判定用
    [SerializeField, Space(5), Header("LowPass")] private MyButton enableButton;
    [SerializeField] private Knob frequencyKnob;
    [SerializeField] private Knob resonanceKnob;

    [Range(500.0f, 6000.0f)] private float frequency;
    [Range(1.0f, 2.0f)]      private float resonance;
    private bool initFlg = false;

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
        if (enableButton.IsCurrentKeyDown() || frequencyKnob.isHitting)
		{
            // frequency
            if (Input.GetKey(KeyCode.LeftArrow)) { frequencyKnob.RotateKnob(-150.0f * Time.deltaTime); }
            else if (Input.GetKey(KeyCode.RightArrow)) { frequencyKnob.RotateKnob(150.0f * Time.deltaTime); }

            // resonance
            // if (Input.GetKey(KeyCode.UpArrow)) { resonanceKnob.RotateKnob(-100.0f * Time.deltaTime); }
            // else if (Input.GetKey(KeyCode.DownArrow)) { resonanceKnob.RotateKnob(100.0f * Time.deltaTime); }

            // ノブの値を適用
            frequency = ((6000.0f - 500.0f) * frequencyKnob.GetcurrentRotateValue()) + 500.0f;
            // resonance = ((2.0f - 1.0f) * resonanceKnob.GetcurrentRotateValue()) + 1.0f;
        }

        EffectUpdate();
    }

	// エフェクト適用
    private void EffectUpdate()
    {
        // エフェクトを適用するか
        if (enableButton.IsPushed() || frequencyKnob.isHitting)
        {
            if (!initFlg)  myAudioMixer.SetFloat("bgm_LPF_Wet", 0.0f);

            myAudioMixer.SetFloat("bgm_LPF_Frequency", frequency);
            // myAudioMixer.SetFloat("bgm_LPF_Resonance", resonance);
            initFlg = true;
        }
        // 初期化されてなければする
        else if(initFlg)
        {
            Initialize();
            myAudioMixer.SetFloat("bgm_LPF_Wet", -80.0f);
        }
    }

    //----------------------------------------------------------
    // アウェイク
    // 
    private void Initialize()
    {
        frequency = 6000.0f;
        resonance = 1.0f;
        myAudioMixer.SetFloat("bgm_LPF_Frequency", frequency);
        myAudioMixer.SetFloat("bgm_LPF_Resonance", resonance);
        initFlg = false;
    }

    // 状態判別用
    public bool IsEnabled() { return enableButton.IsPushed(); }
    
    // 0.0~1.0で返す。初期値は 0.0f
    public float GetEffectLevel()
    {
        if (IsEnabled())            
            return Mathf.Max(frequencyKnob.GetcurrentRotateValue(), 0.1f);
        else
            return 0.0f;
    }
}
