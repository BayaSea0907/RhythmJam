//=================================================================
//  ◆ ChorusKnob.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 20171218
//  Description:
//    コーラスのノブ操作に使います
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;


public class ChorusKnob : MonoBehaviour
{
    [SerializeField] private AudioMixer myAudioMixer;

    // Bypass適用判定用
    [SerializeField, Space(5), Header("Chorus")] private MyButton enableButton;
    [SerializeField] private Knob depthKnob;

    [Range(0.0f,1.0f)] private float depth;
    private bool initFlg = false;

    //----------------------------------------------------------
    // スタート
    //
    private void Start ()
    {
        Initialize();
    }
	
	//----------------------------------------------------------
    // アップデート
    //a
	private void Update ()
    {
        // キーボード操作でノブを回す
        if (enableButton.IsCurrentKeyDown() || depthKnob.isHitting)
        {
            if      (Input.GetKey(KeyCode.LeftArrow))   { depthKnob.RotateKnob(-150.0f * Time.deltaTime); }
            else if (Input.GetKey(KeyCode.RightArrow))  { depthKnob.RotateKnob( 150.0f * Time.deltaTime); }

			depth = depthKnob.GetcurrentRotateValue();
        }
		EffectUpdate();
    }

	// エフェクト適用
	private void EffectUpdate()
	{

		// エフェクトを適用するか
		if (enableButton.IsPushed() || depthKnob.isHitting)
		{
			if (!initFlg) myAudioMixer.SetFloat("bgm_Chorus_Wet", 0.0f);

			myAudioMixer.SetFloat("bgm_Chorus_Depth", depth);
			initFlg = true;
		}
		else if (initFlg)
		{
			Initialize();
			myAudioMixer.SetFloat("bgm_Chorus_Wet", -80.0f);
		}
	}

    //----------------------------------------------------------
    // アウェイク
    // 
    private void Initialize()
    {
        depth = 0.0f;
        myAudioMixer.SetFloat("bgm_Chorus_Depth", depth);
        myAudioMixer.ClearFloat("bgm_Chorus_Wet");
        initFlg = false;
    }

    // 状態判別用
    public bool IsEnabled() { return enableButton.IsPushed(); }

    // 0.0~1.0で返す。初期値は 0.0
    public float GetEffectLevel()
    {
        if (IsEnabled() || depthKnob.isHitting)
            return depthKnob.GetcurrentRotateValue();
        else
            return 0.0f;
    }
}
