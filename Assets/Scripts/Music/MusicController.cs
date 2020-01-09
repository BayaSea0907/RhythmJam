//=================================================================
//  ◆ MusicController.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 2017/11/15
//  Description:
//    BGM操作に使います
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicController : MonoBehaviour {

    [SerializeField, Header("Audio")] private AudioMixer myAudioMixer;
    [SerializeField] private  float maxVolume = 1.0f;

    static private AudioSource audioSource;
    static private float bgmVolume;

    //----------------------------------------------------------
    // アウェイク
    // 
    private void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
        audioSource.volume = 0.0f;
        bgmVolume = maxVolume;
    }
    // ----------------------------------------------------
    // スタート
    //
    private void Start()
    {
        Music.Play(this.gameObject.name, 0);
        iTween.AudioTo(audioSource.gameObject, bgmVolume, 1.0f, 5.0f);
    }

	// ----------------------------------------------------
	// アップデート
	//
	private void Update()
	{
        // キー操作のデバッグ
        if (Input.GetKey(KeyCode.LeftShift))
        {
            // 早送り
            if (Input.GetKey(KeyCode.RightArrow)) audioSource.time++;
            // 巻き戻し
            else if (Input.GetKey(KeyCode.LeftArrow)) audioSource.time--;

            else if (Input.GetKeyDown("1")) Music.Play("MusicEngine", 0);
        }

        if(GameManager.GameState != GameState.Title && GameManager.GameState != GameState.Opening)
        {
            if (Music.IsJustChangedBar())
            {
                GameManager.CurrentMusicBarOffset += 1;
            }
        }
	}

    //----------------------------------------------------------
    // 曲変更
    //
    public static void MusicChange(AudioClip clip, int bpm)
    {
        Music.Stop();
        audioSource.clip = clip;
        Music.CurrentSection.bpm = bpm;
        Music.Play("MusicEngine", 0);
    }
}

/*
AudioClipの作成
if (Input.GetKey(KeyCode.Q))
{
    static private AudioClip originalAudioClip;
    [SerializeField] private AudioClip clonelAudioClip;
    float[] cloneAudioClipSamples;
    private int passageSamplesCount;

    // AudioClipの複製 必要なかった。
    clonelAudioClip = AudioClip.Create("CLONE",
        originalAudioClip.samples,
        originalAudioClip.channels,
        originalAudioClip.frequency,
        false, true, OnAudioRead, OnSetPosition);

    audioSource.clip = clonelAudioClip;
    cloneAudioClipSamples = new float[clonelAudioClip.samples * clonelAudioClip.channels];
    originalAudioClip.GetData(cloneAudioClipSamples, 0);
    audioSource.Play();
}


// オーディオデータの読み込み
private void OnAudioRead(float[] data)
{
    int count = 0;
    while (count < data.Length)
    {
        data[count] = cloneAudioClipSamples[passageSamplesCount % cloneAudioClipSamples.Length];
        passageSamplesCount++;
        count++;
    }
}

// オーディオ位置の設定
private void OnSetPosition(int position)
{
    passageSamplesCount = position;
}
*/