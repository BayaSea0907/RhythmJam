using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RotateMarker : MonoBehaviour, IMarker
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;
    [SerializeField] private AudioClip hitingSound;
    [SerializeField] private AudioClip successSound;

    [SerializeField] private GameObject pointObj;

    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private GameObject outRing;
    [SerializeField] private float outRingScaleTime;

    [SerializeField] private GameObject resultObj;

    private RotateMarkerPoint point;

    // 初期化
    public void MarkerInitialize()
    {
        pointObj.SetActive(true);
        point = pointObj.GetComponentInChildren<RotateMarkerPoint>();
    }
    // 更新処理
    public void MarkerUpdate()
    {
        this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    // 衝突した瞬間
    public void MarkerHitEnter()
    {
        iTween.ScaleTo(pointObj, point.maxScale, 0.2f);
    }

    // 衝突中
    public void MarkerHitStay()
    {
        if (!point.IsSuccessed())
        {
            // リズムに合わせて音を鳴らす
            if (Music.IsJustChangedUnit() && !GetComponent<AudioSource>().isPlaying)
                GetComponent<AudioSource>().PlayOneShot(hitingSound);
        }
        else if(!particleSystem.isPlaying)
        {
            MarkerSuccess();
        }
    }

    // 操作成功時
    public void MarkerSuccess()
    {
        // リザルト表示
        // TODO: Badも入れる
        resultObj.SetActive(true);
        iTween.ScaleTo(resultObj, Vector3.one, 0.2f);

        particleSystem.Play();
        iTween.ScaleTo(pointObj, point.minScale, 0.2f);
        GetComponent<AudioSource>().PlayOneShot(successSound);
        Destroy(this.gameObject, 1.0f);
    }

    // 自身の破棄
    public void MarkerDestroy()
    {
        // アウトリングのアニメーション
        outRing.SetActive(true);
        iTween.ScaleTo(outRing, iTween.Hash("scale", Vector3.one, "time", outRingScaleTime, "easetype", iTween.EaseType.linear,
                                            "oncompletetarget", this.gameObject));

        speed *= 0.01f;
        Destroy(this.gameObject, lifeTime);
    }
}
