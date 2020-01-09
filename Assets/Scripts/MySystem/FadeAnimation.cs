using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FadeType
{
    FadeIn,
    FadeOut,
    Loop,
}

public enum ImageType
{
    SpriteRendererAlpha,
    SpriteRendererColor,
}

public class FadeAnimation : MonoBehaviour
{
    // フェード情報
    FadeType fadeType;
    ImageType imageType;
    

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
