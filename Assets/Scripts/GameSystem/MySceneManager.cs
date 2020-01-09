//=================================================================
//  ◆ MySceneManager.cs
//-----------------------------------------------------------------
//  Author:
//    H.Kobayashi - 2018/2/2
//  Description:
//    シーン管理
//=================================================================
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

//**********************************************************
// シーン管理者
//
public class MySceneManager : SingletonMonoBehaviour<MySceneManager>
{
    // シーン遷移処理を実行中か
    public bool isRunning = false;

    // シーンロード完了通知
    private Subject<Unit> onAllSceneLoaded = new Subject<Unit>();
    public IObservable<Unit> OnScenesLoaded { get { return onAllSceneLoaded; } }

    //----------------------------------------------------------
    // アウェイク
    //
    private void Awake()
    {
        // シーン間で共有出来るようにする
        DontDestroyOnLoad(this.gameObject);
    }

    //----------------------------------------------------------
    // シーンのリリース
    //
    public void ReleaseScene(string sceneName)
    {
        StartCoroutine(SceneReleaseColutine(sceneName));
    }
    // アンロード処理の本体
    private IEnumerator SceneReleaseColutine(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);

        if (scene != null)
            yield return SceneManager.UnloadSceneAsync(scene.buildIndex);
        else
            Debug.Log(sceneName + "というSceneはないので、アンロードできません！！");

        yield return null;
    }

    //----------------------------------------------------------
    // シーン遷移
    // 新規
    public void StartSceneLoad(string sceneName)
    {
        if (!isRunning)
            StartCoroutine(SceneLoadCoroutine(sceneName, false));
    }
    // 追加
    public void StartAddtiveScene(string sceneName)
    {
        if (!isRunning)
            StartCoroutine(SceneLoadCoroutine(sceneName, true));
    }
    // 遷移処理の本体
    private IEnumerator SceneLoadCoroutine(string sceneName, bool isAddtive)
    {
        // 処理開始フラグ
        isRunning = true;

        // 追加シーンがある場合は一緒に読み込む
        if (!isAddtive)
        {
            // メインとなるシーンをSingleで読み込む
            SceneManager.LoadSceneAsync(sceneName.ToString(), LoadSceneMode.Single);
        }
        else
        {
            // すでにシーンがある場合はロードしない
            Scene scene = SceneManager.GetSceneByName(sceneName);
            if (scene.name != sceneName)
            {
                // バックグラウンドでシーンを非同期にロード
                yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            }
            else
            {
                // ReleaseScene(sceneName);
                // yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            }
        }

        // 使ってないリソースの解放と、GC(ガベコレ) を実行
        Resources.UnloadUnusedAssets();
        GC.Collect();

        // シーンロードの完了通知を発行(Unity Reactive Extentions)
        onAllSceneLoaded.OnNext(Unit.Default);

        //終了
        isRunning = false;
    }
}

//**********************************************************
// シーン受け渡し用インターフェース
//
public static class SceneLoader
{
    // シーン遷移マネージャ
    private static MySceneManager _sceneManager;
    public static MySceneManager SceneManager
    {
        get
        {
            if (_sceneManager != null) return _sceneManager;

            Initialize();
            return _sceneManager;
        }
    }

    //----------------------------------------------------------
    // イニシャライザ
    //
    private static void Initialize()
    {
        if (MySceneManager.Instance == null)
        {
            var resource = Resources.Load("Utilities/TransitionCanvas");
            GameObject.Instantiate(resource);
        }
        _sceneManager = MySceneManager.Instance;
    }

    // シーンのロードが全て完了したことを通知する
    public static IObservable<Unit> OnScenesLoaded { get { return MySceneManager.Instance.OnScenesLoaded.FirstOrDefault(); } }

    // 遷移処理のフラグチェック
    public static bool IsSceneLoadRunning
    {
        get { return MySceneManager.Instance.isRunning; }
    }

    // 任意のシーンをロード
    public static void LoadScene(string sceneName)
    {
        // 次のシーンの呼び出し
        MySceneManager.Instance.StartSceneLoad(sceneName);
    }

    public static void AddtiveScene(string sceneName)
    {
        MySceneManager.Instance.StartAddtiveScene(sceneName);
    }

    public static void ReleaseScene(string sceneName)
    {
        MySceneManager.Instance.ReleaseScene(sceneName);
    }
}