using System;
using System.Collections.Generic;
using System.Linq;
using Assets.SimpleLocalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CastleController : MonoBehaviour
{
    private const int CardMax = 15;

    public Text descriptionText;
    public GridLayoutGroup cardTable;
    public GameObject prefab;
    public Sprite FogSprite;
    public Sprite DeleteSprite;
    public Sprite[] CardSpriteList;
    public string[] storyList;
    public string[] oldStoryList;
    public AudioClip[] soundList;

    private int cardIndex;

    private readonly List<CardController> cardList = new List<CardController>(CardMax);
    private int turnCount;

    private AudioSource _sound;

    void Awake()
    {
        if (null == GameController.Instance)
        {
            GameController.LoadScene();
            return;
        }

        _sound = GetComponent<AudioSource>();

        for (var i = 0; i < CardMax; i++)
        {
            var obj = Instantiate(prefab, cardTable.transform);
            cardList.Add(obj.GetComponent<CardController>());

            cardList[i].Init(this);
        }

        descriptionText.text = LocalizationManager.Localize("Game.FirstMessage");
            //"Tap on picture to open next memory\n" + "Tap on damaged picture to refresh them\n";
        turnCount = 0;
        AddCard();

        foreach (var story in storyList)
            Debug.AssertFormat(story.Length <= 216, "{0}:{1}", story.Length, story);

        foreach (var story in oldStoryList)
            Debug.AssertFormat(story.Length <= 216, "{0}:{1}", story.Length, story);

        for (var i = 0; i < 15; i++)
        {
            var story = LocalizationManager.Localize($"Game.Story.{i}");
            Debug.AssertFormat(story.Length <= 210, "Game.Story.{0}:{1}[{2}]", i,story.Length, story);
            story = LocalizationManager.Localize($"Game.OldStory.{i}");
            Debug.AssertFormat(story.Length <= 210, "Game.OldStory.{0}:{1}[{2}]", i, story.Length, story);
        }
    }

    public static void LoadScene()
    {
        Debug.LogFormat("CastleScene");
        SceneManager.LoadSceneAsync("CastleScene");
    }

    public void OnTurn(CardController card)
    {
        if (card.IsDeleted)
        {
            descriptionText.text = "That memory is gone forever.";
            _sound.PlayOneShot(soundList[4]);
            return;
        }

        turnCount++;
        //descriptionText.text = card.Fog ? oldStoryList[card.Index] : storyList[card.Index];
        descriptionText.text = card.Fog ?
            LocalizationManager.Localize($"Game.OldStory.{card.Index}") :
            LocalizationManager.Localize($"Game.OldStory.{card.Index}");

        for (var i = 0; i < 2; i++)
            if (BlurCard(card))
                _sound.PlayOneShot(soundList[4]);

        if (turnCount < 6) AddCard();
        else if (0 == turnCount % 3) AddCard();

        if (card.Fog)
        {
            _sound.PlayOneShot(soundList[3]);
        }
        else
        {
            switch (card.Status)
            {
                case MemoStatus.Shine:
                    _sound.PlayOneShot(soundList[0]);
                    break;
                case MemoStatus.Fresh:
                    _sound.PlayOneShot(soundList[1]);
                    break;
                case MemoStatus.Vague:
                    _sound.PlayOneShot(soundList[2]);
                    break;
            }
        }
    }

    private bool BlurCard(CardController saveCard)
    {
        var rnd = Random.Range(0, cardList.Count);
        rnd = Math.Min(rnd, cardList.Count - 1);
        if (saveCard == cardList[rnd]) return false;
        return cardList[rnd].BlurMemory();
    }

    private void AddCard()
    {
        var rnd = Random.Range(0, cardList.Count);
        rnd = Math.Min(rnd, cardList.Count - 1);

        if (cardIndex > CardSpriteList.Length) cardIndex = CardSpriteList.Length - 1;
        var index = cardIndex;
        cardIndex++;

        for (var i = rnd; i < cardList.Count; i++)
            if (cardList[i].AddMemory(index))
                return;

        for (var i = 0; i < rnd; i++)
            if (cardList[i].AddMemory(index))
                return;

        if (!cardList
            .Where(item => !item.IsDeleted)
            .OrderBy(item => item.Index)
            .Any(item => item.AddFog()))
        {
            GameController.Instance.saveMemoCount = cardList.Count(item => item.Status != MemoStatus.Deleted);

            LoseController.LoadScene();
        }
    }
}
