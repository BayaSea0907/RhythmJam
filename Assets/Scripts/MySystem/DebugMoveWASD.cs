//=================================================================
//  ◆ DebugMoveWASD.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 2018/1/18
//  Description:
//    キーボードで移動するようにするやつ
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 軸用
[System.Serializable]
public enum Axis
{
    X, Y, Z, XY, XZ, YZ, XYZ
}
// 方向用
[System.Serializable]
public class Direction
{
    public enum Dir { Forward, Back, Right, Left, Up, Down }
    public enum Axis {X, Y, Z}
    public Dir dir;
    public Vector3 direction
    {
        get
        {
            switch (dir)
            {
                case Dir.Forward:
                    return Vector3.forward; break;
                case Dir.Back:
                    return Vector3.back; break;
                case Dir.Right:
                    return Vector3.right; break;
                case Dir.Left:
                    return Vector3.left; break;
                case Dir.Up:
                    return Vector3.up; break;
                case Dir.Down:
                    return Vector3.down; break;
                default:
                    break;
            }
            return Vector3.zero;
        }
    }

}

public class DebugMoveWASD : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;

    [Header("Keykord"), SerializeField] private KeyCode keyUp;
    [SerializeField] private KeyCode keyDown;
    [SerializeField] private KeyCode keyRight;
    [SerializeField] private KeyCode keyLeft;

    [Header("Direction"), SerializeField] private Direction dirUp;
    [SerializeField] private Direction dirDown;
    [SerializeField] private Direction dirRight;
    [SerializeField] private Direction dirLeft;

	void Update () {
        // W,A,S,Dでのカメラ移動
        if (Input.GetKey(keyUp))    { this.transform.Translate(dirUp.direction    * speed * Time.deltaTime); }    // 前
        if (Input.GetKey(keyDown))  { this.transform.Translate(dirDown.direction  * speed * Time.deltaTime); }    // 後
        if (Input.GetKey(keyRight)) { this.transform.Translate(dirRight.direction * speed * Time.deltaTime); }    // 左
        if (Input.GetKey(keyLeft))  { this.transform.Translate(dirLeft.direction  * speed * Time.deltaTime); }    // 右
    }
}
