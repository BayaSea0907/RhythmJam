//=================================================================
//  ◆ GameManager.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 2018/2/2
//  Description:
//    ゲームの場面、イベント、表示などを管理する。
//=================================================================
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Water;

public enum GameState
{
    Debug = -1,
    Opening,
    // Demo,
    Title,
    Stage1,
    Stage2,
    Stage3,
    Result,
}

public class GameManager : MonoBehaviour
{
    // キーによるシーン操作を行うか
    [SerializeField] private bool isKeyDebugOperation = false;

	// VRを使用しているか
	// static public bool IsVRActive { get { } }

    // 現在のゲーム状態。外部から取得のみ可能。
    static public GameState GameState { get { return _gameState; } }
    static private GameState _gameState = GameState.Opening;
    static private GameState currentGameState = GameState.Opening;

    static public int stage1EndTiming = 28;
    static public int stage2StartTiming = 29;
    static public int stage2EndTiming = 70;
    static public int gameEndTiming = 100;
    static public int CurrentMusicBarOffset = 0;
    [SerializeField] private int monitorMusicBarOffset = 0;

    // シーンの最初のロード時か
    private bool isDoneInit = false;

    //----------------------------------------------------------
    // アウェイク
    //
    private void Awake()
    {
        // exe起動時のカーソル非表示
        if (!Debug.isDebugBuild)
        {
            Cursor.visible = false;
        }

        // シーン間で共有出来るようにする
        DontDestroyOnLoad(this.gameObject);

        // 最初に呼び出すシーンの適用
        _gameState = GameState.Opening;
        currentGameState = _gameState;
    }

    //----------------------------------------------------------
    // スタート
    //
    private void Start()
    {
        CurrentGameStateUpdate();
    }

    //----------------------------------------------------------
    // アップデート
    //
    private void Update()
    {
        //----------------------------------------------------------
        // ESC. ゲーム終了。exe実行時のみ使える
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        // デバック操作のチェック
        if (isKeyDebugOperation) KeyDebugCheck();

        // ゲーム状態が変わった最初のフレームのみ更新
        if (currentGameState != _gameState)
        {
            currentGameState = _gameState;
            Debug.Log(currentGameState.ToString() + "を呼び出し。");
            isDoneInit = false;
        }

        // インスペクターに現在のBarを表示
        monitorMusicBarOffset = CurrentMusicBarOffset;

        // ゲーム状態に合わせたアップデート
        CurrentGameStateUpdate();
    }

    //----------------------------------------------------------
    // キー入力によるデバック操作
    private void KeyDebugCheck()
    {
		//----------------------------------------------------------
		// 学内共通のオペレーション。LeftCtrlを押してる場合適用。
		//
		if (Input.GetKey(KeyCode.LeftControl)) {
			// F1. 最初のシーン（オープニングへ）
			if (Input.GetKeyDown(KeyCode.F1))
			{
				SceneLoader.LoadScene("Opening");
				_gameState = GameState.Opening;
				isDoneInit = false;
			}

			// F3. 難易度選択 or デバック用。
			else if (Input.GetKeyDown(KeyCode.F2)) ;

			// F4. アプリケーション終了。(ESCと同様)
			else if (Input.GetKeyDown(KeyCode.F3)) Application.Quit();

			// F5. 次にのチェックポイントへ進む
			else if (Input.GetKeyDown(KeyCode.F4))
			{
				SceneLoader.LoadScene("Title");
				_gameState = GameState.Title;
				isDoneInit = false;
			}

			// F6. 前のチェックポイントに戻る
			else if (Input.GetKeyDown(KeyCode.F5))
			{
				SceneLoader.LoadScene("GameMain");
				_gameState = GameState.Stage1;
				isDoneInit = false;
			}
		}
    }

    //----------------------------------------------------------
    // 現在のゲーム状態に合わせたアップデート。
    // 細かい処理は各オブジェクトからステートを呼び出して処理するので、
    // ここではあまり干渉しない。
    private void CurrentGameStateUpdate()
    {
        switch (GameState)
        {
            case GameState.Opening:
                Opening();
                break;

            // case GameState.Demo:
            //     Demo();
            //     break;

            case GameState.Title:
                Title();
                break;

            case GameState.Stage1:
                Stage1();
                break;

            case GameState.Stage2:
                Stage2();
                break;

            case GameState.Stage3:
                Stage3();
                break;

            case GameState.Result:
                Result();
                break;
            default:
                break;
        }
    }

    //----------------------------------------------------------
    // 各場面のロード処理
    //
    // スポンサー
    private void Opening()
    {
        // フェードイン
        if (!isDoneInit && !SceneLoader.IsSceneLoadRunning)
        {
            CurrentMusicBarOffset = 0;
            monitorMusicBarOffset = 0;

            // オープニングのロード
            SceneLoader.LoadScene("Opening");
            isDoneInit = true;
        }

        // タイトルへ
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || 
            OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) ||
            Input.GetKeyDown(KeyCode.Return))
        {
            _gameState = GameState.Title;
        }
    }

    //----------------------------------------------------------
    // デモ
    //
    private void Demo()
    {
        // フェードイン

        // デモ

        // フェードアウト

        if (OVRInput.GetDown(OVRInput.Button.Down)) _gameState = GameState.Title;
    }

    //----------------------------------------------------------
    // タイトル
    private void Title()
    {
        // 最初の読み込み
        if (!isDoneInit && !SceneLoader.IsSceneLoadRunning)
        {
            CurrentMusicBarOffset = 0;
            monitorMusicBarOffset = 0;
            isDoneInit = true;
        }

        // タイトル用オブジェクトの読み込みと、
        // カメラの処理を起動
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) ||
            OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) ||
            Input.GetKeyDown(KeyCode.Return))
        {
            _gameState = GameState.Stage1;
        }
    }

    //----------------------------------------------------------
    // ステージ１
    //
    public void Stage1()
    {
        // 最初の読み込み
        if (!isDoneInit && !SceneLoader.IsSceneLoadRunning)
        {
            isDoneInit = true;
        }

        // 次のフェイズへ
        if (CurrentMusicBarOffset > stage2StartTiming)
        {
            _gameState = GameState.Stage2;
        }
    }

    //----------------------------------------------------------
    // ステージ2
    //
    public void Stage2()
    {
        if (!isDoneInit)
        {
            isDoneInit = true;
        }

        // 次のフェイズへ
        if (CurrentMusicBarOffset > stage2EndTiming)
        {
            _gameState = GameState.Stage3;
        }
    }

    //----------------------------------------------------------
    // ステージ3
    //
    public void Stage3()
    {
        if (!isDoneInit)
        {
            isDoneInit = true;
        }
    }

    //----------------------------------------------------------
    // リザルト
    //
    private void Result()
    {
        if (!isDoneInit)
        {
            isDoneInit = true;
        }

        if (CurrentMusicBarOffset > GameManager.gameEndTiming)
        {
            _gameState = GameState.Opening;
        }
    }
}