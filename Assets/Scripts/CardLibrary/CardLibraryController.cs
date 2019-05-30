using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(GridLayoutGroup))]
[RequireComponent(typeof(ContentSizeFitter))]
public class CardLibraryController : MonoBehaviour
{
    [SerializeField]
    private CardLibraryData cardLibraryData;

    [SerializeField]
    private RectTransform cardTemplate;

    private List<ICardController> cardControllers;

    public List<ICardController> CardControllers { get { return cardControllers; } }

    public event Action OnCardGenerationComplete;

    private void Start()
    {
        if (cardLibraryData.cardLibray == null || cardLibraryData.cardLibray.Count == 0)
            cardLibraryData.Randomize();

        GenerateCardLibrary();
    }

    private void GenerateCardLibrary()
    {
        cardControllers = new List<ICardController>();
        for (int i = 0; i < cardLibraryData.numberOfCardsInLibrary; i++)
        {
            var card = Instantiate(cardTemplate);
            CardController controller = GetCardController(i, card);
            controller.OnCardClicked += HandleCardClicked;
            cardControllers.Add(controller);
            card.SetParent(this.transform, false);
            card.gameObject.name = i.ToString();
            card.gameObject.SetActive(true);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.GetComponent<RectTransform>());

        GetComponent<ContentSizeFitter>().enabled = false;
        GetComponent<GridLayoutGroup>().enabled = false;

        OnCardGenerationComplete?.Invoke();
    }

    private void HandleCardClicked(ICardController obj)
    {
        foreach (ICardController cardController in cardControllers)
        {
            if (cardController != obj)
            {
                cardController.Model.Alpha = 0.2f;
            }
        }
    }

    private CardController GetCardController(int index, RectTransform card)
    {
        var currentSpriteIndex = cardLibraryData.cardLibray[index];
        var sprite = cardLibraryData.cardSprites[currentSpriteIndex];
        var view = card.GetComponent<CardView>();
        var model = new CardModel(index, sprite);
        var controller = new CardController(model, view);
        return controller;
    }

    private void OnDestroy()
    {
        foreach (ICardController cardController in cardControllers)
        {
            cardController.OnCardClicked -= HandleCardClicked;
        }
    }
}
