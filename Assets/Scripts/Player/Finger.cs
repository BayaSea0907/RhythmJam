//=================================================================
//  ◆ Finger.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 20171221
//  Description:
//    指
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Finger : MonoBehaviour
{
    private BoxCollider myCollider = null;

    [SerializeField] private List<GameObject> hitObjects;

    //----------------------------------------------------------
    // アウェイク
    //
    private void Awake()
    {
        this.gameObject.tag = "Finger";
        hitObjects = new List<GameObject>();

		this.GetComponent<Rigidbody>().useGravity = false;
		this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
		this.GetComponent<Rigidbody>().freezeRotation = true;
	}

    //----------------------------------------------------------
    // コライダーの設定
    //
    public void SetColliderOffset(float x, float y, float z)
    {
        if (myCollider == null)  myCollider = this.gameObject.AddComponent<BoxCollider>();

        myCollider.center = new Vector3(x, y, z);        
    }
    public void SetColliderSize(float x, float y, float z)
    {
        if (myCollider == null)  myCollider = this.gameObject.AddComponent<BoxCollider>();
        
        myCollider.size = new Vector3(x, y, z);
    }

    //----------------------------------------------------------
    // 衝突中オブジェクト取得
    public GameObject GetHitObject(string tagName)
    {
        if (hitObjects != null)
        {
            for (int i = 0; i < hitObjects.Count; i++)
            {
                if (hitObjects[i] == null)
                {
                    hitObjects.Clear();
                }
                else if (hitObjects[i].tag == tagName)
                {
                    return hitObjects[i];
                }
            }
        }
        return null;
    }

    //----------------------------------------------------------
    // 衝突判定
    //
    private void OnTriggerEnter(Collider other)
    {
        hitObjects.Clear();

        // 新しいタグなら追加
        hitObjects.Add(other.gameObject);
    }

    // 離れた瞬間
    private void OnTriggerExit(Collider other)
    {
        // 衝突中のオブジェクトを検索
        for (int i = 0; i < hitObjects.Count; i++)
        {
            if (hitObjects[i].tag == other.tag)
            {
                // タグのリセット
                hitObjects.Remove(hitObjects[i]);
                hitObjects.Clear();
                return;
            }
        }
    }
}