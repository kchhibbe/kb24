using System;
using UnityEngine;

public interface ICardController
{
    ICardModel Model { get; }
    ICardView View { get; }
    event Action<ICardController> OnCardClicked;
}

public class CardController : ICardController, IDisposable
{
    private ICardModel model;
    public ICardModel Model { get { return model; } }

    private ICardView view;
    public ICardView View { get { return view; } }

    public event Action<ICardController> OnCardClicked;

    public CardController(ICardModel model, ICardView view)
    {
        this.model = model;
        this.view = view;
        Initialize();
    }

    private void Initialize()
    {
        model.OnSpriteChanged += HandleSpriteChanged;
        model.OnAlphaChanged += HandleAlphaChanged;
        view.OnClicked += HandleClicked;
        UpdateSprite();
        UpdateAlpha();
    }

    private void HandleClicked()
    {
        OnCardClicked?.Invoke(this);
    }

    private void HandleSpriteChanged()
    {
        UpdateSprite();
    }

    private void HandleAlphaChanged()
    {
        UpdateAlpha();
    }

    private void UpdateSprite()
    {
        view.CardSprite = model.CardSprite;
    }

    private void UpdateAlpha()
    {
        view.Alpha = model.Alpha;
    }

    public void Dispose()
    {
        model.OnSpriteChanged -= HandleSpriteChanged;
        model.OnAlphaChanged -= HandleAlphaChanged;
        view.OnClicked -= HandleClicked;
    }
}
