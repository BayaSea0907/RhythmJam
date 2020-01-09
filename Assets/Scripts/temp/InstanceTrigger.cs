//=================================================================
//  ◆ InstanceTrigger.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 20171211
//  Description:
//    当たったらオブジェクトをインスタンスして、消滅。
//    タイミングに合わせて出す。
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceTrigger : MonoBehaviour {

    [SerializeField] private GameObject instanceObj;

    [SerializeField] private List<Timing> exitTiming;

    //----------------------------------------------------------
    // スタート
    //
    private void Start ()
    {
	}
	
	//----------------------------------------------------------
    // アップデート
    //
	private void Update ()
    {

        if (Music.IsJustChangedAt(exitTiming[0]))
        {
            GameObject.Instantiate(instanceObj);
            Destroy(this.gameObject);
        }       
    }

    //----------------------------------------------------------
    // 衝突判定
    //
    private void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;

        if (tag == "Hand" || tag == "Stick" || tag == "Finger")
        {
            GameObject.Instantiate(instanceObj);
            Destroy(this.gameObject);
        }
    }
}
