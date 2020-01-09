//=================================================================
//  ◆ RotateObj.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi   2018/2/20
//  Description:
//    アタッチしているオブジェクトを任意の方向に回転
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObj : MonoBehaviour
{
    [SerializeField] private bool isLooping = false;
    [SerializeField] private bool isLocal = false;
    [SerializeField] private float speed = 10.0f;
    // 回転軸
    [SerializeField] private Axis axis;
    private Vector3 m_axis
    {
        get
        {
            switch (axis)
            {
                case Axis.X:
                    return Vector3.right;
                case Axis.Y:
                    return Vector3.up;
                case Axis.Z:
                    return Vector3.forward;
                default:
                    break;
            }
            return Vector3.zero;
        }
    }
    [SerializeField] private float endAngle;
    private float currentAngle;

    //----------------------------------------------------------
    // スタート
    //
    private void Start()
    {       
        switch (axis)
        {
            case Axis.X:
                endAngle = this.transform.localEulerAngles.x + endAngle;
                break;
            case Axis.Y:
                endAngle = this.transform.localEulerAngles.y + endAngle;
                break;
            case Axis.Z:
                endAngle = this.transform.localEulerAngles.z + endAngle;
                break;
            default:
                Debug.Log("Axisは、X,Y,Zのどれかを選んでください");
                break;
        }        
    }

    //----------------------------------------------------------
    // アップデート
    //
    private void Update()
	{
        // isLoopなら問答無用で回転
        if (isLooping)
        {
            Rotate();
        }
        // 目標角度まで行ったら止める
        else if (endAngle >= GetCurrentLocalEulerAngle())
        {
            Rotate();
        }
    }

    //----------------------------------------------------------
    // 回転処理
    //
    private void Rotate()
    {
        // ローカル軸で回転
        if (isLocal)
        {
            transform.Rotate(m_axis, speed * Time.deltaTime);
        }
        // ワールド軸で回転
        else
        {
            transform.RotateAround(this.transform.position, m_axis, speed * Time.deltaTime);
        }
    }

    //----------------------------------------------------------
    // 対照軸のアングルを取得
    //
    private float GetCurrentLocalEulerAngle()
    {
        switch (axis)
        {
            case Axis.X:
                return this.transform.localEulerAngles.x;
            case Axis.Y:
                return this.transform.localEulerAngles.y;
            case Axis.Z:
                return this.transform.localEulerAngles.z;
            default:
                Debug.Log("Axisは、X,Y,Zのどれかを選んでください");
                break;
        }

        return 0;
    }
}
