//=================================================================
//  ◆ SpotKnobLevel.cs
//-----------------------------------------------------------------
//  Author:
//     H.Kobayashi  2018/1/16
//  Description:
//    ノブのFillを制御。要らなくなる。
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpotKnobLevel : MonoBehaviour
{
    // オーディオエフェクトの種類
    [SerializeField] private AudioEffectsManager.EffectType type;

    [SerializeField] public GameObject KnobLevel_1;
    [SerializeField] public GameObject KnobLevel_2;
    [SerializeField] public GameObject KnobLevel_3;


    //----------------------------------------------------------
    // スタート
    //
	private void Start ()
    {
        if (KnobLevel_1 == null) KnobLevel_1 = transform.Find("KnobLevel_1").gameObject;
        if (KnobLevel_2 == null) KnobLevel_2 = transform.Find("KnobLevel_2").gameObject;
        if (KnobLevel_3 == null) KnobLevel_3 = transform.Find("KnobLevel_3").gameObject;

        KnobLevel_1.SetActive(false);
        KnobLevel_2.SetActive(false);
        KnobLevel_3.SetActive(false);
    }
	
    //----------------------------------------------------------
    // アップデート
    //
	private void Update ()
    {
        var level = AudioEffectsManager.GetEffectLevel(type);

        // ノブレベルUIの表示切替
        if(0.0f < level && level <= 1.0f)
        {
            if (0.66f < level) {
                KnobLevel_1.SetActive(true);
                KnobLevel_2.SetActive(true);
                KnobLevel_3.SetActive(true);
            }
            else if (0.33f < level) {
                KnobLevel_1.SetActive(true);
                KnobLevel_2.SetActive(true);
                KnobLevel_3.SetActive(false);
            }
            else {
                KnobLevel_1.SetActive(true);
                KnobLevel_2.SetActive(false);
                KnobLevel_3.SetActive(false);
            }
        }
        else
        {
            KnobLevel_1.SetActive(false);
            KnobLevel_2.SetActive(false);
            KnobLevel_3.SetActive(false);
        }
	}

    public void SetEffectType(AudioEffectsManager.EffectType _type)
    {
        type = _type;
    }
}
