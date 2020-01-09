 //=================================================================
//  ◆ AudioEffectsManager.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi 2018/1/9
//  Description:
//    AudioEffectの値を引っ張ってくるインターフェース。0.0~1.0で返す。
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioEffectsManager : MonoBehaviour
{
    public enum EffectType
    {
        NONE = -1,
        LPF,HPF,PITCH,FLANGE,CHORUS,REVERB,
    }

    // シングルトン 結局使ってない。
    // AudioEffectsManager.Instance.でどこからでもアクセスできる。
    #region instance...singleton
    
    /*
    static private AudioEffectsManager m_instance;
    private AudioEffectsManager() { Debug.Log("AudioEffectsManager 生成...singleton"); }

    // 外部からはこの変数にアクセスする
    static public AudioEffectsManager Instance
    {
        get
        {
            if (m_instance == null) m_instance = new AudioEffectsManager();
            return m_instance;
        }
    }
    */
    #endregion

    [SerializeField] private GameObject bgmMixerObj;
    private AudioMixer bgmMixer;

    // 波形データ
    static public int sampleSize;

	static private int _maxEffectCount = 30 + 120 + 3;	// Stage1 + Stage2 + other Effects
	static public int MaxEffectCount {  get{ return _maxEffectCount; } }
	static public int EffectHitCount = 0;      
    // ノブ各種
    #region Knobs
    static private LPFKnob lpfKnob;
    static private HPFKnob hpfKnob;
    static private PitchKnob pitchKnob;
    static private FlangeKnob flangeKnob;
    static private ChorusKnob chorusKnob;
    static private ReverbKnob reverbKnob;
    #endregion

    // エフェクト使用しているかのチェック用
    static public bool LPF_Enabled      { get { return lpfKnob.IsEnabled(); } }
    static public bool HPF_Enabled      { get { return hpfKnob.IsEnabled(); } }
    static public bool Pitch_Enabled    { get { return pitchKnob.IsEnabled(); } }
    static public bool Flange_Enabled   { get { return flangeKnob.IsEnabled(); } }
    static public bool Chorus_Enabled   { get { return chorusKnob.IsEnabled(); } }
    static public bool Reverb_Enabled   { get { return reverbKnob.IsEnabled(); } }

    // エフェクト適用度取得用
    static public float LPF_Level       { get { return lpfKnob.GetEffectLevel(); } }
    static public float HPF_Level       { get { return hpfKnob.GetEffectLevel(); } }
    static public float Pitch_Level     { get { return pitchKnob.GetEffectLevel(); } }
    static public float Flange_Level    { get { return flangeKnob.GetEffectLevel(); } }
    static public float Chorus_Level    { get { return chorusKnob.GetEffectLevel(); } }
    static public float Reverb_Level    { get { return reverbKnob.GetEffectLevel(); } }

    // エフェクト適用度を取得する
    static public float GetEffectLevel(EffectType type)
    {
        switch (type)
        {
            case EffectType.NONE:   return 0.0f;
            case EffectType.LPF:    return LPF_Level;
            case EffectType.HPF:    return HPF_Level;
            case EffectType.PITCH:  return Pitch_Level;
            case EffectType.FLANGE: return Flange_Level;
            case EffectType.CHORUS: return Chorus_Level;
            case EffectType.REVERB: return Reverb_Level;

            default:
                Debug.Log("Type= NONE for AudioEffectManager");
                return 0.0f;
        }
    }

    // エフェクトを一定時間適用する
    static public void AutoEffectPlay(EffectType type, float lifTime)
    {
        switch (type)
        {
            case EffectType.LPF:    // lpfKnob.AutoEffectPlay(lifTime); break;
            case EffectType.HPF:    // hpfKnob.AutoEffectPlay(); break;
            case EffectType.PITCH:  // pitchKnob.AutoEffectPlay(); break;
            case EffectType.FLANGE: // flangeKnob.AutoEffectPlay(); break;
            case EffectType.CHORUS: // chorusKnob.AutoEffectPlay(); break;
            case EffectType.REVERB: // reverbKnob.AutoEffectPlay(); break;

            case EffectType.NONE:
                break;
            default:
                Debug.Log("Type= NONE for AudioEffectManager");
                return;
        }
    }

    //----------------------------------------------------------
    // スタート
    //
    private void Start ()
    {
        Initialize();
    }

    //----------------------------------------------------------
    // アップデート
    //
    private void Update()
    {
    }


    //----------------------------------------------------------
    // アウェイク
    //
    private void Initialize()
    {
        if (bgmMixer == null)
        {
            bgmMixer = this.GetComponent<AudioSource>().outputAudioMixerGroup.audioMixer;
        }

        if (bgmMixerObj != null)
        {
            lpfKnob     = bgmMixerObj.GetComponentInChildren<LPFKnob>();
            hpfKnob     = bgmMixerObj.GetComponentInChildren<HPFKnob>();
            pitchKnob   = bgmMixerObj.GetComponentInChildren<PitchKnob>();
            flangeKnob  = bgmMixerObj.GetComponentInChildren<FlangeKnob>();
            chorusKnob  = bgmMixerObj.GetComponentInChildren<ChorusKnob>();
            reverbKnob  = bgmMixerObj.GetComponentInChildren<ReverbKnob>();
        }
    }
}
