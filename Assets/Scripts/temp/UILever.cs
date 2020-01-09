//=================================================================
//  ◆ UILever.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 20171211
//  Description:
//    MIDIコントローラー用のUIのレバーです
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// エフェクトタイプ
public enum EffectType
{
    None = -1,
    Volume,
    LowPath,
    HightPath,
    Reverbe,
    Echo,
    Chorus,
    Distortion,
}

public class UILever : MonoBehaviour {

    [SerializeField] private float length;
    [SerializeField] private float currentLevel;
    private float currentLevelPrev;
    private float leverMoveQuantity;

    private float centerLength;
    private float maxLimit;
    private float minLimit;

    //----------------------------------------------------------
    // スタート
    //
    private void Start ()
    {
        currentLevel = this.transform.position.y;
        currentLevelPrev = 0.0f;
        centerLength = length / 2.0f;
        maxLimit = currentLevel + centerLength;
        minLimit = currentLevel - centerLength;
    }
	
	//----------------------------------------------------------
    // アップデート
    //
	private void Update ()
    {
        currentLevelPrev = currentLevel;
        currentLevel += this.transform.position.y - currentLevelPrev;

        // レバー移動量の計算
        leverMoveQuantity = currentLevel - currentLevelPrev;

        LeverMove();
    }

    //----------------------------------------------------------
    // 衝突判定
    //
    private void OnTriggerStay(Collider other)
    {
    }

    //----------------------------------------------------------
    // レバー位置の更新
    //
    private void LeverMove()
    {
        Vector3 pos = this.transform.position;

        // レバー座標の制御
        if ((minLimit < currentLevel) && (currentLevel < maxLimit))
        {
            // 範囲内。collisionがあれば特に処理もいらない
        }
        else if (minLimit >= currentLevel)
        {
            pos.y = minLimit;
        }
        else if (maxLimit <= currentLevel)
        {
            pos.y = maxLimit;
        }

        // レバー座標更新
        this.transform.position = pos;
    }

    //----------------------------------------------------------
    // レバーの移動量取得
    public float GetLeverMoveQuantity()
    {
        return leverMoveQuantity;
    }
}
