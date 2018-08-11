using UnityEngine;
using UnityEngine.SceneManagement;

public class CastleController : MonoBehaviour
{
    void Awake()
    {
        if (null == GameController.Instance) GameController.LoadScene();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void LoadScene()
    {
        Debug.LogFormat("CastleScene");
        SceneManager.LoadSceneAsync("CastleScene");
    }
}
