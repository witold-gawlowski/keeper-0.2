using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ConnectorIterator
{
    public RoomScript room;
    public Connector connector;


    public ConnectorIterator(RoomScript room, Connector connector)
    {
        this.room = room;
        this.connector = connector;
    }
}

public class RoomGroup : MonoBehaviour
{
    [SerializeField]
    private List<RoomScript> rooms;

    public void Add(List<RoomScript> rooms)
    {
        rooms.AddRange(rooms);
    }
    public List<RoomScript> GetRooms()
    {
        return rooms;
    }
    public void Clear()
    {
        rooms.Clear();
    }
    public IEnumerable<ConnectorIterator> GetConnectors()
    {
        foreach (var r in rooms)
        {
            foreach (var b in r.GetConnectors())
            {
                yield return new ConnectorIterator(r, b);
            }
        }
    }

    public bool DoesCollide(RoomGroup other, Vector2Int shift)
    {
        // tworzymy reprezentację mapową obu kloców
        // przechodzimy po całym klocku other i sprawdzamy czy bloki na siebie nie nachodzą
        // przechodzimy po wszystkich wejściach jednego i drugiego klocka
        // // patrzymy czy przeciwny klocek jest pusty w tych miejscach
        // // // jeśli nie to wrzucamy do seta takie wyjście
        // // // wrzucając patrzymy czy nie ma tam już lustrzanego
        // na koniec patrzymy czy set jest pusty
        return false;
    }
}