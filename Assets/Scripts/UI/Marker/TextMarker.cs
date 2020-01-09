using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMarker : MonoBehaviour, IMarker
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;

    // 初期化
    public void MarkerInitialize()
    {
        this.transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    // 更新
    public void MarkerUpdate()
    {
        this.transform.Translate(Vector3.back * speed * Time.deltaTime);
    }

    // 衝突時
    public void MarkerHitEnter() { }

    // 衝突中
    public void MarkerHitStay() { }

    // 操作成功時
    public void MarkerSuccess() { }

    // 自身の破棄
    public void MarkerDestroy()
    {
        speed *= 0.01f;
        Destroy(this.gameObject, lifeTime);
    }
}
