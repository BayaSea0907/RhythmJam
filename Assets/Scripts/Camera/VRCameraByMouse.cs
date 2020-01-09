//=================================================================
//  ◆ VRCameraByMouse.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 2017/11/14
//  Description:
//    VRデバイスを使わないときのカメラ
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class VRCameraByMouse : MonoBehaviour
{
    [SerializeField] private bool useGyro;
    [SerializeField] private bool useDebugMove;

    [SerializeField] private float debugMoveSpeed;
    [SerializeField] private float debugScrollSpeed;
    [SerializeField] private float debugCameraRollSpeed;

    // カメラ回転用
    private Vector3 lastAngle;
    private Vector3 lastMousePosition;

    // ジャイロ用
    private Quaternion correction;
    private Quaternion targetCorrection;
    private double lastCompassUpdateTime;

    //----------------------------------------------------------
    // アウェイク
    //
    private void Awake()
    {
        lastMousePosition = Vector3.zero;

        correction = Quaternion.identity;
        targetCorrection = Quaternion.identity;
        lastCompassUpdateTime = 0.0;
    }

    //----------------------------------------------------------
    // スタート
    //
    private void Start()
    {
        // ジャイロセンサーの有効化
        if (useGyro)
        {
            Input.gyro.enabled = true;
            Input.compass.enabled = true;
		}
    }

    //----------------------------------------------------------
    // アップデート
    //
    private void Update()
    {
        // デバック操作が有効の場合
        if (useDebugMove) DebugCameraOperation();

        // ジャイロが有効の場合
        if (useGyro) GyroOperation();
    }


    //----------------------------------------------------------
    // カメラのデバック操作:  ジンバルロックを考慮
    //
    private void DebugCameraOperation()
    {
        this.transform.localEulerAngles = lastAngle;

        //**********************************************************
        // 左または右クリック状態でのカメラ回転
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            // x, yが逆なのは、y軸が縦回転のため。
            lastAngle.y += 5.0f * (Input.mousePosition.x - lastMousePosition.x) * Time.deltaTime;
            lastAngle.x -= 5.0f * (Input.mousePosition.y - lastMousePosition.y) * Time.deltaTime;
            lastMousePosition = Input.mousePosition;
        }

        //**********************************************************
        // 奥行移動
        if (Input.mouseScrollDelta.y > 0.0f) this.transform.Translate(Vector3.forward * debugScrollSpeed * Time.deltaTime);
        else if (Input.mouseScrollDelta.y < 0.0f) this.transform.Translate(Vector3.back * debugScrollSpeed * Time.deltaTime);


        //**********************************************************
        // マウススクロール押し込み状態も、上下左右の移動可能
        if (Input.GetMouseButton(2))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            if (mouseX > 0.1f) this.transform.Translate(Vector3.left * mouseX * debugMoveSpeed * Time.deltaTime);
            else if (mouseX < -0.1f) this.transform.Translate(Vector3.right * -mouseX * debugMoveSpeed * Time.deltaTime);

            if (mouseY > 0.1f) this.transform.Translate(Vector3.down * mouseY * debugMoveSpeed * Time.deltaTime);
            else if (mouseY < -0.1f) this.transform.Translate(Vector3.up * -mouseY * debugMoveSpeed * Time.deltaTime);
        }
    }

    //----------------------------------------------------------
    // ジャイロカメラ処理
    // 
    private void GyroOperation()
    {
        Quaternion gorientation = changeAxis(Input.gyro.attitude);

        // 前フレームの向きとの変化を比較
        if (Input.compass.timestamp > lastCompassUpdateTime)
        {
            lastCompassUpdateTime = Input.compass.timestamp;

            // ジャイロの値を取得
            Vector3 gravity = Input.gyro.gravity.normalized;
            Vector3 rawvector = compassRawVector;
            Vector3 flatnorth = rawvector - Vector3.Dot(gravity, rawvector) * gravity;

            Quaternion corientation = changeAxis(
                Quaternion.Inverse(Quaternion.LookRotation(flatnorth, -gravity)));

            // +zを北にするためQuaternion.Euler(0,0,180)を入れる。
            Quaternion tcorrection = corientation *
                Quaternion.Inverse(gorientation) *
                Quaternion.Euler(0, 0, 180);

            // 計算結果が異常値になったらエラー
            // そうでない場合のみtargetCorrectionを更新する。
            if (!isNaN(tcorrection))
                targetCorrection = tcorrection;
        }

        if (Quaternion.Angle(correction, targetCorrection) < 45)
        {
            correction = Quaternion.Slerp(correction, targetCorrection, 0.02f);
        }
        else
            correction = targetCorrection;

        transform.localRotation = correction * gorientation;
    }

    //----------------------------------------------------------
    // Androidの場合はScreen.orientationに応じてrawVectorの軸を変換
    //
    private Vector3 compassRawVector
    {
        get
        {
            Vector3 ret = Input.compass.rawVector;

            if (Application.platform == RuntimePlatform.Android)
            {
                switch (Screen.orientation)
                {
                    case ScreenOrientation.LandscapeLeft:
                        ret = new Vector3(-ret.y, ret.x, ret.z);
                        break;
                    case ScreenOrientation.LandscapeRight:
                        ret = new Vector3(ret.y, -ret.x, ret.z);
                        break;
                    case ScreenOrientation.PortraitUpsideDown:
                        ret = new Vector3(-ret.x, -ret.y, ret.z);
                        break;
                }
            }

            return ret;
        }
    }

    //----------------------------------------------------------
    // Quaternionの各要素がNaNもしくはInfinityかどうかチェック
    //
    private bool isNaN(Quaternion q)
    {
        bool ret =
            float.IsNaN(q.x) || float.IsNaN(q.y) ||
            float.IsNaN(q.z) || float.IsNaN(q.w) ||
            float.IsInfinity(q.x) || float.IsInfinity(q.y) ||
            float.IsInfinity(q.z) || float.IsInfinity(q.w);

        return ret;
    }

    //----------------------------------------------------------
    // 軸調整用
    //
    private Quaternion changeAxis(Quaternion q)
    {
        return new Quaternion(-q.x, -q.y, q.z, q.w);
    }
}