//=================================================================
//  ◆ PlayerHand.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 20171220
//  Description:
//    手の当たり判定用
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum HandType
{
    NONE = -1,
    TYPE_RIGHT,
    TYPE_LEFT,
    TYPE_DEBUG,
}

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private HandType handType;
    [SerializeField] private OVRInput.Controller controllerType;

    [SerializeField] private GameObject indexFingerObject;        // 人差し指
    [SerializeField] private GameObject thumbFingerObject;        // 親指
	[SerializeField] private GameObject handObj;

    private Finger indexFinger;
    private Finger thumbFinger;

    private string indexFingerObjectName;
    private string thumbFingerObjectName;
	private string handObjctName;

    // スティック用
    [SerializeField] private GameObject drumStick;
    private Vector3 drumStickStartPos;
    private Quaternion  drumStickStartRot;
    [SerializeField] private OVRInput.RawButton ajustButton;
    [SerializeField] private FixedJoint joint; 

    // つまみ回転用
    [SerializeField] private GameObject tempKnob;
    [SerializeField] private float anglePrev;
    [SerializeField] private float angleQuantity;


    //----------------------------------------------------------
    // アウェイク
    //
    private void Awake()
    {
        this.gameObject.tag = "Hand";
        joint = this.GetComponent<FixedJoint>();
    }

    //----------------------------------------------------------
    // スタート
    //
    private void Start ()
    {
		
		if (handType == HandType.TYPE_RIGHT)
		{
			this.GetComponentInChildren<DrumStick>().axisType = OVRInput.Axis2D.SecondaryThumbstick;
		}
		else if (handType == HandType.TYPE_LEFT)
		{
			this.GetComponentInChildren<DrumStick>().axisType = OVRInput.Axis2D.PrimaryThumbstick;
		}
		/*
		// OVRAvatarの、指オブジェクトの名前を取得
		if (handType == HandType.TYPE_RIGHT || handType == HandType.TYPE_DEBUG)
        {
            indexFingerObjectName   = "hands:b_r_index_ignore";
            thumbFingerObjectName   = "hands:b_r_thumb_ignore";
			handObjctName = "hand_right_renderPart_0";

		}
        else if (handType == HandType.TYPE_LEFT)
        { 
            indexFingerObjectName   = "hands:b_l_index_ignore";
            thumbFingerObjectName   = "hands:b_l_thumb_ignore";
			handObjctName = "hand_left_renderPart_0";
		}

        // 指オブジェクト取得
        indexFingerObject = SearchHandObject(indexFingerObjectName);
        thumbFingerObject = SearchHandObject(thumbFingerObjectName);
		handObj = SearchHandObject(handObjctName);

        // 指オブジェクト初期化
        if ((indexFingerObject != null) && (thumbFingerObject != null))
            InitFinger();
			anglePrev = this.transform.localEulerAngles.z;
			*/
	}

    //----------------------------------------------------------
    // アップデート
    //
    private void Update ()
    {	
		
		// angleQuantity = this.transform.localEulerAngles.z - anglePrev;
		// anglePrev = this.transform.localEulerAngles.z;

		/*
		if ((indexFingerObject != null) && (thumbFingerObject != null))
        {
			// RotateKnob();
		}
        else
        {
            // 指オブジェクト取得
            indexFingerObject = SearchHandObject(indexFingerObjectName);
            thumbFingerObject = SearchHandObject(thumbFingerObjectName);

			if ((indexFingerObject != null) && (thumbFingerObject != null))
                InitFinger();
        }*/

		//----------------------------------------------------------
		// リザルトの処理
		if (GameManager.GameState == GameState.Result)
        {
            // スティックを投げる。
            if (OVRInput.GetDown(ajustButton))
            {
                // スティックがジョイントされてるとき
                if (joint.connectedBody != null)
                {
                    // 次回インスタンス座標の保管
                    drumStickStartPos.Set(drumStick.transform.position.x, drumStick.transform.position.y, drumStick.transform.position.z);
                    drumStickStartRot.Set(drumStick.transform.rotation.x, drumStick.transform.rotation.y, drumStick.transform.rotation.z, drumStick.transform.rotation.w);

                    drumStick.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    drumStick.GetComponent<Rigidbody>().useGravity = true;
                    drumStick.transform.SetParent(this.transform.parent);

                    Rigidbody stickRigidBody = joint.connectedBody;
                    joint.connectedBody = null;

                    stickRigidBody.velocity = OVRInput.GetLocalControllerVelocity(controllerType);
                    stickRigidBody.angularVelocity = OVRInput.GetLocalControllerAngularVelocity(controllerType);
                    stickRigidBody.maxAngularVelocity = stickRigidBody.angularVelocity.magnitude;
                }
                // ジョイントされてないとき
                else
                {
                    Destroy(drumStick, 3.0f);
                    drumStick = Instantiate(drumStick, drumStickStartPos, drumStickStartRot, this.transform);                
                    joint.connectedBody = drumStick.GetComponent<Rigidbody>();

                    drumStick.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    drumStick.GetComponent<Rigidbody>().freezeRotation = true;
                    drumStick.GetComponent<Rigidbody>().useGravity = false;
                }    
            }     
        }
    }

    //----------------------------------------------------------
    // 指の初期化
    private void InitFinger()
    {
        float offset = 0.0f;

        // 人差し指
        if (indexFingerObject.GetComponent<Finger>() == null)
        {
            if (handType == HandType.TYPE_RIGHT) offset = 0.055f;

            indexFinger = indexFingerObject.AddComponent<Finger>();
            indexFinger.SetColliderOffset(0.025f - offset, 0.0f, 0.0f);
            indexFinger.SetColliderSize(0.05f, 0.02f, 0.02f);
        }

        // 親指
        if (thumbFingerObject.GetComponent<Finger>() == null)
        {
            if (handType == HandType.TYPE_RIGHT) offset = 0.04f;

            thumbFinger = thumbFingerObject.AddComponent<Finger>();
            thumbFinger.SetColliderOffset(0.02f - offset, 0.0f, 0.0f);
            thumbFinger.SetColliderSize(0.04f, 0.02f, 0.02f);
		}
    }
        
    //----------------------------------------------------------
    // ノブの操作
    private void RotateKnob()
    {        
		/*
        // 衝突中ノブの取得
        if (indexFinger != null && thumbFinger != null)
        {
			// 親指につまみが当たってるか調べる
			tempKnob = thumbFinger.GetHitObject("Knob");

			// ノブが見つかれば回転。親指だけ回す
			if (tempKnob != null)
			{
				tempKnob.GetComponent<Knob>().RotateKnob(angleQuantity);
			}
		}
		*/
    }
    
    // 子オブジェクト全検索
    // TODO: シングルトンでオブジェクトオペレーターにまとめる
    private GameObject SearchHandObject(string objName)
    {
        List<GameObject> list = GameObjectOperator.GetAll(this.gameObject);

        for(int i = 0; i < list.Count; i++)
        {
            if (list[i].name == objName)
            {
                return list[i];
            }
        }

        // Debug.Log("[" + objName + "] は見つかりませんでした");
        return null;
    }
}