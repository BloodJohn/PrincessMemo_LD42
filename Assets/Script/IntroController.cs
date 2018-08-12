using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroController : MonoBehaviour
{
    public Button ScreenBtn;

    void Awake()
    {
        if (null == GameController.Instance)
        {
            GameController.LoadScene();
            return;
        }

        ScreenBtn.onClick.AddListener(OnClick);
    }

    public static void LoadScene()
    {
        Debug.LogFormat("IntroScene");
        SceneManager.LoadSceneAsync("IntroScene");
    }

    private void OnClick()
    {
        CastleController.LoadScene();
    }

}
