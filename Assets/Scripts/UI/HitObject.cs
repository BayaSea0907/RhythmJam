using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObject : MonoBehaviour {

	[SerializeField] public bool isHit;
    private BoxCollider myCollision;

	// Use this for initialization
	void Start () {
		myCollision = this.GetComponent<BoxCollider>();
    }

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Hand" || other.tag == "Finger"){
			isHit = true;
		}
	}
}
