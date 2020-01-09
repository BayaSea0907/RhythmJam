using System.Collections;
using UnityEngine;

public class rotation : MonoBehaviour
{
    [SerializeField] private AnimationCurve speedCurve;

	[SerializeField] private float xRotation = 0F;
	[SerializeField] private float yRotation = 0F;
	[SerializeField] private float zRotation = 0F;

	void OnEnable() {
		InvokeRepeating("rotate", 0f, 0.0167f);
	}
	void OnDisable() {
		CancelInvoke();
	}
	public void clickOn() {
		InvokeRepeating("rotate", 0f, 0.0167f);
	}
	public void clickOff() {
		CancelInvoke();
	}
	void rotate() {
		this.transform.localEulerAngles += new Vector3(xRotation,yRotation,zRotation);
    }

    public void RotateChange() {
        float yRandom = Random.Range(-0.5f, 0.5f);
        float zRandom = Random.Range(-0.5f, 0.5f);
        yRotation = yRandom; zRotation = zRandom;
    }

    public void RotateReverse() {
        xRotation *= -1f; yRotation *= -1f; zRotation *= -1f;

    }
}
