using UnityEngine;

public class UI_OptionPanel : UI_Popup
{
    public void OnClickBackButton()
    {
        LobbySceneManager.Instance.CloseUI();
    }
}
