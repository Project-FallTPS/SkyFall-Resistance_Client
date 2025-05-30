using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TowerDataEditor : EditorWindow //추후에 수정할거니까 건들지 마쇼 -> 수민
{

    /*private TowerDataCollection collection;
    private Vector2 scrollPos;

    private const string FileName = "Tower/TowerDataCollection"; // Resources/Json/Tower/TowerDataCollection.json

    [MenuItem("Tools/Tower Data Editor")]
    public static void ShowWindow()
    {
        GetWindow<TowerDataEditor>("Tower Data Editor");
    }

    private void OnEnable()
    {
        try
        {
            collection = JsonDataManager.LoadFromFile<TowerDataCollection>(FileName);
            if (collection.Datas == null)
                collection.Datas = new List<TowerData>();
        }
        catch
        {
            Debug.LogWarning("TowerDataCollection 파일이 없어서 새로 생성합니다.");
            collection = new TowerDataCollection();
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Tower Data Editor", EditorStyles.boldLabel);
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int i = 0; i < collection.Datas.Count; i++)
        {
            var tower = collection.Datas[i];
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.LabelField($"Tower {i + 1}", EditorStyles.boldLabel);
            tower.TowerType = (ETowerType)EditorGUILayout.EnumPopup("Tower Type", tower.TowerType);
            tower.TypeString = EditorGUILayout.TextField("Type String", tower.TypeString);
            tower.MaxHp = EditorGUILayout.FloatField("Max HP", tower.MaxHp);
            tower.Damage = EditorGUILayout.FloatField("Damage", tower.Damage);
            tower.AtkSpeed = EditorGUILayout.FloatField("Attack Speed", tower.AtkSpeed);
            tower.Range = EditorGUILayout.FloatField("Range", tower.Range);
            tower.BuildTime = EditorGUILayout.FloatField("BuildTime", tower.BuildTime);
            tower.PopulationMultiplier = EditorGUILayout.FloatField("Population Multiplier", tower.PopulationMultiplier);

            // Cost 리스트
            EditorGUILayout.LabelField("Cost", EditorStyles.boldLabel);
            if (tower.Cost == null)
                tower.Cost = new List<TowerCostData>();

            for (int j = 0; j < tower.Cost.Count; j++)
            {
                var cost = tower.Cost[j];
                EditorGUILayout.BeginHorizontal();

                cost.Type = (ResourceType)EditorGUILayout.EnumPopup("Resource", cost.Type);
                cost.Amount = EditorGUILayout.IntField("Amount", cost.Amount);

                if (GUILayout.Button("삭제", GUILayout.Width(50)))
                {
                    tower.Cost.RemoveAt(j);
                    break;
                }

                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("비용 추가"))
            {
                tower.Cost.Add(new TowerCostData(ResourceType.Stone, 0));
            }

            GUILayout.Space(5);
            if (GUILayout.Button("타워 삭제"))
            {
                collection.Datas.RemoveAt(i);
                break;
            }

            EditorGUILayout.EndVertical();
            GUILayout.Space(10);
        }

        EditorGUILayout.EndScrollView();
        GUILayout.Space(10);

        if (GUILayout.Button("새로운 타워 추가"))
        {
            collection.Datas.Add(new TowerData());
        }

        if (GUILayout.Button("JSON으로 저장"))
        {
            JsonDataManager.CreateFile(FileName, collection);
            Debug.Log("TowerDataCollection 저장 완료!");
            AssetDatabase.Refresh();
        }
    }
    */
}
