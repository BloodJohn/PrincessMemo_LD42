using System;
using UnityEngine;
using UnityEngine.UI;

public enum MemoStatus
{
    Future = 0, // еще не случилось
    Shine = 1, // свежий
    DiedDown = 2, //улегшийся
    Vague = 3, //смутный
    Deleted = 4,//стертый
}

public class CardController : MonoBehaviour
{
    public int Index;
    public MemoStatus Status = MemoStatus.Future;
    public bool Fog = false;

    private CastleController _castle;
    private Button _button;
    private Image _image;
    private Text _text;

    private string _description;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _text = GetComponentInChildren<Text>();

        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    public void Init(CastleController castle, int index)
    {
        _castle = castle;
        Index = index;
    }

    public bool AddMemory(string description)
    {
        if (Status != MemoStatus.Future) return false;

        _description = description;
        Refresh();

        return true;
    }

    public bool AddFog()
    {
        if (Fog) return false;
        if (Status == MemoStatus.Deleted) return false;

        Fog = true;
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
        //UpdateStatus();
        _text.text = _description; //туман временно рассеивается!
    }

    private void UpdateStatus()
    {
        switch (Status)
        {
            case MemoStatus.Future:
                _text.text = string.Empty;
                break;
            case MemoStatus.Shine:
                _text.text = _description;
                break;
            case MemoStatus.DiedDown:
                _text.text = string.Format("({0})", _description.Substring(0,5));
                break;
            case MemoStatus.Vague:
                _text.text = string.Format("-={0}=-", _description.Substring(0,3));
                break;
            case MemoStatus.Deleted:
                _text.text = "deleted";
                break;
        }

        if (Status == MemoStatus.Deleted) Fog = false;

        if (Fog) _text.text = "###";
    }

    private void OnClick()
    {
        Debug.LogFormat("OnClick {0}", Index);

        Refresh();

        _castle.OnTurn();
    }
}
