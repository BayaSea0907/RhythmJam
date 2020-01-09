//=================================================================
//  ◆ ColorBullet.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 2018/2/12
//  Description:
//    ドラムから放たれるバレット
//=================================================================
using UnityEngine;

public class ColorBullet : MonoBehaviour
{
    [SerializeField] private Gradient bulletColor;
    [SerializeField] private GameObject hitEffectParticle;

    //----------------------------------------------------------
    // スタート
    //
	private void Start ()
    {
        if(bulletColor != null)
        {
            ParticleSystem.MainModule main = this.GetComponent<ParticleSystem>().main;
            main.startColor = bulletColor;
        }
    }

    //----------------------------------------------------------
    // 衝突判定
    //
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "ParticleEffect")
        {
            ParticleSystem.MainModule main = other.gameObject.GetComponentInChildren<ParticleSystem>().main;
            main.startColor = bulletColor;

            // エフェクトにヒットした回数を記録
			AudioEffectsManager.EffectHitCount++;

            // ヒットエフェクト生成
            GameObject tempParticle = Instantiate(hitEffectParticle);
            tempParticle.transform.localPosition = other.ClosestPoint(this.transform.localPosition);
            
            // エフェクト再生
            tempParticle.GetComponent<ParticleSystem>().Play();

            // 一定時間で破棄
            Destroy(tempParticle, 1.0f);
        }

    }
}
