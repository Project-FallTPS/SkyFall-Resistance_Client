using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : Singleton<SceneTransitionManager>
{
    
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadScene(ESceneNames sceneName)
    {
        SceneManager.LoadScene(nameof(sceneName));
    }

    public IEnumerator LoadSceneAsync(int sceneIndex, 
        Func<bool> canActivateScene = null, Action onLoading = null, Action onComplete = null)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        if (asyncOperation != null)
        {
            asyncOperation.allowSceneActivation = false;

            while (!asyncOperation.isDone)
            {
                onLoading?.Invoke();

                if (0.9f <= asyncOperation.progress)
                {
                    onComplete?.Invoke();

                    // 외부 조건이 true가 되어야 씬 전환 진행
                    if (canActivateScene != null && canActivateScene())
                    {
                        asyncOperation.allowSceneActivation = true;
                    }
                }
                yield return null;
            }
        }
    }

    public IEnumerator LoadSceneAsync(string sceneName, 
        Func<bool> canActivateScene = null, Action onLoading = null, Action onComplete = null)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        if (asyncOperation != null)
        {
            asyncOperation.allowSceneActivation = false;

            while (!asyncOperation.isDone)
            {
                onLoading?.Invoke();

                if (0.9f <= asyncOperation.progress)
                {
                    onComplete?.Invoke();

                    // 외부 조건이 true가 되어야 씬 전환 진행
                    if (canActivateScene != null && canActivateScene())
                    {
                        asyncOperation.allowSceneActivation = true;
                    }
                }
                yield return null;
            }
        }
    }

    public IEnumerator LoadSceneAsync(ESceneNames sceneName, 
        Func<bool> canActivateScene = null, Action onLoading = null, Action onComplete = null)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(nameof(sceneName));
        if (asyncOperation != null)
        {
            asyncOperation.allowSceneActivation = false;

            while (!asyncOperation.isDone)
            {
                onLoading?.Invoke();

                if (0.9f <= asyncOperation.progress)
                {
                    onComplete?.Invoke();

                    // 외부 조건이 true가 되어야 씬 전환 진행
                    if (canActivateScene != null && canActivateScene())
                    {
                        asyncOperation.allowSceneActivation = true;
                    }
                }
                yield return null;
            }
        }
    }
}
