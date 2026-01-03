using UnityEngine;

namespace ModStructureHelperPlugin.Editing.Managers;

public class CursorOverrideManager : MonoBehaviour
{
    [SerializeField] private Texture2D downwardDiagonal;
    [SerializeField] private Texture2D upwardDiagonal;
    [SerializeField] private Texture2D horizontal;
    [SerializeField] private Texture2D vertical;

    public void SetCursor(CustomCursor cursor)
    {
        var cursorTexture = GetCursor(cursor);
        Cursor.SetCursor(cursorTexture, cursorTexture == null ? Vector2.zero : new Vector2(31, 31), CursorMode.Auto);
    }

    private Texture2D GetCursor(CustomCursor cursor)
    {
        return cursor switch
        {
            CustomCursor.DownwardDiagonal => downwardDiagonal,
            CustomCursor.UpwardDiagonal => upwardDiagonal,
            CustomCursor.Horizontal => horizontal,
            CustomCursor.Vertical => vertical,
            _ => null
        };
    }

    public enum CustomCursor
    {
        None,
        DownwardDiagonal,
        UpwardDiagonal,
        Horizontal,
        Vertical
    }
}