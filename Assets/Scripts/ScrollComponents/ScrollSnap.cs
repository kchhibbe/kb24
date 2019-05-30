using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum SnapEdge
{
    Top,
    Bottom
}

[RequireComponent(typeof(ScrollDirection))]
public class ScrollSnap : MonoBehaviour, IDragHandler
{
    [SerializeField]
    private RectTransform top;

    [SerializeField]
    private RectTransform bottom;

    [SerializeField]
    private CardLibraryController cardLibraryController;

    private ScrollRect scrollRect;
    private bool scrolling;
    private List<RectTransform> cardRects;
    private float distanceBetweenTwoRows;

    private ScrollDirection scrollDirection;

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        scrollDirection = GetComponent<ScrollDirection>();
        scrollDirection.OnDragEnd += HandleDragEnd;
        cardLibraryController.OnCardGenerationComplete += HandleCardGeneration;
    }

    private void HandleCardGeneration()
    {
        // GetComponent<UI_ScrollRectOcclusion>().Init();
        cardRects = new List<RectTransform>();
        //TODO: CHANGE THIS TO GET SCROLL RECT CONTENT CHILDREN
        foreach (var card in cardLibraryController.CardControllers)
        {
            var cardMonoBehaviour = card.View as MonoBehaviour;
            if (cardMonoBehaviour != null)
            {
                var cardRect = cardMonoBehaviour.GetComponent<RectTransform>();
                cardRects.Add(cardRect);
            }
        }
        CalculateDistanceBetweenRows();
    }

    private void Update()
    {
        if (scrolling && scrollRect.velocity.sqrMagnitude < 100f)
            scrolling = false;
    }

    private void CalculateDistanceBetweenRows()
    {
        var firstRowPosition = cardRects[0].position;
        var secondRowPosition = Vector3.zero;
        for (int i = 0; i < cardRects.Count; i++)
        {
            if (cardRects[i].position.y != firstRowPosition.y)
            {
                secondRowPosition = cardRects[i].position;
                break;
            }
        }
        distanceBetweenTwoRows = Mathf.Abs(firstRowPosition.y - secondRowPosition.y);
    }

    private void HandleDragEnd(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                Snap(SnapEdge.Bottom);
                break;
            case Direction.Down:
                Snap(SnapEdge.Top);
                break;
            default:
                Debug.Log("Scroll Direction Error!");
                break;
        }
    }

    public void Snap(SnapEdge edge)
    {
        StopAllCoroutines();
        StartCoroutine(SnapTo(edge));
    }

    private IEnumerator SnapTo(SnapEdge edge)
    {
        yield return new WaitUntil(() => scrolling == false);
        var closestCard = GetClosestCard(edge);

        if (!scrolling)
            LerpTo(closestCard, edge);
    }

    private void LerpTo(RectTransform card, SnapEdge edge)
    {
        RectTransform target = (edge == SnapEdge.Top) ? top : bottom;
        var yDistance = card.position.y - target.position.y;
        var lerpDistance = new Vector3(0f, -yDistance, 0f);
        if (yDistance < 0)
            lerpDistance -= new Vector3(0f, distanceBetweenTwoRows, 0f);

        lerpDistance += new Vector3(0f, distanceBetweenTwoRows / 2, 0f);
        var rect = cardLibraryController.GetComponent<RectTransform>();
        StartCoroutine(MoveRectTransform(rect, lerpDistance, 1f));
    }

    private IEnumerator MoveRectTransform(RectTransform rect, Vector3 difference, float time)
    {
        Vector3 startPos = rect.position;
        Vector3 endPos = startPos + difference;
        float rate = 1.0f / time;
        float t = 0.0f;

        while (t < 1.0f)
        {
            t += Time.deltaTime * rate;
            rect.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
    }

    private RectTransform GetClosestCard(SnapEdge edge)
    {
        RectTransform target = (edge == SnapEdge.Top) ? top : bottom;
        RectTransform closestCard = null;
        float closestDistanceSquare = Mathf.Infinity;

        foreach (RectTransform card in cardRects)
        {
            Vector3 directionToTarget = target.position - card.position;
            float directionSquared = directionToTarget.sqrMagnitude;

            if (directionSquared < closestDistanceSquare)
            {
                closestCard = card;
                closestDistanceSquare = directionSquared;
            }
        }
        return closestCard;
    }

    public void OnDrag(PointerEventData eventData)
    {
        scrolling = true;
    }

    private void OnDestroy()
    {
        scrollDirection.OnDragEnd -= HandleDragEnd;
        cardLibraryController.OnCardGenerationComplete -= HandleCardGeneration;
    }
}
