using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIOptionPopUP : MonoBehaviour
{
    [SerializeField] private UI_Popup _optionPanel;
    private Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    private bool _isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_popupStack.Count > 0)
                CloseTopUI();
            else
                OpenOptionUI();
        }
    }

    public void OpenOptionUI()
    {
        if (_optionPanel == null) return;

        _optionPanel.Open(() =>
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            DOTween.To(() => AudioListener.volume, x => AudioListener.volume = x, 0f, 0.3f);
            Time.timeScale = 0f;
            AudioListener.pause = true;
            _isPaused = true;
        });

        _popupStack.Push(_optionPanel);
    }

    public void CloseTopUI()
    {
        if (_popupStack.Count == 0) return;

        UI_Popup topUI = _popupStack.Pop();
        topUI.Close(() =>
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            DOTween.To(() => AudioListener.volume, x => AudioListener.volume = x, 1f, 0.3f);
            Time.timeScale = 1f;
            AudioListener.pause = false;
            _isPaused = false;
        });
    }

    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
