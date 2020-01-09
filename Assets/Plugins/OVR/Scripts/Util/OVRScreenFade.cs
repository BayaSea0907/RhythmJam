/************************************************************************************

Copyright   :   Copyright 2014 Oculus VR, LLC. All Rights reserved.

Licensed under the Oculus VR Rift SDK License Version 3.3 (the "License");
you may not use the Oculus VR Rift SDK except in compliance with the License,
which is provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.

You may obtain a copy of the License at

http://www.oculus.com/licenses/LICENSE-3.3

Unless required by applicable law or agreed to in writing, the Oculus VR SDK
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

************************************************************************************/

using UnityEngine;
using System.Collections;

public class OVRScreenFade : MonoBehaviour
{
	public float fadeTime = 2.0f;
	public bool isFading = false;
	private Material fadeMaterial = null;

	void Awake()
	{
		fadeMaterial = new Material(Shader.Find("Oculus/Unlit Transparent Color"));
	}

	void OnDestroy()
	{
		if (fadeMaterial != null)
		{
			Destroy(fadeMaterial);
		}
	}


	// ----------------------------------------------
	// 追記 フェードイン
	// ----------------------------------------------
	public void FadeIn()
	{
		StartCoroutine(FadeInCoroutine());
	}
	// 本体
	IEnumerator FadeInCoroutine()
	{
		Debug.Log("fadeIn");
		float elapsedTime = 0.0f;
		fadeMaterial.color = Color.black;
		isFading = true;
		while (elapsedTime < fadeTime)
		{
			yield return new WaitForFixedUpdate();
			elapsedTime += Time.deltaTime;
			fadeMaterial.color = new Color(
				fadeMaterial.color.r,
				fadeMaterial.color.g,
				fadeMaterial.color.b,
				1.0f - Mathf.Clamp01(elapsedTime / fadeTime));
		}
		isFading = false;
	}

	// ----------------------------------------------
	// 追記 フェードアウト
	// ----------------------------------------------
	public void FadeOut()
	{
		StartCoroutine(FadeOutCoroutine());
		Debug.Log("fadeOut");
	}	
	// 本体
	IEnumerator FadeOutCoroutine()
	{
		Debug.Log("fadeOutCroutine");
		while (true)
		{
			// フェードインが終わるまで待機
			if (isFading)
			{
				yield return 0;
			}
			else break;
		}

		// 初期化
		float elapsedTime = 0.0f;
		isFading = true;

		// フェード処理
		while (elapsedTime < fadeTime)
		{
			yield return new WaitForFixedUpdate();
			elapsedTime += Time.deltaTime;
			fadeMaterial.color = new Color(fadeMaterial.color.r, fadeMaterial.color.g, fadeMaterial.color.b, Mathf.Clamp01(elapsedTime / fadeTime));
		}
		isFading = false;

		yield return 0;
	}

	void OnPostRender()
	{		
		if (isFading)
		{
			fadeMaterial.SetPass(0);
			GL.PushMatrix();
			GL.LoadOrtho();
			GL.Color(fadeMaterial.color);
			GL.Begin(GL.QUADS);
			GL.Vertex3(0f, 0f, -12f);
			GL.Vertex3(0f, 1f, -12f);
			GL.Vertex3(1f, 1f, -12f);
			GL.Vertex3(1f, 0f, -12f);
			GL.End();
			GL.PopMatrix();
		}
	}
}
