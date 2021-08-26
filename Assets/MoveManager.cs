using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MoveManager : MonoBehaviour
{
    RoomGroup draggedGroup;
    Vector2 shift;
    ConnectorIterator snappedDragged;
    ConnectorIterator snappedStatic;
    [SerializeField]
    List<ConnectorIterator> staticConnectors;
    [SerializeField]
    List<ConnectorIterator> draggedConnectors;
    public float snapThreshold = 1.0f;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))
            {
                Collider2D collider = Physics2D.OverlapPoint(mousePosition);
                if (collider)
                {
                    RoomScript selectedRoom = collider.GetComponentInParent<RoomScript>();
                    draggedGroup = selectedRoom.parent;
                    shift = (Vector2)draggedGroup.transform.position - mousePosition;

                    SetupConnectors();
                }
            }

            if (draggedGroup)
            {
                draggedGroup.transform.position = mousePosition + shift;
                GetClosestConnectors();
            }
        }
        if (snappedDragged != null && snappedStatic != null)
        {
            RoomGroup draggedGroup = snappedDragged.room.parent;
            SetDraggedGroupSpritesShift();
        }
        if (Input.GetMouseButtonUp(0))
        {
            draggedGroup = null;
        }
    }

    public void SetDraggedGroupSpritesShift()
    {
        Vector2 absStatConPos = snappedStatic.connector.GetRelativePosition() +
            (Vector2) snappedStatic.room.transform.position;
        Vector2 absDragConPos = snappedDragged.connector.GetRelativePosition() +
            (Vector2)snappedDragged.room.transform.position;
        Vector2 shift = absStatConPos - absDragConPos;

        foreach(RoomScript rs in draggedGroup.GetRooms())
        {
            rs.UpdateShift(shift);
        }
    }


    public void SetupConnectors()
    {
        staticConnectors =
            GameObject.FindObjectsOfType<RoomGroup>()
            .Except(new[] { draggedGroup })
            .SelectMany((i) => i.GetConnectors())
            .ToList();

        draggedConnectors = draggedGroup.GetConnectors().ToList();
    }

    private void Snap(ConnectorIterator draggedConnector, ConnectorIterator staticConnector)
    {
        snappedDragged = draggedConnector;
        snappedStatic = staticConnector;
    }

    private void UnSnap()
    {
        foreach (RoomScript rs in draggedGroup.GetRooms())
        {
            rs.UpdateShift(Vector2.zero);
        }
        snappedStatic = null;
        snappedDragged = null;
    }

    private void GetClosestConnectors()
    {
        float minDist = 999;
        ConnectorIterator minDC = null;
        ConnectorIterator minSC = null;
        foreach (var dc in draggedConnectors)
        {
            Vector2 dcPos = dc.connector.GetRelativePosition() + (Vector2) dc.room.transform.position;
            foreach (var sc in staticConnectors)
            {
                Vector2 scPos = sc.connector.GetRelativePosition() + (Vector2) sc.room.transform.position;

                float dist = (dcPos - scPos).magnitude;
                if (minDist > dist)
                {
                    minDC = dc;
                    minSC = sc;
                    minDist = dist;
                }
            }
        }
        
        if (minDist < snapThreshold)
        {
            if (minDC != snappedDragged || minSC != snappedStatic)
            {
                Snap(minDC, minSC);
            }
        }
        else
        {
            UnSnap();
        }
    }

}
