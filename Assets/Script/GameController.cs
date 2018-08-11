using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        UnityEngine.Random.InitState(DateTime.UtcNow.Millisecond);
    }

    private void Start()
    {
        CastleController.LoadScene();
    }

    public static void LoadScene()
    {
        Debug.LogFormat("LoadScene");
        SceneManager.LoadSceneAsync("LoadScene");
    }
}