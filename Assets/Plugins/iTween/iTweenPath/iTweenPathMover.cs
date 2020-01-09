//=================================================================
//  ◆ iTweenPathMover.cs
//-----------------------------------------------------------------
//  Author:
//    K.Endo
//  Description:
//    iTween settings on Inspector.
//  Requirements:
//    iTween (Pixelplacement)
//    iTweenPath (Pixelplacement)
//  License:
//    Apache License, Version 2.0
//      - http://www.apache.org/licenses/LICENSE-2.0
//=================================================================
using UnityEngine;


	public enum iTweenType{
		None = -1,
		MoveTo,
		ScaleTo,
	}

// iTweenPathをこのスクリプトにアタッチして、コンポーネントを呼び出せるようにする
[RequireComponent(typeof(iTweenPath))]
public class iTweenPathMover : MonoBehaviour
{
    [SerializeField] private iTweenType type;
    [SerializeField] private bool playOnAwake = false;

    [Tooltip("使用するパスの名前")]
    public string pathName;
    [Tooltip("移動にかかる時間(1.5秒なら1.5)")]
    public float time;
    [Tooltip("ディレイ(1.5秒なら1.5)")]
    public float delay;
    [Tooltip("使用するイージングカーブ")]
    public iTween.EaseType easeType = iTween.EaseType.linear;
    [Tooltip("繰り返し設定")]
    public iTween.LoopType loopType;
    [Tooltip("常に正面を向くか")]
    public bool orientToPath;
    [Tooltip("流石にこれくらいはわかるだろ")]
    public bool isLocal;

    private void Start()
    {
        if (playOnAwake) Action();
    }

    public void Action()
    {

        switch (type)
        {
            case iTweenType.MoveTo:
                MoveStart();
                break;

            case iTweenType.ScaleTo:
                break;

            case iTweenType.None:
                Debug.Log("iTweenType= None");
                break;

            default:
                break;
        }
    }

    private void MoveStart()
    {
        iTween.Init(this.gameObject);
        iTween.MoveTo(this.gameObject, iTween.Hash(
                "path", iTweenPath.GetPath(pathName),
                "time", time,
                "delay", delay,
                "easeType", easeType,
                "looptype", loopType,
                "orienttopath", orientToPath,
                "isLocal", isLocal,
                "lookTime", 1
                ));
    }
}
