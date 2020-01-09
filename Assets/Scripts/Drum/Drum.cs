//=================================================================
//  ◆ Drum.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 2018/1/27
//  Description:
//    ドラムです。
//=================================================================
using System.Collections;
using UnityEngine;

public enum DrumType
{
    Kick = 0,
    Snare,
    HighTom,
    LowTom,
    FloorTom,
    Hihat,
    Crash1,
    Crash2,
    Ride,
    // China
}

public class Drum : MonoBehaviour
{

    // キー入力対応
    [SerializeField, Header("DrumInfo")] private DrumType drumType = 0;
    [SerializeField] private KeyCode debugKey;
    [SerializeField] private OVRInput.Button ovrButton;     // oculus

    // アニメーション
    [SerializeField, Header("Animation")] private ParticleSystem hitParticle;
    [SerializeField] private Vector3 hitMaxScale;
    private Vector3 hitDefaultScale;

    // カラーバレット
    [SerializeField, Header("Bullet")] private GameObject colorBullet;    // 生成時のみ参照
    private GameObject _colorBullet = null;             // 生成中のバレットを保管
    private int hitCount;
    [SerializeField] private int instanceHitCycleCount = 2;   // 何ヒットで生成するか
    [SerializeField] private GameObject bulletAngleCamera;
	[SerializeField] private GameObject bulletAngleVRCamera;

	//----------------------------------------------------------
	// アウェイク
	//
	private void Awake()
    {
        // デフォルトスケールの設定(アニメーション用)
        hitDefaultScale = this.transform.localScale;

        hitCount = 0;
    }

    //----------------------------------------------------------
    // スタート
    //
    private void Start()
    {
        StartCoroutine("DrumUpdate");
    }
    //----------------------------------------------------------
    // アップデート
    //
    private IEnumerator DrumUpdate()
    {
        while (true)
        {
            // キー & OculusTouch入力に対応
            if ((OVRInput.GetDown(ovrButton) ||
                    Input.GetKeyDown(debugKey)))
            {
                // +++++++++++++++++++++++++++++++++++++
                // 音の再生
                // +++++++++++++++++++++++++++++++++++++
                AudioPluginInterface.AudioPlay((int)drumType);

                // 叩いた回数を保管
                hitCount++;

                // アニメーション
                HitAnimation();

                // 回数に応じてバレットを飛ばす
                ColorBulletInstance();
            }

            yield return 0;
        }
	}

    //----------------------------------------------------------
    // 衝突判定
    //
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Stick")
        {
            // +++++++++++++++++++++++++++++++++++++
            // 再生
            // +++++++++++++++++++++++++++++++++++++
            AudioPluginInterface.AudioPlay((int)drumType);

            // アニメーション
            HitAnimation();

            // 叩いた回数を保管
            hitCount++;
            // 回数に応じて生成
            ColorBulletInstance();
        }
    }

    //----------------------------------------------------------
    // ドラムヒット時のアニメーション
    //
    private void HitAnimation()
    {
        // エフェクト処理
        hitParticle.Play();

        // スケールアニメーション

        iTween.ScaleTo(this.gameObject, iTween.Hash(
            "scale", hitMaxScale,
            "time", 0.2f,
            "oncompletetarget", this.gameObject,
            "oncomplete", "HitExitAnimation"));
    }

    // スケールを戻すアニメーション
    private void HitExitAnimation()
    {
		iTween.ScaleTo(this.gameObject, hitDefaultScale, 0.2f);
    }

    //----------------------------------------------------------
    // バレット生成
    //
    private void ColorBulletInstance()
    {
        //----------------------------------------------------------
        // カラーバレットが生成されてない場合生成
        if (_colorBullet == null)
        {
            if (hitCount % instanceHitCycleCount == instanceHitCycleCount - 1)
            {
                _colorBullet = Instantiate(colorBullet, this.transform);

            }
        }

        if (hitCount % instanceHitCycleCount == 0)
        {
            // 指定したオブジェクトの方向に飛ばす
            _colorBullet.GetComponent<Rigidbody>().useGravity = true;
            _colorBullet.GetComponent<Rigidbody>().isKinematic = false;
            _colorBullet.GetComponent<Rigidbody>().AddForce(Vector3.up * 5.0f, ForceMode.Impulse);

			// VR
			if (UnityEngine.XR.XRDevice.isPresent)
			{
				_colorBullet.GetComponent<Rigidbody>().AddForce(bulletAngleVRCamera.transform.forward * 25.0f, ForceMode.Impulse);
			}
			// Not VR
			else
			{
				_colorBullet.GetComponent<Rigidbody>().AddForce(bulletAngleCamera.transform.forward * 25.0f, ForceMode.Impulse);
			}
            Destroy(_colorBullet, 2.0f);

            _colorBullet = null;
        }
    }

}
