using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEffectMarker : MonoBehaviour, IMarker
{
    AudioEffectsManager.EffectType audioEffectType;
    [SerializeField] private float effectLifeTime;
    [SerializeField] private Material enableMaterial;
    [SerializeField] private Material disableMaterial;
    // 初期化
    public void MarkerInitialize() { }

    // 更新
    public void MarkerUpdate() { }

    // 衝突時
    public void MarkerHitEnter()
    {
        if (!IsPushed())
        {
            AudioEffectsManager.AutoEffectPlay(audioEffectType, effectLifeTime);
            this.GetComponent<MeshRenderer>().material = enableMaterial;
        }
        else
        {
            this.GetComponent<MeshRenderer>().material = disableMaterial;
        }        
    }

    // 衝突中
    public void MarkerHitStay() { }

    // 操作成功時
    public void MarkerSuccess() { }

    // 自身の破棄
    public void MarkerDestroy() { }

    
    
    private bool IsPushed()
    {
        return false;
    }
}
