//=================================================================
//  ◆ MoveObj.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi 2018/1/12
//  Description:
//    アタッチしているオブジェクトを任意の方向に移動
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MoveType
{
    Forward, Back, Up, Down,
    WaveX,
    WaveY,
    WaveZ
}

public class MoveObj : MonoBehaviour
{
    [SerializeField] private MoveType type;
    [SerializeField] private float speed;
    [SerializeField] private bool isLocal;

	private void Update ()
    {
        Vector3 pos;
        if (isLocal) pos = this.transform.localPosition;
        else         pos = this.transform.position;

        float moveQuantity = Music.MusicalCos(16.0f, 0.0f, -speed, speed);
        switch (type)
        {
            /*
            case MoveType.Forward:
                this.transform.Translate(Vector3.forward);
                break;

            case MoveType.Back:
                this.transform.Translate(Vector3.back * speed * Time.deltaTime);
                break;

            case MoveType.Up:
                this.transform.Translate(Vector3.up * speed * Time.deltaTime);
                break;

            case MoveType.Down:
                this.transform.Translate(Vector3.down * speed * Time.deltaTime);
                break;
                */

            case MoveType.WaveX:
                pos.x += moveQuantity * Time.deltaTime;
                break;

            case MoveType.WaveY:
                pos.y += moveQuantity * Time.deltaTime;
                break;

            case MoveType.WaveZ:
                pos.z += moveQuantity * Time.deltaTime;
                break;

            default:
                break;
        }

        transform.position = new Vector3(pos.x, pos.y, pos.z);
    }
}
