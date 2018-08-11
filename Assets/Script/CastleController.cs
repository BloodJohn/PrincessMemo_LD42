using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CastleController : MonoBehaviour
{
    private const int CardMax = 15;

    public GridLayoutGroup cardTable;
    public GameObject prefab;

    private readonly List<CardController> cardList = new List<CardController>(CardMax);
    private int turnCount;

    void Awake()
    {
        if (null == GameController.Instance)
        {
            GameController.LoadScene();
            return;
        }

        for (var i = 0; i < CardMax; i++)
        {
            var obj = Instantiate(prefab, cardTable.transform);
            cardList.Add(obj.GetComponent<CardController>());

            cardList[i].Init(this, i);
        }

        turnCount = 0;
        AddCard();
    }

    public static void LoadScene()
    {
        Debug.LogFormat("CastleScene");
        SceneManager.LoadSceneAsync("CastleScene");
    }

    public void OnTurn()
    {
        for (var i = 0; i < 2; i++)
            BlurCard();

        turnCount++;

        if (turnCount < 3) AddCard();
        else if (0 == turnCount % 3) AddCard();
    }

    private void BlurCard()
    {
        var rnd = (int)((cardList.Count - 1) * Random.value);
        cardList[rnd].BlurMemory();
    }

    private void AddCard()
    {
        var rnd = (int)((cardList.Count - 1) * Random.value);
        var description = "memory new";

        for (var i = rnd; i < cardList.Count; i++)
            if (cardList[i].AddMemory(description))
                return;

        for (var i = 0; i < rnd; i++)
            if (cardList[i].AddMemory(description))
                return;

        for (var i = rnd; i < cardList.Count; i++)
            if (cardList[i].AddFog())
                return;

        for (var i = 0; i < rnd; i++)
            if (cardList[i].AddFog())
                return;

        LoseController.LoadScene();
    }
}
