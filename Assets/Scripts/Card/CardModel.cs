using UnityEngine;
using System;

public interface ICardModel
{
    event Action OnAlphaChanged;
    event Action OnSpriteChanged;
    int CardIndex { get; }
    Sprite CardSprite { get; set; }
    float Alpha { get; set; }
}

public class CardModel : ICardModel
{
    private int cardIndex;
    public int CardIndex
    {
        get { return cardIndex; }
        private set
        {
            if (cardIndex != value)
                cardIndex = value;
        }
    }

    public event Action OnSpriteChanged;

    private Sprite cardSprite;
    public Sprite CardSprite
    {
        get { return cardSprite; }
        set
        {
            if (cardSprite != value)
            {
                cardSprite = value;
                OnSpriteChanged?.Invoke();
            }
        }
    }

    public event Action OnAlphaChanged;

    private float alpha;
    public float Alpha
    {
        get { return alpha; }
        set
        {
            if (alpha != value && alpha >= 0.0f && alpha <= 1.0f)
            {
                alpha = value;
                OnAlphaChanged?.Invoke();
            }
        }
    }

    public CardModel(int cardIndex, Sprite cardSprite)
    {
        this.CardIndex = cardIndex;
        this.CardSprite = cardSprite;
        this.Alpha = 1.0f;
    }
}
