//=================================================================
//  ◆ Cursor.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 20171115
//  Description:
//    カーソルの操作、衝突処理用
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    //----------------------------------------------------------
    // 当たり判定用のフレーム
    //
    // Miss:    実装未定
    // Perfect: 6frame以内
    // Good:    20frame以内
    // Bad:     40frame以内
    //
    public enum HitCheckFrames
    {
        Miss = -1,
        Perfect = 6,
        Good = 20,
        Bad = 40,
    }

    public class Cursor : MonoBehaviour
    {
        // 使用する場合true
        [SerializeField] private bool useMouse;
        [SerializeField] private bool useKeyboard;

        // 入力を取るボタン
        [SerializeField] private KeyCode keycode;
        //[SerializeField] private OVRInput.Button vrButton;

        private BoxCollider myCollider;
        private int inputFrame;
        private int checkHitFrame;

        // マウスorキーボード入力の回転用
        [SerializeField] private float radius;       // r
        private float vertical;     // 角度 (360°)
        private float horizontal;

        //----------------------------------------------------------
        // アウェイク
        //
        private void Awake()
        {
            myCollider = GetComponent<BoxCollider>();
            myCollider.enabled = false;

            // 入力チェック用
            inputFrame = -1;
            checkHitFrame = 5;

            // 回転用
            vertical = 90.0f;
            horizontal = 90.0f;
        }

        //----------------------------------------------------------
        // スタート
        //
        private void Start()
        {            
        }

        //----------------------------------------------------------
        // アップデート
        //
        private void Update()
        {            
            // コントローラーの入力
            // if (OVRInput.GetDown(vrButton))     inputFrame = 0;     // Oculus Touch
            if (Input.GetKeyDown(keycode))      inputFrame = 0;     // キーボード
            if (Input.GetMouseButtonDown(0))    inputFrame = 0;     // マウス

            // キーボードによる回転操作
            if (useKeyboard)
            {
                float x = 0.0f;
                float y = 0.0f;

                if (Input.GetKey(KeyCode.RightArrow))   x = -80.0f;
                if (Input.GetKey(KeyCode.LeftArrow))    x =  80.0f;
                if (Input.GetKey(KeyCode.UpArrow))      y = -80.0f;
                if (Input.GetKey(KeyCode.DownArrow))    y =  80.0f;
                StartCoroutine(RotationCursor(x,y));
            }

            // マウスによる回転操作
            if (useMouse)
            {
                float x = Input.GetAxis("Mouse X") * -150.0f;
                float y = Input.GetAxis("Mouse Y") * -150.0f;
                StartCoroutine(RotationCursor(x, y));
            }

            InputFrameCheck();
        }

        //----------------------------------------------------------
        // 入力フレームの更新
        //
        private void InputFrameCheck()
        {
            // 入力フレーム
            if ((inputFrame < checkHitFrame) && (inputFrame != -1))
            {
                myCollider.enabled = true;
                inputFrame++;
            }
            else
            {
                myCollider.enabled = false;
            }
        }

        //----------------------------------------------------------
        // カーソル操作用
        //
        private IEnumerator RotationCursor(float valueX, float valueY)
        {
            // 座標処理
            vertical += valueY * Time.deltaTime;
            horizontal += valueX * Time.deltaTime;

            float angle1 = vertical * Mathf.Deg2Rad;        // φ 
            float angle2 = horizontal * Mathf.Deg2Rad;      // Θ

            Vector3 pos = new Vector3();

            pos.x = radius * Mathf.Sin(angle1) * Mathf.Cos(angle2);
            pos.y = radius * Mathf.Cos(angle1);
            pos.z = radius * Mathf.Sin(angle1) * Mathf.Sin(angle2);

            transform.position = transform.root.position + pos;

            // 向いてる方向の処理
            transform.LookAt(this.transform.root);
            transform.Rotate(0.0f, 180.0f, 0.0f);
            
            // 処理が終わるまで次の処理に移行しない
            yield return new WaitForEndOfFrame();
        }
    }
}
