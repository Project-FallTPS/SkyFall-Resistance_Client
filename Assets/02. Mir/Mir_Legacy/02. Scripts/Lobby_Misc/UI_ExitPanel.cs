using UnityEngine;

public class UI_ExitPanel : UI_Popup
{
    public void OnClickExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();  // 빌드된 상태에선 게임 종료
#endif
    }

    public void OnClickCancelButton()
    {
        LobbySceneManager.Instance.CloseUI();
    }
}
