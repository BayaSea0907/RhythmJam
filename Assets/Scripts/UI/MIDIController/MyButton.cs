//=================================================================
//  ◆ MyButton.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 2017/12/20
//  Description:
//    ボタン
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Rigidbodyのアタッチを強制する
[RequireComponent(typeof(Rigidbody))]

public class MyButton : MonoBehaviour {

    // TODO: 押した瞬間、押してる間、離した瞬間を考慮する
    //
    [SerializeField] private bool isPushed = false;
    [SerializeField] private KeyCode key;

    [SerializeField] private Material enableMaterial;
    [SerializeField] private Material disableMaterial;

    //----------------------------------------------------------
    // アップデート
    //
    private void Update()
    {
        ColorChangedCheck();
        
        // キーボード操作も可能にする。
        if (Input.GetKey(key)) isPushed = true;
        else if(Input.GetKeyUp(key)) isPushed = false;
    }

    //----------------------------------------------------------
    // 衝突判定
    //
    private void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;

        if (tag == "Hand" || tag == "Finger" || tag == "Stick")
        {
            Push();
            ColorChangedCheck();
        }
    }


    // 色変え
    private void ColorChangedCheck()
    {
        if (IsPushed()) this.GetComponent<MeshRenderer>().material = enableMaterial;
        else this.GetComponent<MeshRenderer>().material = disableMaterial;
    }


    //----------------------------------------------------------
    // 挙動制御
    //
    private void PushAnimation()
    {
    }

    //----------------------------------------------------------
    // 判定用
    public bool IsPushed() { return isPushed == true; }

    public void Push() { isPushed = !isPushed; }

    public bool IsCurrentKeyDown() { return Input.GetKey(key); }
}
