using Assets.SimpleLocalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroController : MonoBehaviour
{
    public Button ScreenBtn;
    public Text DescriptionText;

    void Awake()
    {
        if (null == GameController.Instance)
        {
            GameController.LoadScene();
            return;
        }

        ScreenBtn.onClick.AddListener(OnClick);

        DescriptionText.text = LocalizationManager.Localize("Game.IntroScene.1")+"\n"
            + LocalizationManager.Localize("Game.IntroScene.1");
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
