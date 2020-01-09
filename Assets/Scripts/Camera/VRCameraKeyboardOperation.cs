//=================================================================
//  ◆ VRCameraKeyboardOperation.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 20171127
//  Description:
//    VRデバイスがないとき、疑似VRとして使う
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VRCameraKeyboardOperation : MonoBehaviour
{
	// カメラ回転用
	[SerializeField] private float debugCameraRollSpeed;
	private Vector3 lastAngle;

	//----------------------------------------------------------
	// スタート
	//
	private void Start()
	{
		lastAngle = Vector3.zero;
	}
	
	//----------------------------------------------------------
	// アップデート
	//
	private void Update()
	{
		this.transform.localEulerAngles = lastAngle;

		// Shift+Keyでカメラ回転。
		if (Input.GetKey(KeyCode.LeftShift))
		{
			// x, yが逆なのは、y軸が縦回転のため。
			if (Input.GetKey("w")) { lastAngle.x += -debugCameraRollSpeed * Time.deltaTime; }
			if (Input.GetKey("a")) { lastAngle.y += -debugCameraRollSpeed * Time.deltaTime;	}
			if (Input.GetKey("s")) { lastAngle.x +=  debugCameraRollSpeed * Time.deltaTime; }
			if (Input.GetKey("d")) { lastAngle.y +=  debugCameraRollSpeed * Time.deltaTime; }
		}
	}
}
