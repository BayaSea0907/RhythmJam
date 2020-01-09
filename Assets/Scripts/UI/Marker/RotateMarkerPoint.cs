using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMarkerPoint : MonoBehaviour
{

    public Vector3 maxScale;
    public Vector3 minScale;
    [SerializeField] private int startAngle;
    [SerializeField] private int endAngle;

    [SerializeField] private float rotateSpeed = 300.0f;
    [SerializeField] private float radius;  // 半径
    [SerializeField] private float degree;  // 角度


    //----------------------------------------------------------
    // 
    private void Start()
    {
        degree = startAngle;
    }

    //----------------------------------------------------------
    // アップデート
    //
    private void Update()
    {
        RotatePointUpdate();        
    }

    // マーカーの移動処理
    public void RotatePointUpdate()
    {
        Vector3 parent = this.transform.parent.position;
        float rad = degree * Mathf.Deg2Rad;

        // 座標更新
        this.transform.position = new Vector3(parent.x + Mathf.Cos(rad) * radius, parent.y + Mathf.Sin(rad) * radius, this.transform.position.z);
    }

    //----------------------------------------------------------
    // 衝突判定
    //
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Hand" || other.tag == "Finger")
        {
            degree += Vector2.Distance(other.transform.position, this.transform.position) * rotateSpeed;

            // 値の制限。start~endAngleに収める
            degree = Mathf.Min(Mathf.Max(degree, startAngle), endAngle);
        }
    }

    // 操作を終えたか
    public bool IsSuccessed()
    {
        return (int)degree == endAngle;
    }
}
