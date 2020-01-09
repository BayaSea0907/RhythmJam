//=================================================================
//  ◆ HitEffect.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 20171127
//  Description:
//    ヒット時のエフェクト
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour {

	[SerializeField] private float lifeTime;

	private AudioSource myAudioSource;
	private float startTime;

	//----------------------------------------------------------
	// アウェイク
	private void Awake()
	{
		myAudioSource = this.GetComponent<AudioSource>();
		startTime = 0.0f;
	}

	//----------------------------------------------------------
	// スタート
	//
	private void Start ()
	{
		Music.QuantizePlay(myAudioSource, 1, 0.0f);
		startTime = Time.time;
	}

	//----------------------------------------------------------
	// アップデート
	private void Update ()
	{		
		if(!myAudioSource.isPlaying) 
		{
			Destroy(this.gameObject, lifeTime);
		}
	}
}
