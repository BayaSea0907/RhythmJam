using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMarker : MonoBehaviour, IMarker
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip successSound;

    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private GameObject outRing;
    [SerializeField] private float outRingScaleTime;

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
        GetComponent<AudioSource>().PlayOneShot(hitSound);
        MarkerSuccess();
    }

    // 衝突中
    public void MarkerHitStay()
    {        
    }

    // 操作成功時
    public void MarkerSuccess()
    {
        // リザルト表示
        // TODO: Badも入れる
        resultObj.SetActive(true);
        iTween.ScaleTo(resultObj, Vector3.one, 0.2f);

        GetComponent<AudioSource>().PlayOneShot(successSound);
        particleSystem.Play();
        Destroy(this.gameObject, 0.8f);
    }

    // 自身の破棄
    public void MarkerDestroy()
    {
        outRing.SetActive(true);
        iTween.ScaleTo(outRing, iTween.Hash("scale", Vector3.one, "time", outRingScaleTime, "easetype", iTween.EaseType.linear,
                                            "oncompletetarget", this.gameObject));

        // TODO: 終了時に、タイミングよくヒットしていればSuccessを呼び出す？
        // iTween.ScaleTo(outRing, iTween.Hash("scale", Vector3.one, "time", outRingScaleTime, "easetype", iTween.EaseType.easeInOutQuart,
        //                                     "oncompletetarget", this.gameObject, "oncomplete", "MarkerSuccess"));


        speed *= 0.01f;
        Destroy(this.gameObject, lifeTime);
    }
}
