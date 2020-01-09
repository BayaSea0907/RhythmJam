//=================================================================
//  ◆ HitMarker.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 20171112
//  Description:
//    ノーツが衝突する場所の表示用
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HitMarker_old : MonoBehaviour
{

    [SerializeField] private float lifeTime;
    private float time;

    //----------------------------------------------------------
    // スタート
    private void Start ()
    {
        time = 0.0f;
    }
	
	//----------------------------------------------------------
    // アップデート
	private void Update ()
    {
        time += Time.deltaTime;

        if(time > lifeTime)
        {
            Destroy(this.gameObject);
        }
	}
}
