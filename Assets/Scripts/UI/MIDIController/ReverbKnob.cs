//=================================================================
//  ◆ Reverb}Knob.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 20171218
//  Description:
//    リバーブのノブ操作に使います
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;


public class ReverbKnob : MonoBehaviour
{
    [SerializeField] private AudioMixer myAudioMixer;

    // Bypass適用判定用
    [SerializeField, Space(5), Header("Reverb")] private MyButton enableButton;
    [SerializeField] private Knob roomKnob;
    [SerializeField] private Knob reverbKnob;
    [SerializeField] private Knob reflectionsKnob;

    [Range(-1000.0f, 0.0f)]     private float room;
    [Range(-300.0f, 1500.0f)]   private float reverb;
    [Range(-500.0f, 1000.0f)]   private float reflections;
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
		if (enableButton.IsCurrentKeyDown())
		{
			if (Input.GetKey(KeyCode.LeftArrow)) { reverbKnob.RotateKnob(-150.0f * Time.deltaTime); }
			else if (Input.GetKey(KeyCode.RightArrow)) { reverbKnob.RotateKnob(150.0f * Time.deltaTime); }

			// ノブの値を適用
			reverb = ((1500.0f - (-300.0f)) * reverbKnob.GetcurrentRotateValue()) + (-500.0f);
			// room = ((0.0f - (-1000.0f)) * roomKnob.GetcurrentRotateValue()) + (-1000.0f);
			// reflections = ((1000.0f - (-500.0f)) * reflectionsKnob.GetcurrentRotateValue()) + (-500.0f);
		}
		EffectUpdate();
    }

	// エフェクト適用
	private void EffectUpdate()
	{
		// エフェクトを適用するか
		if (enableButton.IsPushed() || reverbKnob.isHitting)
		{
			if (!initFlg) myAudioMixer.SetFloat("bgm_Reverb_Wet", 0.0f);

			myAudioMixer.SetFloat("bgm_Reverb", reverb);
			// myAudioMixer.SetFloat("bgm_HPF_Resonance", resonance);
			initFlg = true;
		}
		else if (initFlg)
		{
			Initialize();
			myAudioMixer.SetFloat("bgm_HPF_Wet", -80.0f);
		}
	}


	//----------------------------------------------------------
	// アウェイク
	// 
	private void Initialize()
    {
        room = 0.0f;
        reverb = -300.0f;
        reflections = -500.0f;
        myAudioMixer.SetFloat("bgm_Reverb_Room", room);
        myAudioMixer.SetFloat("bgm_Reverb", reverb);
        myAudioMixer.SetFloat("bgm_Reverb_Reflections", reflections);
        initFlg = false;
    }

    // 状態判別用
    public bool IsEnabled() { return enableButton.IsPushed(); }
    public float GetEffectLevel()
    {
        if (IsEnabled() || reverbKnob.isHitting)
            return roomKnob.GetcurrentRotateValue();
        else
            return 0.0f;
    }
}
