using UnityEngine;
using UnityEditor;

// VRCameraByMouseのエディタ編集用
[CustomEditor(typeof(VRCameraByMouse))]
public class VRCameraByMouseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("このカメラは実行中に使うものです。\n" +
            " 使うときはOVRCameraPlayerを「disable」にすること。", MessageType.Info);
        base.OnInspectorGUI();
    }
}
