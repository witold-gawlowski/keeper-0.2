using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public enum Direction
{
    Top = 0, Right = 1, Bottom = 2, Left = 3
}

[System.Serializable]
public class ConnectorData
{
    public Vector2 position;
    public Direction direction;
    public RoomScript parent;
    public ConnectorData other;
}

[System.Serializable]
public class Block
{
    public Vector2Int position;
    public List<Direction> connectors;
}
[System.Serializable]
public class Connector
{
    static Dictionary<Direction, Vector2> shifts =
    new Dictionary<Direction, Vector2>{
            {Direction.Bottom, Vector2.down * 0.5f},
            {Direction.Left, Vector2.left * 0.5f},
            {Direction.Right, Vector2.right * 0.5f},
            {Direction.Top, Vector2.up * 0.5f}
    };
    public Vector2Int blockPosition;
    public Direction direction;

    public Vector2 GetRelativePosition()
    {
        return blockPosition +
            shifts[direction];
    }

    public Connector(Vector2Int position, Direction direction)
    {
        this.blockPosition = position;
        this.direction = direction;
    }
}

public class RoomScript : MonoBehaviour
{
    public RoomGroup parent;
    public Block[] blocks;
    public Transform spritesParent;

    public void Rotate()
    {
        blocks.ToList().ForEach((i) =>
        {
            i.position = new Vector2Int(-i.position.y, i.position.x);
            i.connectors = i.connectors.Select((j) => (Direction)(((int)j + 1) % 4)).ToList();
        });
    }

    public void UpdateShift(Vector2 value)
    {
        spritesParent.transform.localPosition = value;
    }

    public void Connect(ConnectorData local, ConnectorData other)
    {
        local.other = other;
        other.other = local;

        RoomScript otherRoom = other.parent;
        List<RoomScript> otherRooms = otherRoom.parent.GetRooms();
        parent.Add(otherRooms);

        otherRoom.parent.Clear();
    }
    public List<Connector> GetConnectors()
    {
        return blocks.Select((i) =>
            i.connectors.Select((j) =>
                new Connector(i.position, j)
            )
        )
        .SelectMany(k => k).ToList();
    }
    public List<Block> GetBlocks()
    {
        return blocks.ToList();
    }
}
