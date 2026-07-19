using UnityEditor;
using UnityEngine;

public class SpriteRenamer : EditorWindow
{
    private string baseName = "";
    private int startIndex = 0;

    [MenuItem("Tools/Sprite Renamer")]
    private static void ShowWindow()
    {
        GetWindow<SpriteRenamer>("Sprite Renamer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Batch Rename Sprites", EditorStyles.boldLabel);

        baseName = EditorGUILayout.TextField("Base Name", baseName);
        startIndex = EditorGUILayout.IntField("Start Index", startIndex);

        GUILayout.Space(10);

        if (GUILayout.Button("Rename Selected Sprites"))
        {
            RenameSprites();
        }
    }

    private void RenameSprites()
    {
        Object[] selectedObjects = Selection.objects;

        int currentIndex = startIndex;

        foreach (Object obj in selectedObjects)
        {
            obj.name = $"{baseName}{currentIndex}";
            currentIndex++;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}