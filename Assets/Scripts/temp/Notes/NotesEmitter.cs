//=================================================================
//  ◆ NotesEmitter.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 20171110
//  Description:
//    ノーツの生成者
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ノーツ生成のタイミング
public enum LoopTempo
{
    None = -1,
    Unit,       // 1/16
    Bar,        // 1/4
    Beat,       // 1/1
    HalfBeat,   // beatの半分
    Free,
}

public class NotesEmitter : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject notes;
    [SerializeField] private LoopTempo loopTempo;
    [SerializeField] private List<Timing> timingAry;

    // ---------------------------------------------------------
    // インスペクタ編集時に更新される処理
    private void OnValidate()
    {
    }

    //----------------------------------------------------------
    // スタート
    //
    private void Start()
    {
    }

    //----------------------------------------------------------
    // アップデート
    //
    private void Update()
    {
        // リズムに合わせてノーツ生成
        if (Music.IsPlaying) StartCoroutine("EmitNotes");
    }

    //----------------------------------------------------------
    // ノーツ生成のタイミングかチェックする
    //
    private IEnumerator EmitNotes()
    {
        switch (loopTempo)
        {
            case LoopTempo.Bar:
                if (Music.IsJustChangedBar()) InstanceNotes();
                break;

            case LoopTempo.Beat:
                if (Music.IsJustChangedBeat()) InstanceNotes();
                break;

            case LoopTempo.HalfBeat:
                if (Music.IsJustChangedHalfBeat()) InstanceNotes();
                break;

            case LoopTempo.Free:
                foreach (Timing timing in timingAry)
                {
                    // 任意のタイミングで生成
                    if (Music.IsJustChangedAt(timing))
                    {
                        if (Music.IsJustChangedHalfBeat())
                        {
                            InstanceNotes();

                            // 処理が終わるまで次の処理に移行しない
                            yield return new WaitForEndOfFrame();
                        }
                    }

                    // SeekToSectionの繰り返しを考慮してコンテナは消さない
                    // timingAry.Remove(timing);
                    break;
                }
                break;

            case LoopTempo.None:
                break;

            default:
                break;
        }

        yield return false;
    }

    // ノーツ生成
    private void InstanceNotes()
    {
        GameObject gobj;

        gobj = GameObject.Instantiate(notes);
        gobj.transform.position = this.transform.position;
        gobj.transform.SetParent(this.transform);
        gobj.GetComponent<Notes>().SetTarget(target);
    }
}