using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectMove : MonoBehaviour
{
    public float movetime;
    

    void Start ()
    {
        if (this.gameObject.GetComponent<iTweenPath>().enabled)
        {
            iTweenPath itween = this.gameObject.GetComponent<iTweenPath>();

            iTween.MoveTo(this.gameObject, iTween.Hash("path", iTweenPath.GetPath(itween.pathName),
                                                        "time", movetime,
                                                        "easetype", iTween.EaseType.linear,
                                                        "looptype", iTween.LoopType.loop));
        }
    }

    void Update ()
    {
        var objscale = Mathf.Max(Music.MusicalCos() , 0.2f);
        this.transform.localScale = new Vector3(objscale, objscale, objscale);
	}
}
