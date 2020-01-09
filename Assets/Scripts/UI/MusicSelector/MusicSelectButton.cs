//=================================================================
//  ◆ MyButton.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 2018/1/8
//  Description:
//    押された時にUIをスケールしながら表示
//=================================================================
using System.Collections;
using UnityEngine;

public class MusicSelectButton : MonoBehaviour
{
    [SerializeField] private MyButton button;
    [SerializeField] private GameObject musicSelectorObj;

    [SerializeField] private Vector3 maxScale;
    [SerializeField] private Vector3 minScale;
    public bool isAnimationRunning = false;
    private bool animEndFlg;

    //----------------------------------------------------------
    // スタート
    //
    private void Start()
    {
        musicSelectorObj.SetActive(false);        
        musicSelectorObj.transform.SetParent(this.transform);
        this.transform.localScale = minScale;

        // 更新処理の起動
        StartCoroutine("ContentSelectingUpdate");
    }


    private IEnumerator ContentSelectingUpdate()
    {
        while (true)
        {
            // 入力があるまで待機
            while (true)
            {
                if (!button.IsPushed()) yield return 0;
                else break;
            }

            // スケールを大きく
            musicSelectorObj.SetActive(true);
            iTween.ScaleTo(this.gameObject, maxScale, 1.0f);
            isAnimationRunning = true;

            yield return new WaitForSeconds(1.0f);
            isAnimationRunning = false;

            // 入力があるまで待機
            while (true)
            {
                if (button.IsPushed()) yield return 0;
                else break;
            }

            // スケールを小さく
            iTween.ScaleTo(this.gameObject, minScale, 0.5f);

            yield return new WaitForSeconds(0.5f);
        }
    }
}
