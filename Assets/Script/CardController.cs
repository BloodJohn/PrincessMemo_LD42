using UnityEngine;
using UnityEngine.UI;

public enum MemoStatus
{
    Future = 0, // еще не случилось
    Shine = 1, // Совесм новый
    Fresh = 2, // свежий
    Vague = 3, // смутный
    Deleted = 4,//стертый
}

public class CardController : MonoBehaviour
{
    [HideInInspector] public int Index;
    [HideInInspector] public MemoStatus Status = MemoStatus.Future;
    [HideInInspector] public bool Fog = false;
    public Image maskImage;
    public Image overImage;
    public Image cardImage;

    public Sprite[] maskList;
    public Sprite[] overList;

    private CastleController _castle;
    private Button _button;

    private static readonly Color AlfaColor = new Color(1f,1f,1f,0f);

    public bool IsDeleted => Status == MemoStatus.Deleted;
    public bool NotOpen => Status == MemoStatus.Future;

    private void Awake()
    {
        _button = GetComponentInChildren<Button>();
        _button.onClick.AddListener(OnClick);
    }

    public void Init(CastleController castle)
    {
        _castle = castle;
        Status = MemoStatus.Future;
        UpdateStatus();
    }

    public bool AddMemory(int index)
    {
        if (!NotOpen) return false;
        Index = index;
        Fog = false;
        Status = MemoStatus.Shine;
        cardImage.sprite = _castle.CardSpriteList[Index];
        UpdateStatus();

        return true;
    }

    public bool AddFog()
    {
        if (Fog) return false;
        Fog = true;

        if (IsDeleted) return false;

        UpdateStatus();
        return true;
    }

    public bool BlurMemory()
    {
        if (NotOpen) return false;
        if (IsDeleted) return false;

        Status++;
        UpdateStatus();
        //Debug.LogFormat("blur {0} : {1} => {2}", Index, Status, Status==MemoStatus.Deleted);
        return IsDeleted;
    }

    private void Refresh()
    {
        if (NotOpen) return;
        if (IsDeleted) return;
        Status = MemoStatus.Shine;
        UpdateStatus();

        //туман временно рассеивается!
        cardImage.sprite = _castle.CardSpriteList[Index];
        //cardImage.color = colorList[(int) MemoStatus.Shine];
    }

    private void UpdateStatus(bool useFog = true)
    {
        cardImage.color = Color.white;
        if (useFog && Fog 
            && !IsDeleted
            && !NotOpen)
        {
            cardImage.sprite = _castle.FogSprite;
            maskImage.sprite = maskList[0];
            overImage.gameObject.SetActive(false);
            return;
        }

        switch (Status)
        {
            case MemoStatus.Future:
                cardImage.color = AlfaColor;
                _button.interactable = false;
                maskImage.sprite = maskList[0];
                overImage.gameObject.SetActive(false);
                break;
            case MemoStatus.Shine:
                _button.interactable = true;
                maskImage.sprite = maskList[0];
                overImage.gameObject.SetActive(false);
                break;
            case MemoStatus.Fresh:
                _button.interactable = true;
                maskImage.sprite = maskList[1];
                overImage.sprite = overList[0];
                overImage.gameObject.SetActive(true);
                break;
            case MemoStatus.Vague:
                _button.interactable = true;
                maskImage.sprite = maskList[2];
                overImage.sprite = overList[1];
                overImage.gameObject.SetActive(true);
                break;
            case MemoStatus.Deleted:
                _button.interactable = true;
                cardImage.sprite = _castle.DeleteSprite;
                maskImage.sprite = maskList[0];
                overImage.gameObject.SetActive(false);
                break;
        }
    }

    private void OnClick()
    {
        _castle.OnTurn(this);
        Refresh();
    }
}
