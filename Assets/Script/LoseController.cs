using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseController : MonoBehaviour
{
    public Button ScreenBtn;
    public Text _title;

    void Awake()
    {
        if (null == GameController.Instance)
        {
            GameController.LoadScene();
            return;
        }

        ScreenBtn.onClick.AddListener(OnClick);
        _title = ScreenBtn.gameObject.GetComponentInChildren<Text>();

        _title.text = string.Format(
            "The end\n\n" +
            "Remained {0} of memories\n\n" +
            "Try again?", 
            GameController.Instance.saveMemoCount);
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
