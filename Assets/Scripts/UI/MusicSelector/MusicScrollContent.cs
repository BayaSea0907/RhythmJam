//=================================================================
//  ◆ MusicScrollContent.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 20180106
//  Description:
//    曲選択用のUI。1曲につき1個。
//=================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicScrollContent : MonoBehaviour
{
    // 曲情報
    [SerializeField, Space(5), Header("Audio")] private AudioClip audioClip;
    [SerializeField] private Text contentsName;
    [SerializeField] int bpm;

    // UI画像
    [SerializeField, Space(5), Header("StateMaterial")] private Material selectMaterial;
    [SerializeField] private Material choiceMaterial;
    [SerializeField] private MusicSelectButton musicSelectButton;
	public ScrollContentRoot scrollcontentRoot;
	private BoxCollider myCollision;
    private bool isSelect;

    //----------------------------------------------------------
    // スタート
    //
    private void Start()
    {
        // オーディオ
        if (audioClip == null) Debug.Log("UI: 曲を設定してください");
        else
        {
            // contentsName.text = audioClip.name;
        }

        // コリジョン
        myCollision = this.GetComponent<BoxCollider>();
    }

    //----------------------------------------------------------
    // アップデート
    //
    private void Update()
    {
        if (musicSelectButton.isAnimationRunning) myCollision.enabled = false;
        else myCollision.enabled = true;
    }

    //----------------------------------------------------------
    // 選ぶ
    //
    public void Choise()
    {
        this.GetComponent<Image>().material = choiceMaterial;
    }
    // 決定
    public void Select()
    {
        isSelect = true;
        this.GetComponent<Image>().material = selectMaterial;

        // 曲を設定
        MusicController.MusicChange(audioClip, bpm);
    }

    //----------------------------------------------------------    
    // 選択処理リセット
    //
    public void ResetChoise()
    {
        if(!isSelect)
            this.GetComponent<Image>().material = null;
    }
    // 決定処理リセット
    public void ResetSelect()
    {
        isSelect = false;
        this.GetComponent<Image>().material = null;
    }
    // サイズ取得
    public Vector2 GetImageScale()
    {
        return this.GetComponent<RectTransform>().sizeDelta;
    }
}
