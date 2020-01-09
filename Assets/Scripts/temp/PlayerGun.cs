//=================================================================
//  ◆ PlayerGun.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 20171207
//  Description:
//    バレットの発射台用です
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour {

    [SerializeField] private GameObject Bullet;
    [SerializeField] private KeyCode shotKey;
    //[SerializeField] private OVRInput.Button vrButton;

    // ----------------------------------------------------
    // スタート
    //
    private void Start ()
    {
	}

    // ----------------------------------------------------
    // アップデート
    //
    private void Update ()
    {
        // if (Input.GetKeyDown(shotKey) || OVRInput.GetDown(vrButton))    Shot();
	}

    //----------------------------------------------------------
    // ショット
    //
    private void Shot()
    {
        GameObject obj;
        // バレット生成と、親の設定
        obj = Instantiate(Bullet);
        obj.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);
    }
}
