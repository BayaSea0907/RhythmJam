//=================================================================
//  ◆ Notes.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 20171110
//  Description:
//    音ゲーで言うノーツ
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ActionType
{
    Move,
    Wait,
}

public class Notes : MonoBehaviour
{
    [SerializeField] private ActionType type;
    [SerializeField] private float speed;
    [SerializeField] private GameObject hitEffect;
    private SphereCollider myCollider;
    private float startTime;
    private bool isHit;

    // ターゲット情報
    private Vector3 targetPos;
    private Quaternion targetRotate;
    private float lastLength;
    private float targetLength;

    // marker用レイ
    private Ray ray;
    [Tooltip("マーカーを生成させる距離")]
    [SerializeField]
    private float rayDistance;

    //----------------------------------------------------------
    // アウェイク
    // 
    private void Awake()
    {
        isHit = false;
        myCollider = GetComponent<SphereCollider>();

        targetPos = Vector3.zero;
        targetRotate = Quaternion.identity;
        targetLength = 0.0f;
        lastLength = 0.0f;
        startTime = 0.0f;
    }

    // ----------------------------------------------------
    // スタート
    //
    void Start()
    {
        IsHitRay();
    }

    // ----------------------------------------------------
    // アップデート
    //
    void Update()
    {

        switch (type)
        {
            case ActionType.Move:

                // ターゲットに向かって移動
                Move();

                // プレイヤーに向けてレイを飛ばす
                if (!isHit)
                {
                    RaycastHit hit = IsHitRay();

                    if (hit.collider != null)
                    {
                        GameObject hitObj = hit.collider.gameObject;

                        // ヒットゾーンに当たった場合
                        if (hitObj.tag == "HitZone" && !isHit)
                        {
                            // マーカー生成
                            hitObj.GetComponent<HitZone>().CreateHitMarker(
                                        ray.GetPoint(rayDistance), this.gameObject);
                            isHit = true;
                        }
                    }
                }
                break;

            case ActionType.Wait:
                break;

            default:
                break;
        }
    }

    //----------------------------------------------------------
    // ターゲットに向かって移動する
    // 
    private void Move()
    {
        float nowTime = (Time.time - startTime);
        lastLength = (nowTime / targetLength);

        // 次のオブジェクトに向かって移動
        transform.position = Vector3.Lerp(this.transform.position, targetPos, lastLength);
    }

    //----------------------------------------------------------
    // ターゲット位置の設定
    //
    public void SetTarget(GameObject target)
    {
        targetPos = target.transform.position;
        targetRotate = target.transform.rotation;

        // ターゲットまでの距離
        targetLength = Vector3.Distance(GetComponentInParent<Transform>().position, targetPos);

        // ターゲットを向く
        transform.rotation = Quaternion.LookRotation(targetPos - this.transform.position, target.transform.up);

        startTime = Time.time;
    }

    //----------------------------------------------------------
    // レイによる衝突処理
    // マーカー表示用として使う
    private RaycastHit IsHitRay()
    {
        RaycastHit hit;    // 当たったオブジェクト

        ray = new Ray(transform.position, transform.forward);

        // レイを飛ばす
        Physics.Raycast(ray, out hit, rayDistance);

        // レイ表示
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.green, 0.0f);

        return hit;
    }

    //----------------------------------------------------------
    // 衝突判定
    //
    private void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;

        if (tag == "Hand" || tag == "Stick" || tag == "Finger")
        {
            GameObject obj;
            obj = Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }
}