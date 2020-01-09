//=================================================================
//  ◆ Knob.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 20171220
//  Description:
//    ノブ。-135°~135°の範囲で回転させる。
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Rigidbodyのアタッチを強制する
[RequireComponent(typeof(Rigidbody))]

public class Knob : MonoBehaviour
{    
    [SerializeField, Range(-135.0f, 135.0f)] private float startAngle;
    [SerializeField, Range(-1.0f, 1.0f)] private float currentRotateValue;
    [SerializeField] private bool isReverseRotate = false;
    private float limitAngle;

	[System.NonSerialized] public bool isHitting = false;
	[SerializeField] private Material enableMaterial;
	[SerializeField] private Material disableMaterial;

	//----------------------------------------------------------
	// アウェイク
	//
	private void Awake()
    {
        this.gameObject.tag = "Knob";
        limitAngle = 135.0f;
    }

    //----------------------------------------------------------
    // スタート
    // 
    private void Start ()
    {
        this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, startAngle, this.transform.localEulerAngles.z);

        currentRotateValue = startAngle / limitAngle;
    }

    //----------------------------------------------------------
    // アップデート
    //
    private void Update()
    {
		float rotateY = Mathf.Max(Mathf.Min(currentRotateValue * limitAngle, 135.0f), -135.0f);
		// 回転
		this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, rotateY, this.transform.localEulerAngles.z);
    }

    // ノブ操作
    public void RotateKnob(float angle)
    {
		// TODO: 回転量制御を要調整。
		currentRotateValue += angle / limitAngle;
	    currentRotateValue = Mathf.Max(Mathf.Min(currentRotateValue, 1.0f), -1.0f);
    }

    // ノブひねり値の取得。0~1で返す
    public float GetcurrentRotateValue()
    {
        // 右にひねって、0→1で返す
        if (!isReverseRotate)
        {
            // -1~1を、0~1に補正
            return (currentRotateValue + 1.0f) * 0.5f;
        }
        // 右にひねって、1→0で返す
        else
        {                           
            return 1.0f - (currentRotateValue + 1.0f) * 0.5f;
        }
    }

	//------------------------------------------------------------------------
	// 衝突判定
	//
	// 衝突中
	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Stick")
		{
			this.GetComponent<MeshRenderer>().material = enableMaterial;
			this.isHitting = true;
		}
	}

	//------------------------------------------------------------------------
	// 離れたとき
	// 
	private void OnTriggerExit(Collider other)
	{
		if(other.tag == "Stick")
		{
			this.GetComponent<MeshRenderer>().material = disableMaterial;
			this.isHitting = false;
		}

	}

}
