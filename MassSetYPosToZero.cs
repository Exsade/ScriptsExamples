using UnityEngine;
using UnityEditor;

//This script allows move objects to nearly Y = 0 regarding camera view. Camera rotation on this project was fixed(x: 31.5; y: 45; z: 0)

public static class MassSetYPositionToZero
{
    //Constants for correct move regarding camera view
    private const float X_STEP = 0.1382f;
    private const float Y_STEP = 0.1374f;
    private const float Z_STEP = 0.1382f;

    [MenuItem("GameObject/MassSetYPositionToZero", false, 20)]
    public static void CloserToCamera(MenuCommand menuCommand)
    {
        var gameObject = menuCommand.context as GameObject;
        int _childsCount = gameObject.transform.childCount;

        for (int i = 0; i < _childsCount; i++)
        {
            Vector3 _currentPos = gameObject.transform.GetChild(i).transform.position;

            gameObject.transform.GetChild(i).transform.position = PositionChange(_currentPos);
        }
    }

    public static Vector3 PositionChange(Vector3 pos)
    {
        if (pos.y > 0)
        {
            while (pos.y > 0)
            {
                pos = new Vector3(pos.x + X_STEP, pos.y - Y_STEP, pos.z + Z_STEP);
            }

            return pos;
        }

        if (pos.y < 0)
        {
            while (pos.y < 0)
            {
                pos = new Vector3(pos.x - X_STEP, pos.y + Y_STEP, pos.z - Z_STEP);
            }

            return pos;
        }

        return pos;
    }

}
