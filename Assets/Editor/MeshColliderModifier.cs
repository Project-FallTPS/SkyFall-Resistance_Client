using UnityEngine;
using UnityEditor;

public class MeshColliderModifier : EditorWindow
{
    [MenuItem("Tools/MeshCollider Modifier")]
    public static void ShowWindow()
    {
        GetWindow<MeshColliderModifier>("MeshCollider Modifier");
    }

    private void OnGUI()
    {
        GUILayout.Label("선택한 오브젝트 하위의 MeshCollider 설정 변경", EditorStyles.boldLabel);

        if (GUILayout.Button("Convex 및 Provides Contacts 활성화"))
        {
            ModifySelectedObjects();
        }
    }

    private void ModifySelectedObjects()
    {
        var selected = Selection.gameObjects;

        if (selected.Length == 0)
        {
            Debug.LogWarning("오브젝트를 하나 이상 선택해주세요.");
            return;
        }

        int modifiedCount = 0;

        foreach (GameObject go in selected)
        {
            MeshCollider[] meshColliders = go.GetComponentsInChildren<MeshCollider>(true);

            foreach (MeshCollider mc in meshColliders)
            {
                Undo.RecordObject(mc, "Modify MeshCollider");

                mc.convex = true;

#if UNITY_2022_2_OR_NEWER
                mc.providesContacts = true;
#endif

                modifiedCount++;
                EditorUtility.SetDirty(mc);
            }
        }

        Debug.Log($"{modifiedCount}개의 MeshCollider가 수정되었습니다.");
    }
}
