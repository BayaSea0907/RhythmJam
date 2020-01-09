using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class KeepMarker : MonoBehaviour, IMarker
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;
    [SerializeField] private AudioClip hitingSound;
    [SerializeField] private AudioClip successSound;

    [SerializeField] private UnityEngine.UI.Image fillImage;
    [SerializeField] private float keepingTime = 1.5f;
    [SerializeField] private ParticleSystem particleSystem;

    [SerializeField] private GameObject outRing;
    [SerializeField] private float outRingScaleTime;

    // TODO: これにパーティクルもセットする？
    [SerializeField] private GameObject resultObj;

    // 初期化
    public void MarkerInitialize()
    {
        this.transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    // 更新処理
    public void MarkerUpdate()
    {
        this.transform.Translate(Vector3.back * speed * Time.deltaTime);
    }

    // 衝突した瞬間
    public void MarkerHitEnter()
    {        
    }

    // 衝突中
    public void MarkerHitStay()
    {
        if (fillImage.fillAmount < 1.0f)
        {
            if (Music.IsJustChangedUnit())
                GetComponent<AudioSource>().PlayOneShot(hitingSound);

            fillImage.fillAmount += keepingTime * Time.deltaTime;
        }
        else if (!particleSystem.isPlaying)
        {
            MarkerSuccess();
        }
    }
    private IEnumerable PlaySound()
    {
        // リズムに合わせて音を鳴らす

        yield return false;
    }

    // 操作成功時
    public void MarkerSuccess()
    {
        // リザルト表示
        // TODO: Badも入れる
        resultObj.SetActive(true);
        iTween.ScaleTo(resultObj, Vector3.one, 0.2f);

        particleSystem.Play();
        GetComponent<AudioSource>().PlayOneShot(successSound);
        Destroy(this.gameObject, 0.8f);
    }

    // 自身の破棄
    public void MarkerDestroy()
    {
        // アウトリングのアニメーション
        outRing.SetActive(true);
        iTween.ScaleTo(outRing, iTween.Hash("scale", new Vector3(1.16f, 1.16f, 1.16f), "time", outRingScaleTime, "easetype", iTween.EaseType.linear,
                                            "oncompletetarget", this.gameObject));
        speed *= 0.01f;
        Destroy(this.gameObject, lifeTime);
    }
}