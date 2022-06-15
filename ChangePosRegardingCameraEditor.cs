using UnityEngine;
using UnityEditor;

//This script allows to move objects regarding camera view. Camera rotation on this project was fixed(x: 31.5; y: 45; z: 0)
//This script was very helpfull for controll object position in z buffer, for correct sorting

public static class ChangePosRegardingCameraEditor
{
    //Constants for correct move regarding camera view
    private const float X_STEP = 0.2764f;
    private const float Y_STEP = 0.2748f;
    private const float Z_STEP = 0.2764f;

    [MenuItem("CONTEXT/Transform/CloserToCamera")]
    public static void CloserToCamera(MenuCommand menuCommand)
    {
        Transform _transform = menuCommand.context as Transform;
        _transform.position = new Vector3(_transform.position.x - X_STEP, _transform.position.y + Y_STEP, _transform.position.z - Z_STEP);
    }

    [MenuItem("CONTEXT/Transform/FurtherFromCamera")]
    public static void FurtherFromCamera(MenuCommand menuCommand)
    {
        Transform _transform = menuCommand.context as Transform;
        _transform.position = new Vector3(_transform.position.x + X_STEP, _transform.position.y - Y_STEP, _transform.position.z + Z_STEP);
    }

    [MenuItem("CONTEXT/Transform/CloserToCameraX5")]
    public static void CloserToCameraX3(MenuCommand menuCommand)
    {
        Transform _transform = menuCommand.context as Transform;
        _transform.position = new Vector3(_transform.position.x - X_STEP * 5, _transform.position.y + Y_STEP * 5, _transform.position.z - Z_STEP * 5);
    }

    [MenuItem("CONTEXT/Transform/FurtherFromCameraX5")]
    public static void FurtherFromCameraX4(MenuCommand menuCommand)
    {
        Transform _transform = menuCommand.context as Transform;
        _transform.position = new Vector3(_transform.position.x + X_STEP * 5, _transform.position.y - Y_STEP * 5, _transform.position.z + Z_STEP * 5);
    }
}
