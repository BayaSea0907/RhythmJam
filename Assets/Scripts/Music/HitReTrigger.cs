using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitReTrigger : MonoBehaviour {

    [SerializeField] private AudioClip clip;
    private AudioSource myAudioSource;
	// Use this for initialization
	void Start () {
        myAudioSource = this.GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            // 再生
            if (Input.GetKey(KeyCode.F9))
            {
                if (Music.IsJustChangedBeat()) myAudioSource.PlayOneShot(clip);
            }
            else if (Input.GetKey(KeyCode.F10))
            {
                if (Music.IsJustChangedHalfBeat() || Music.IsJustChangedBeat()) myAudioSource.PlayOneShot(clip);
            }
            else if (Input.GetKey(KeyCode.F11))
            {
                if (Music.IsJustChangedUnit()) myAudioSource.PlayOneShot(clip);
            }
            else if (Input.GetKey(KeyCode.F12))
            {
                if (Music.IsJustChangedUnit() || Music.IsJustChangedHalfUnit()) myAudioSource.PlayOneShot(clip);
            }
        }
    }
}
