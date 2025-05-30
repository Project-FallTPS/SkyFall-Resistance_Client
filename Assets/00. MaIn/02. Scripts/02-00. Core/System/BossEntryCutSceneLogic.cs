using System;
using UnityEngine;

public class BossEntryCutSceneLogic : MonoBehaviour
{
    
    private void Awake()
    {
        
    }

    private void Start()
    {
        StartCoroutine(SceneTransitionManager.Instance.LoadSceneAsync(ESceneNames.BossEntryCutScene));
    }
}
