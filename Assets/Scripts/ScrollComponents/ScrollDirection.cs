using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum Direction
{
    Up,
    Down
}

[RequireComponent(typeof(ScrollRect))]
public class ScrollDirection : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    private Vector2 startPosition = Vector2.zero;

    public event Action<Direction> OnDragEnd;

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var direction = eventData.position - startPosition;
        startPosition = Vector2.zero;
        if (direction.y >= 0)
            OnDragEnd?.Invoke(Direction.Up);
        else
            OnDragEnd?.Invoke(Direction.Down);
    }
}
