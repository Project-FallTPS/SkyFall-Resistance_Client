using System;
using System.Collections;
using UnityEngine;

public class BossEntryCutSceneLogic : MonoBehaviour
{
    private bool isCutSceneFinished = false;

    private void Start()
    {
        StartCoroutine(SceneTransitionManager.Instance.LoadSceneAsync(
            nameof(ESceneNames.BossScene),
            () => isCutSceneFinished, // PlayCutscene() 코루틴 종료후 true로 바꿔야 씬 전환 진행
            () => Debug.Log("보스씬 로딩 중..."),
            () => Debug.Log("보스씬 로딩 완료, 컷씬이 끝나기를 기다리는 중")
        ));

        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        // 컷씬 재생
        yield return new WaitForSeconds(5f);
        isCutSceneFinished = true;
    }
}
