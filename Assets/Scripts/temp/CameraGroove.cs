//=================================================================
//  ◆ CameraGroove.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 20171115
//  Description:
//    音に合わせてカメラを操作します
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGroove : MonoBehaviour {

    [SerializeField] private GameObject targetObj;
    [SerializeField] private List<Camera> myCameras;

    //----------------------------------------------------------
    // アウェイク
    //
    private void Awake()
    {
        CameraDisabled();
        myCameras[0].gameObject.SetActive(true);
    }

    //----------------------------------------------------------
    // スタート
    //
    private void Start ()
    {
    }
	
	//----------------------------------------------------------
    // アップデート
    //
	private void Update () {
        if (Input.GetKeyDown(KeyCode.F1) || (Music.IsJustChangedBar() && Music.Just.Bar % 4 == 0))
        {
            CameraDisabled();
            myCameras[0].gameObject.SetActive(true);
			myCameras[0].gameObject.GetComponent<iTweenPathMover>().Action();
        }
        else if (Input.GetKeyDown(KeyCode.F2) || (Music.IsJustChangedBar() && Music.Just.Bar % 4 == 1))
        {
            CameraDisabled();
            myCameras[1].gameObject.SetActive(true);
			myCameras[1].gameObject.GetComponent<iTweenPathMover>().Action();
		}
        else if (Input.GetKeyDown(KeyCode.F3) || (Music.IsJustChangedBar() && Music.Just.Bar % 4 == 2))
        {
            CameraDisabled();
            myCameras[2].gameObject.SetActive(true);
			myCameras[2].gameObject.GetComponent<iTweenPathMover>().Action();
		}
        else if (Input.GetKeyDown(KeyCode.F4) || (Music.IsJustChangedBar() && Music.Just.Bar % 4 == 3))
        {
            CameraDisabled();
            myCameras[3].gameObject.SetActive(true);
			myCameras[3].gameObject.GetComponent<iTweenPathMover>().Action();
		}
        
    }

    private void CameraDisabled()
    {
        foreach (Camera camera in myCameras)
        {
            camera.gameObject.SetActive(false);
        }
    }
}
