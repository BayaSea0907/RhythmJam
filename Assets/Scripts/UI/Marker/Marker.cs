//=================================================================
//  ◆ Marker.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 2018/1/22
//  Description:
//    ナビくんから放たれるやつ
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    private void Start()
    {
        this.transform.SendMessage("MarkerInitialize");        
    }
    
    // ----------------------------------------------------
    // アップデート
    //
    private void Update()
    {
        this.transform.SendMessage("MarkerUpdate");        
    }


    //----------------------------------------------------------
    // 衝突の瞬間
    //
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Stick")
        {
            this.transform.SendMessage("MarkerHitEnter");
        }
        else if (other.tag == "DestroyZone")
        {
            this.transform.SendMessage("MarkerDestroy");
        }
    }

    // 衝突してる間
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Stick")
        {
            this.transform.SendMessage("MarkerHitStay");
        }
    }
}