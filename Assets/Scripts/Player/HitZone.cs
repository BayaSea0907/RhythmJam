//=================================================================
//  ◆ HitZone.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 20171111
//  Description:
//    ノーツの消失点
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HitZone : MonoBehaviour
{

    [SerializeField] private GameObject hitMarker;

    //----------------------------------------------------------
    // ヒットマーカー生成
    //
    public void CreateHitMarker(Vector3 cursorPos, GameObject target)
    {
        GameObject gobj;

        gobj = GameObject.Instantiate(hitMarker);
        gobj.transform.position = cursorPos;
        gobj.transform.SetParent(this.transform);

        gobj.transform.rotation = Quaternion.LookRotation(
            (cursorPos - this.transform.position),target.transform.up);
    }
}
