using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IMarker
{
    // 初期化
    void MarkerInitialize();

    // 更新
    void MarkerUpdate();

    // 衝突時
    void MarkerHitEnter();

    // 衝突中
    void MarkerHitStay();

    // 操作成功時
    void MarkerSuccess();

    // 自身の破棄
    void MarkerDestroy();   
}
