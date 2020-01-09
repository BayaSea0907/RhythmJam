//=================================================================
//  ◆ DrumManager.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 2018/1/27
//  Description:
//    ドラムの音を保管します
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HitAudio
{
    public AudioSource audio;
    public int Bar;
    public int Beat;
    public int Unit;
}


[RequireComponent(typeof(AudioSource))]
public class DrumManager : MonoBehaviour
{
    [SerializeField] bool isRecording;
    [SerializeField] private int startBar;
    [SerializeField] private int monitorBar;

    [SerializeField] private List<HitAudio> hitAudioList = new List<HitAudio>();

    static public bool isRecding = false;

    //----------------------------------------------------------
    // アップデート
    //
    private void Update()
    {        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRecding = Input.GetKey(KeyCode.R);
 
            // 録音した曲の再生
            if (Input.GetKeyDown(KeyCode.P))
            {
                StartCoroutine("PlayBack");
            }
            // リストクリア
            else if (Input.GetKeyDown(KeyCode.C))
            {
                hitAudioList.Clear();
                startBar = 0;
            }
            // 判定チェック
            else if (Input.GetKeyDown(KeyCode.V))
            {
                JustRhythmCheck();
            }
        }
    }

    // 鳴らした音の記録
    public void SetDrumHitTiming(AudioSource audioSource, int Bar, int Beat, int Unit)
    {
        HitAudio hit;
        hit.audio = audioSource;
        hit.Bar = Bar;
        hit.Beat = Beat;
        hit.Unit = Unit;
        hitAudioList.Add(hit);        
    }

    // リズムに乗れてるかのチェック
    private void JustRhythmCheck()
    {
        int hitAudioSize = hitAudioList.Count;
        int justRhythmSize = 0;

        // 軸となる音があるかチェック(Unitがずれてないもの)
        foreach(var hitAudio in hitAudioList)
        {
            if(hitAudio.Bar > 0 && hitAudio.Unit % 2 == 0)
            {
                justRhythmSize++;
            }
        }

        Debug.Log("hitAudioSize= " + hitAudioSize + "justRhythmSize= " + justRhythmSize);

        // 判定チェック
        if (justRhythmSize >= (float)hitAudioSize * 0.4f)
        {
            Debug.Log("Excellent!!");
        }
        else if (justRhythmSize >= (float)hitAudioSize * 0.25f)
        {
            Debug.Log("Nice!!");
        }
        else
        {
            Debug.Log("OK!!");
        }
    }


    // 録音再生
    private IEnumerator PlayBack()
    {
        startBar = (Music.Just.Bar + 1);

        int i = 0;

        while(i < hitAudioList.Count)
        {
            monitorBar = startBar + hitAudioList[i].Bar - hitAudioList[0].Bar;

            Timing t = new Timing(monitorBar, hitAudioList[i].Beat, hitAudioList[i].Unit);

            if (Music.Just >= t || Music.IsJustChangedAt(t))            
            {
                hitAudioList[i].audio.Play();
                i++;
            }
            else
            {
                yield return new WaitForFixedUpdate();
            }
        }
        yield return false;

    }
}
