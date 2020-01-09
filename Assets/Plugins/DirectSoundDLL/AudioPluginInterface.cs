//=================================================================
//  ◆ AudioPluginInterface.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 2018/3/5
//  Description:
//    Nativeなサウンドデバイスを通すことで、レイテンシーを抑える！
//=================================================================
using System;								// IntPtr用
using System.Runtime.InteropServices;		// DLLImport用
using System.Threading;                     // マルチスレッド用
using System.Collections;           // コルーチン用
using UnityEngine;

public class AudioPluginInterface : MonoBehaviour
{
	//----------------------------------------------------------
	// マルチスレッド用
	//
	Thread thread = null;

	//----------------------------------------------------------
	// ウインドウハンドル用
	//
	IntPtr UnityHWnd;
	[DllImport("user32.dll")]
	static public extern IntPtr GetForegroundWindow();

	//----------------------------------------------------------
	// オーディオプラグイン
	//
	// 初期化
	[DllImport("AudioPlugin.dll")]
	static public extern bool DLLInitialize(IntPtr _hWnd);
	// 解放
	[DllImport("AudioPlugin.dll")]
	static public extern bool DLLRelease();
	// 更新
	[DllImport("AudioPlugin.dll")]
	static public extern void DLLUpdate();
	// オーディオソースの追加
	[DllImport("AudioPlugin.dll")]
	static public extern void AddAudioSource(string fileName, int volume);
	// 再生
	[DllImport("AudioPlugin.dll")]
	static public extern void AudioPlay(int index);
	// ループ再生
	[DllImport("AudioPlugin.dll")]
	static public extern void AudioPlayLoop(int index);
	// 停止
	[DllImport("AudioPlugin")]
	static public extern void AudioStop(int index);


	//----------------------------------------------------------
	// スタート
	//
	private void Awake ()
	{
		// ウインドウハンドル取得
		UnityHWnd = GetForegroundWindow();

		// オーディオプラグインの初期化
		DLLInitialize(UnityHWnd);

		// 0~8
		AddAudioSource("Sounds/Drum/kick.wav", 100);
		AddAudioSource("Sounds/Drum/snare.wav", 100);
		AddAudioSource("Sounds/Drum/highTom.wav", 100);
		AddAudioSource("Sounds/Drum/lowTom.wav", 100);
		AddAudioSource("Sounds/Drum/floorTom.wav", 100);
		AddAudioSource("Sounds/Drum/hihat.wav", 100);
		AddAudioSource("Sounds/Drum/crash1.wav", 100);
		AddAudioSource("Sounds/Drum/crash2.wav", 80); 
		AddAudioSource("Sounds/Drum/ride.wav", 100);

		// 最大32まで

		// マルチスレッドでプラグインを更新
		thread = new Thread(ThreadUpdate);
		thread.Start();
	}

    //----------------------------------------------------------
	// スレッドアップデート
    //
	private void ThreadUpdate()
	{
		DLLUpdate();
	}
	//----------------------------------------------------------
	// 破棄
	//
	private void OnDestroy()
	{
		// コルーチン停止
		StopCoroutine("InputCheck");

		// スレッド停止
		thread.Abort();

		// オーディオプラグインの解放
		DLLRelease();
	}
}
