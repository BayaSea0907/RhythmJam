//=================================================================
//  ◆ PitchKnob.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 20171218
//  Description:
//    ピッチのノブ操作に使います
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;


public class PitchKnob : MonoBehaviour
{
    [SerializeField] private AudioMixer myAudioMixer;

    // Bypass適用判定用
    [SerializeField, Space(5), Header("Pitch")] private MyButton enableButton;
    [SerializeField] private Knob pitchKnob;

    [Range(0.8f, 1.2f)] private float pitch;
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
        if (enableButton.IsCurrentKeyDown() || pitchKnob.isHitting)
        {
            if      (Input.GetKey(KeyCode.LeftArrow))   { pitchKnob.RotateKnob(-150.0f * Time.deltaTime); }
            else if (Input.GetKey(KeyCode.RightArrow))  { pitchKnob.RotateKnob( 150.0f * Time.deltaTime); }

			// ノブの値を適用
			pitch = ((1.2f - 0.8f) * pitchKnob.GetcurrentRotateValue()) + 0.8f;
        }
		EffectUpdate();
    }

	// エフェクト適用
	private void EffectUpdate()
	{
		// エフェクトを適用するか
		if (enableButton.IsPushed() || pitchKnob.isHitting)
		{
			if (!initFlg) myAudioMixer.SetFloat("bgm_Pitch_Wet", 0.0f);
			myAudioMixer.SetFloat("bgm_PitchShifter", pitch);
			initFlg = true;
		}
		else if (initFlg)
		{
			Initialize();
			myAudioMixer.SetFloat("bgm_Pitch_Wet", -80.0f);
		}
	}

    //----------------------------------------------------------
    // アウェイク
    // 
    private void Initialize()
    {
        pitch = 1.0f;
        myAudioMixer.SetFloat("bgm_PitchShifter", pitch);
        initFlg = false;
    }

    // 状態判別用
    public bool IsEnabled() { return enableButton.IsPushed(); }

    // 0.0~1.0で返す。初期値は 0.5
    public float GetEffectLevel()
    {
        if (IsEnabled() || pitchKnob.isHitting)
            return pitchKnob.GetcurrentRotateValue();
        else
            return 0.0f;
    }
}
