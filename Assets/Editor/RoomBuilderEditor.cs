using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(RoomBuilder))]
public class RoomBuilderEditor : Editor
{
    public GameObject tile;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        RoomBuilder myScript = (RoomBuilder)target;
        if (GUILayout.Button("Build Object"))
        {
            RoomScript myRoom = myScript.roomToBuild.GetComponent<RoomScript>();
           
            for (var i = myRoom.spritesParent.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(myRoom.spritesParent.GetChild(i).gameObject);
            }

            foreach (Block b in myRoom.GetBlocks())
            {
                var bPos = new Vector3(b.position.x, b.position.y) + myRoom.transform.position;
                Instantiate(
                    myScript.tilePrefab,
                    bPos,
                    Quaternion.identity,
                    myRoom.spritesParent
                );
            }
            foreach (Connector c in myRoom.GetConnectors())
            {
                Instantiate(
                    myScript.conPrefab,
                    (Vector3) c.GetRelativePosition() + myRoom.transform.position,
                    Quaternion.identity,
                    myRoom.spritesParent
                );
            }
        }
    }
}