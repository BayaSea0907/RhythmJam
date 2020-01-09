//=================================================================
//  ◆ AnimationManager.cs
//-----------------------------------------------------------------
//  Author:
//    K.Meguro
//  Description:
//    生物のアニメーションにばらつきを持たせるための制御です。
//  ChangeLog:
//    2017-09-14　新規作成
//	  2017-09-18  再生位置、再生速度両方を変更するよう修正
//	  2017-10-13  再生位置変更の記述を変更、warningMessageに対策
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour {
    private Animator _animator;
	private Animation _anim;
	private AnimatorStateInfo _info;
    private float rand;
	// Use this for initialization
	void Start () {
		if (_animator = GetComponent<Animator>())
		{
			_info = _animator.GetCurrentAnimatorStateInfo(0);
			//再度乱数を振り、再生速度を変更
			rand = Random.Range(0.8f, 1.2f);
			_animator.speed = rand;
			//乱数を振り、再生位置を決定
			rand = Random.Range(0.0f, 1.0f);
			_animator.ForceStateNormalizedTime(rand);
		}
	}
}
