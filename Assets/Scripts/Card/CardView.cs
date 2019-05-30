using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public interface ICardView
{
    event Action OnClicked;
    Sprite CardSprite { set; }
    float Alpha { set; }
}

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Button))]
[RequireComponent(typeof(CanvasGroup))]
public class CardView : MonoBehaviour, ICardView
{
    private Button cardButton;
    private CanvasGroup canvasGroup;

    [SerializeField]
    private Image cardImage;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float lerpDuration = 0.25f;

    public event Action OnClicked;

    public Sprite CardSprite
    {
        set
        {
            cardImage.sprite = value;
        }
    }

    public float Alpha
    {
        set
        {
            SetCanvasGroupAlpha(value);
        }
    }

    private void Awake()
    {
        cardButton = GetComponent<Button>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        cardButton.onClick.AddListener(CardOnClick);
    }

    private void CardOnClick()
    {
        OnClicked?.Invoke();
    }

    private void SetCanvasGroupAlpha(float value)
    {
        float startAlpha = canvasGroup.alpha;
        float endAlpha = Mathf.Clamp(value, 0.0f, 1.0f);

        StartCoroutine(LerpAlpha(startAlpha, endAlpha, lerpDuration));
    }

    private IEnumerator LerpAlpha(float startAlpha, float endAlpha, float time)
    {
        float rate = 1.0f / time;
        float t = 0.0f;

        while (t < 1.0f)
        {
            t += Time.deltaTime * rate;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            yield return null;
        }
    }

}
