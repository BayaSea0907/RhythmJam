//=================================================================
//  ◆ Bullet.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 20171207
//  Description:
//    プレイヤー用に使うバレットです
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour {

    [SerializeField] private AnimationCurve myCurve;
    private float startTime;
    private float currentTime;

    // ----------------------------------------------------
    // スタート
    //
    private void Start ()
    {
        startTime = Time.time;
        currentTime = 0.0f;
    }

    // ----------------------------------------------------
    // アップデート
    //
    private void Update ()
    {
        currentTime = Time.time - startTime;

        Move();

        // 終わりのKeyになったらデストロイ
        if (currentTime > myCurve.keys[myCurve.length - 1].time)
        {
            Destroy(this.gameObject);
        }
    }

    //----------------------------------------------------------
    // 移動処理
    //
    private void Move()
    {
        // 移動処理
        transform.Translate(Vector3.forward * myCurve.Evaluate(currentTime) * Time.deltaTime);
    }
}
