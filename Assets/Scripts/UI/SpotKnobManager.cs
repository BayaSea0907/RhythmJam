//=================================================================
//  ◆ SpotKnobManager.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi   2018/1/10
//  Description:
//    アタッチしているオブジェクトを任意の方向に回転
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AudioEffectInstanceTiming
{
    public string tag;
    public AudioEffectsManager.EffectType type;
    public int Bar;

    // TODO: エフェクトをどれだけかけるかを入れる。
    [Range(0.0f, 1.0f)]public float upperLimit;
    [Range(0.0f, 1.0f)]public float lowerLimit;

    [System.NonSerialized] public bool isInstance = false;
    [System.NonSerialized] public GameObject knobClone;

    [Space(5)] public GameObject targetKnob;
}

public class SpotKnobManager : MonoBehaviour
{
    [SerializeField, Header("Spot Effects")] private GameObject spotKnob;

	// TODO: いっそ専用のクラスを作る
	[SerializeField] private List<AudioEffectInstanceTiming> AudioEffectInstanceTiming;

	private int countdownValue = 3;	// カウント数
	private int instanceBar;		// ジャストの		~Bar前にインスタンス
	private int countdownStartBar;  // カウントダウン		~Bar前にカウントダウンを始める

    private void Awake()
    {		
		instanceBar = 3;			// 3bar前からインスタンス
		countdownStartBar = 1;		// 1bar前からカウント開始   
    }

    private void Update()
    {
        // 曲リセット
        if (Input.GetKeyDown("1")) InstanceReset();

        // エフェクトのタイミングをチェック
        foreach (var timing in AudioEffectInstanceTiming)
        {
            // まだ生成してない場合
            if (!timing.isInstance && (Music.Just.Bar < timing.Bar))
            {
                // 2Bar前からインスタンス
                if (Music.IsJustChangedAt(timing.Bar - instanceBar))
                {
                    // 選択中を示すノブのインスタンス
                    timing.knobClone = Instantiate(spotKnob, timing.targetKnob.transform.position, timing.targetKnob.transform.rotation, GameObject.Find("Player Root").transform);
					timing.knobClone.transform.localEulerAngles = new Vector3(timing.knobClone.transform.localEulerAngles.x, timing.knobClone.transform.localEulerAngles.y, 0.0f);
                    timing.knobClone.GetComponent<SpotKnobLevel>().SetEffectType(timing.type);

                    //スケールリセット
                    Vector3 knobScale = spotKnob.transform.localScale;
                    timing.knobClone.transform.localScale = Vector3.zero;

                    // スケールアニメーション
                    iTween.ScaleTo(timing.knobClone, knobScale, 1.0f);

                    timing.isInstance = true;
                }
            }

            // 1Bar前からジャスト直前までカウント
            else if ((timing.Bar - countdownStartBar) == Music.Just.Bar && (Music.Just.Beat < 3))
            {
                // TODO: カウントから、アルファフェードに。
                // テキスト更新。BarがJutの1つ前になったらカウント開始。  
                timing.knobClone.GetComponentInChildren<TextMesh>().text = (countdownValue - Music.Just.Beat).ToString();
            }

            // ジャストの処理
            else if (Music.IsJustChangedAt(timing.Bar))
            {
                // エフェクトの適用度を取得
                var value = AudioEffectsManager.GetEffectLevel(timing.type);

                // テキスト更新
                if (timing.lowerLimit < value && value < timing.upperLimit)
                {
                    timing.knobClone.GetComponentInChildren<ParticleSystem>().Play();
                    timing.knobClone.GetComponentInChildren<TextMesh>().color = Color.yellow;
                    timing.knobClone.GetComponentInChildren<TextMesh>().text = "Success!!";
                }
                else
                {
                    timing.knobClone.GetComponentInChildren<TextMesh>().color = Color.red;
                    timing.knobClone.GetComponentInChildren<TextMesh>().text = "Failed!";
                }

                Destroy(timing.knobClone.gameObject, 2.0f);
            }
        }
    }

    // 生成の初期化
    private void InstanceReset()
    {
        foreach (var timing in AudioEffectInstanceTiming)
        {
            timing.isInstance = false;
        }
    }
}
