using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseController : MonoBehaviour
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
        Debug.LogFormat("LoseScene");
        SceneManager.LoadSceneAsync("LoseScene");
    }

    private void OnClick()
    {
        GameController.LoadScene();
    }

}
