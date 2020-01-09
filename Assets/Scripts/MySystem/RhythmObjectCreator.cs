using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ObjectType
{
    Circle,
    Rect,
    Line,
    Sphere,
}

public class RhythmObjectCreator : MonoBehaviour
{
    //----------------------------------------------------------
    // 追記:      2017/11/26
    // 球体(球面のみの生成)を実装
    // TODO: パラメーターを、構造体にしてInspectorに表示する
    
	[SerializeField] private bool isCreate = false;
    [SerializeField] private bool isDeleteChild = false;

    [SerializeField] private ObjectType objType;
    [SerializeField] private GameObject coppyObject;

    [SerializeField] private int count;
	[SerializeField] private float circleRadius;
    [SerializeField] private float interval;
	[SerializeField] private float rectHight;
	[SerializeField] private float rectWidth;

    // 球体用
    [SerializeField] private bool isActive;
    [SerializeField] private float radius;       // r
    [SerializeField] private float vertical;     // 角度 (360°)
    [SerializeField] private float horizontal;

    //----------------------------------------------------------
    // インスペクター変更時
    //
    // TODO: エミッター配置時に、一定の距離で配置できるようにする。
    private void OnValidate()
	{
        // 子オブジェクトを生成
        if (isCreate)
		{
			switch(objType){
				case ObjectType.Circle:
					CreateCircle();
					break;

				case ObjectType.Rect:
                    CreateRect();
					break;

				case ObjectType.Line:
                    CreateLine();
                    break;

                case ObjectType.Sphere:
                    CreateSphere();
                    break;

                default:
					break;
			}
			isCreate = false;
		}

        // 子オブジェクトをまとめる
        if (isDeleteChild)
        {
            //----------------------------------------------------------
            //  ・余分なオブジェクトを消去。
            // ※ インスペクタ編集時はデストロイできないので、
            //   ごみオブジェクトとしてまとめる。
            //
            int size = this.transform.childCount;
            if (size > 0)
            {
                GameObject obj = new GameObject();
                obj.name = "WillDelete";

                for (int i = 0; i < size; i++)
                {
                    this.transform.GetChild(0).SetParent(obj.transform);
                }
            }
            isDeleteChild = false;
        }
	}

    // 円形
    private void CreateCircle()
	{
		GameObject obj = new GameObject();
        obj.name = "Circle";

        for (int i = 0; i < count; i++)
		{
			// 円状に生成
			float angle = i * Mathf.PI * 2 / count;
			Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * circleRadius;

            // 生成
            Instantiate(coppyObject, pos, Quaternion.identity)
                .transform.SetParent(obj.transform);
		}
        obj.transform.SetParent(this.transform);
    }

	// 矩形
	private void CreateRect()
	{
        GameObject obj = new GameObject();
        obj.name = "Rect";

        for (int i = 0; i < rectHight; i++)
        {
            for (int j = 0; j < rectWidth; j++)
            {
                Vector3 pos = new Vector3(
                    (float)j * interval, 0.0f, (float)i * interval);

                // 生成
                Instantiate(coppyObject, pos, Quaternion.identity)
                    .transform.SetParent(obj.transform);
            }
        }
        obj.transform.SetParent(this.transform);
    }

	// 直線
	private void CreateLine()
	{
        GameObject obj = new GameObject();
        obj.name = "Line";

        for (int i = 0; i < count; i++)
        {
                Vector3 pos = new Vector3(
                    (float)i * interval, 0.0f, 0.0f);

                // 生成
                Instantiate(coppyObject, pos, Quaternion.identity)
                    .transform.SetParent(obj.transform);
        }
        obj.transform.SetParent(this.transform);
    }

    // 球体
    private void CreateSphere()
    {
        GameObject obj = new GameObject();
        obj.name = "Sphere";
        
        /*
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = new Vector3(
                (float)i * interval, 0.0f, 0.0f);

            // 生成
            Instantiate(coppyObject, pos, Quaternion.identity)
                .transform.SetParent(obj.transform);
        obj.transform.SetParent(this.transform);
        */

        float angle1 = vertical * Mathf.Deg2Rad;   // φ 
        float angle2 = horizontal * Mathf.Deg2Rad;   // Θ

        Vector3 pos = new Vector3();

        pos.x = radius* Mathf.Sin(angle1) * Mathf.Cos(angle2);
            pos.y = radius* Mathf.Cos(angle1);
        pos.z = radius* Mathf.Sin(angle1) * Mathf.Sin(angle2);
    }
}