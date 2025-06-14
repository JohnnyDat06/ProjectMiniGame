using UnityEngine;

public enum ArrowDirection
{
    Up,
    Down,
    Left,
    Right
}

public class ArrowData
{
    public ArrowDirection Direction { get; private set; }
    public GameObject ArrowGameObject { get; private set; }
    public ArrowData(ArrowDirection direction, GameObject arrowGameObject)
    {
        Direction = direction;
        ArrowGameObject = arrowGameObject;
    }
}