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
    public int Index;
    public MemoStatus Status = MemoStatus.Future;
    public bool Fog = false;

    public Color[] colorList;

    private CastleController _castle;
    private Button _button;
    private Image _image;
    private Text _text;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _text = GetComponentInChildren<Text>();

        _button = GetComponent<Button>();
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
        if (Status != MemoStatus.Future) return false;
        Index = index;
        _image.sprite = _castle.CardSpriteList[Index];
        Status = MemoStatus.Shine;
        _text.text = string.Empty;
        UpdateStatus();

        return true;
    }

    public bool AddFog()
    {
        if (Fog) return false;
        Fog = true;

        if (Status == MemoStatus.Deleted) return false;

        UpdateStatus();
        return true;
    }

    public void BlurMemory()
    {
        if (Status == MemoStatus.Future) return;
        if (Status == MemoStatus.Deleted) return;

        Status++;
        UpdateStatus();
    }

    private void Refresh()
    {
        if (Status == MemoStatus.Future) return;
        if (Status == MemoStatus.Deleted) return;
        Status = MemoStatus.Shine;
        UpdateStatus();

        //туман временно рассеивается!
        _image.sprite = _castle.CardSpriteList[Index];
        _image.color = colorList[(int) MemoStatus.Shine];
    }

    private void UpdateStatus()
    {
        _image.color = colorList[(int) Status];
        if (Fog)
        {
            _image.color = colorList[(int)MemoStatus.Fresh];
            _image.sprite = _castle.FogSprite;
        }

        switch (Status)
        {
            case MemoStatus.Future:
                _button.interactable = false;
                break;
            case MemoStatus.Shine:
                _button.interactable = true;
                break;
            case MemoStatus.Fresh:
                _button.interactable = true;
                break;
            case MemoStatus.Vague:
                _button.interactable = true;
                break;
            case MemoStatus.Deleted:
                _button.interactable = false;
                _image.color = colorList[(int)MemoStatus.Deleted];
                _image.sprite = _castle.DeleteSprite;
                break;
        }
    }

    private void OnClick()
    {
        //Debug.LogFormat("OnClick {0}", Index);

        Refresh();

        _castle.OnTurn();
    }
}
