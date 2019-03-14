using Assets.SimpleLocalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseController : MonoBehaviour
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

        _title.text =
            string.Format(LocalizationManager.Localize("Game.LoseScene.1"), GameController.Instance.saveMemoCount) + "\n\n" +
            LocalizationManager.Localize("Game.LoseScene.2") + "\n\n" +
            LocalizationManager.Localize("Game.LoseScene.3") + "\n\n" +
            LocalizationManager.Localize("Game.LoseScene.4") + "\n\n" +
            LocalizationManager.Localize("Game.LoseScene.5");
    }

    public static void LoadScene()
    {
        Debug.LogFormat("LoseScene");
        SceneManager.LoadSceneAsync("LoseScene");
    }

    private void OnClick()
    {
        CastleController.LoadScene();
    }

}
