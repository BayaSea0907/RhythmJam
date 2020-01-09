//=================================================================
//  ◆ RhythmAnimation.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 20171114
//  Description:
//    リズムに合わせてオブジェクトを動かす
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class RhythmAnimation : MonoBehaviour
    {
        [SerializeField] private LoopTempo loopTempo;
        [SerializeField] private List<Timing> timingAry;
        private Animator myAnimator;

        //----------------------------------------------------------
        // スタート
        //
        private void Start()
        {
            myAnimator = GetComponent<Animator>();

            if (myAnimator != null) myAnimator.Play(0);
        }

        //----------------------------------------------------------
        // アップデート
        //
        private void Update()
        {
            //----------------------------------------------------------
            // アニメーション
            // IsJustChanged…16分音符ごとに1フレームずつtrueになる
            //
            if (myAnimator != null)
            {
                bool flg = Music.IsJustChangedBeat();
                myAnimator.SetBool("isAnim", flg);
            }
        }
    }
}