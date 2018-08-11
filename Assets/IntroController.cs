using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroController : MonoBehaviour
{
    public Button ScreenBtn;
    private Text _title;

    void Awake()
    {
        if (null == GameController.Instance)
        {
            GameController.LoadScene();
            return;
        }

        ScreenBtn.onClick.AddListener(OnClick);
        _title = ScreenBtn.gameObject.GetComponentInChildren<Text>();

        _title.text = string.Format("Tap to start");
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
