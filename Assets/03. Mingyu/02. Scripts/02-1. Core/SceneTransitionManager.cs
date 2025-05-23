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

    public IEnumerator LoadSceneAsync(int sceneIndex, Action onLoading = null, Action onComplete = null)
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
                    asyncOperation.allowSceneActivation = true;
                }

                yield return null;
            }
        }
    }

    public IEnumerator LoadSceneAsync(string sceneName, Action onLoading = null, Action onComplete = null)
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
                    asyncOperation.allowSceneActivation = true;
                }
                yield return null;
            }
        }
    }
}
