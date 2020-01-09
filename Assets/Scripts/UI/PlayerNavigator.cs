//=================================================================
//  ◆ PlayerNavigator.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 2018/1/22
//  Description:
//    ナビくん
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InstanceObjectInfo
{
    public string tag;

    [Space(15)]
    public string instanceText;
    public GameObject instanceObj;
    public int Bar, Beat;
}

public class PlayerNavigator : MonoBehaviour
{
    [SerializeField] private GameObject targetObj;
    [SerializeField] private GameObject textMeshObj;

    [SerializeField, Space(5)] private List<InstanceObjectInfo> instanceObjects;

    private int currentIndex = 0;

    //----------------------------------------------------------
    // インスペクタ編集時
    //
    private void OnValidate()
    {
        for(int i = 0; i < instanceObjects.Count; i++)
        {
            instanceObjects[i].tag = instanceObjects[i].Bar.ToString() +"-"+ instanceObjects[i].Beat.ToString()+": "+instanceObjects[i].instanceText;
        }
    }

    //----------------------------------------------------------
    // アップデート
    //
    private void Update()
    {
        // リセット
        if (Input.GetKeyDown("1"))
        {
            currentIndex = 0;
        }

        // リズムに合わせてノーツ生成
        if (Music.IsPlaying)
        {
            InstanceMarker();
        }
    }

    //----------------------------------------------------------
    // ノーツ生成のタイミングかチェックする
    //
    private void InstanceMarker()
    {
        // 任意のタイミングで生成
        if (Music.IsJustChangedAt(
                instanceObjects[currentIndex].Bar,
                instanceObjects[currentIndex].Beat))
        {
            GameObject gobj;

            // PlayerRootの子として生成。（ターゲットとの座標ずれ修正のため）

            // gobj = GameObject.Instantiate(instanceObjects[currentIndex].instanceObj, this.transform.position, this.transform.rotation, this.transform.parent);
            gobj = GameObject.Instantiate(textMeshObj, this.transform.position, this.transform.rotation, this.transform.parent);
            gobj.GetComponent<TextMesh>().text = instanceObjects[currentIndex].instanceText;

            // ターゲットを向く
            gobj.transform.LookAt(targetObj.transform);
            this.GetComponent<AudioSource>().Play();

            currentIndex++;
        }
    }
}