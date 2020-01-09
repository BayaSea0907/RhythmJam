//=================================================================
//  ◆ Drum.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 2018/1/27
//  Description:
//    ドラムです。
//=================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]

public class DrumStick : MonoBehaviour
{
	private Knob hitKnob = null;
	private float koboRotateSpeed = 200.0f;
	public OVRInput.Axis2D axisType;

	//--------------------------------------------------------------------
	// アップデート
	//
	private void Update()
	{
		// ノブがある　&& 使用されてないとき
		if(hitKnob != null && hitKnob.isHitting == true)
		{
			// hitKnob.RotateKnob(OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.Active).x * koboRotateSpeed * Time.deltaTime);
			hitKnob.RotateKnob(OVRInput.Get(axisType, OVRInput.Controller.Active).x * koboRotateSpeed * Time.deltaTime);
		}

	}
	//--------------------------------------------------------------------
	// 衝突判定	
	// 
	// 触れたとき
	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "BGM")
		{
			other.GetComponent<MusicScrollContent>().scrollcontentRoot.ReSelectAll(); ;
			other.GetComponent<MusicScrollContent>().Select();
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "Knob")
		{
			hitKnob = other.GetComponent<Knob>();
			hitKnob.isHitting = true;
		}
	}

	//--------------------------------------------------------------------
	// 離れたとき
	//
	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Knob" && hitKnob != null)
		{
			hitKnob.isHitting = false;
			hitKnob = null;
		}
	}
}
