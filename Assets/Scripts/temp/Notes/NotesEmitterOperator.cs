//=================================================================
//  ◆ NotesEmitterEditor.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 20171125
//  Description:
//    エミッターの位置調整用
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// NotCameraWithVRのエディタ編集用
/*
[CustomEditor(typeof(NotesEmitterEditor))]
public class NotesEmitterEditor_Castomizer : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox(
            "プレイヤーから一定距離の配置を楽にするためのもの。\n" +
            "使うときはisActiveをTrueにする。\n" + 
            "※ 複数の場合は親オブジェクトをPivot回転させると楽。"
            , MessageType.Info);
        base.OnInspectorGUI();
    }
}
*/
public class NotesEmitterOperator : MonoBehaviour
{
    [SerializeField] private bool isActive;
    [SerializeField] private float radius;       // r
    [SerializeField] private float vertical;     // 角度 (360°)
    [SerializeField] private float horizontal;

    //----------------------------------------------------------
    // インスペクタ編集時
    //
    private void OnValidate()
    {
        if (isActive)
        {
            transform.position = AutoPositioning();
        }
    }

    //----------------------------------------------------------
    // スタート
    //
    private void Start()
    {
        isActive = false;
    }

    //----------------------------------------------------------
    // アップデート
    private void Update()
    {
    }

    //----------------------------------------------------------
    // 位置調整
    private Vector3 AutoPositioning()
    {
        float angle1 = vertical * Mathf.Deg2Rad;   // φ 
        float angle2 = horizontal * Mathf.Deg2Rad;   // Θ

        Vector3 pos = new Vector3();

        pos.x = radius * Mathf.Sin(angle1) * Mathf.Cos(angle2);
        pos.y = radius * Mathf.Cos(angle1);
        pos.z = radius * Mathf.Sin(angle1) * Mathf.Sin(angle2);

        return pos;
    }
}