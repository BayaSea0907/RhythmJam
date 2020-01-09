//=================================================================
//  ◆ JetSmoke.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 2018/1/9
//  Description:
//    エフェクトです。スプラッシュです。
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetSmoke : MonoBehaviour {

    [SerializeField] private List<Timing> instanceTiming;
    [SerializeField] private List<GameObject> childList;
    [SerializeField] private int timingIdx;
    private int barPrev;

    //----------------------------------------------------------
    // スタート
    //
    private void Start ()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            childList.Add(this.transform.GetChild(i).gameObject);
        }
        timingIdx = 0;
        barPrev = 0;
    }

    //----------------------------------------------------------
    // アップデート
    //
    private void Update ()
    {     
        if (timingIdx < instanceTiming.Count)
        {
            if (Music.IsJustChangedAt(instanceTiming[timingIdx]))
            {
                foreach (var child in childList)
                {
                    child.GetComponent<ParticleSystem>().Play();
                }
                timingIdx++;
            }
            else if (barPrev > Music.Just.Bar)
            {
                timingIdx--;
                if (timingIdx < 0) timingIdx = 0;
            }
        }
        else
        {
            timingIdx = 0;
        }

        barPrev = Music.Just.Bar;
    }
}
