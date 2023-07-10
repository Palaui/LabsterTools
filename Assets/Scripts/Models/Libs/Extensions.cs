using UnityEngine;

public static class Extensions
{
    public static void SetOnGrid(this Transform transform)
    {
        Vector3 pos = transform.position / EditorConstants.GRID_SIZE;
        transform.position = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z)) * EditorConstants.GRID_SIZE;
        Vector3 euler = transform.eulerAngles / EditorConstants.RECT;
        euler.y = Mathf.Round(euler.y) * EditorConstants.RECT;
        transform.eulerAngles = new Vector3(0, euler.y, 0);
    }

    public static bool IsOnGrid(this Transform transform)
    {
        Vector3 pos = transform.position / EditorConstants.GRID_SIZE;
        Vector3 euler = transform.eulerAngles / EditorConstants.RECT;
        return Mathf.Abs(pos.x - Mathf.Round(pos.x)) < EditorConstants.EPSILON &&
            Mathf.Abs(pos.y - Mathf.Round(pos.y)) < EditorConstants.EPSILON &&
            Mathf.Abs(pos.z - Mathf.Round(pos.z)) < EditorConstants.EPSILON &&
            euler.x == 0 &&
            Mathf.Abs(euler.y - Mathf.Round(euler.y)) < EditorConstants.EPSILON &&
            euler.z == 0;
    }
}