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

        _title.text = string.Format(
            "You saved {0} memories\n\n" +
            "Would you trust a merchant?\n\n" +
            "A strange talking toad ?\n\n"+
            "What about your own childhood memories ? \n\n" +
            "Let's play again!", 
            GameController.Instance.saveMemoCount);

        /*
You saved 10 memories

Would you trust a merchant? 

A strange talking toad? 

What about your own childhood memories?

Let's play again!
         */
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
