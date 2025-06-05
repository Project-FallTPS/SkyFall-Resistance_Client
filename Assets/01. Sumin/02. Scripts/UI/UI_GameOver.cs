using UnityEngine;
using DG.Tweening;

public class UI_GameOver : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _gameClearPanel;

    private CanvasGroup _gameOverCanvasGroup;
    private CanvasGroup _gameClearCanvasGroup;

    private void Awake()
    {
        UIEventHandler.Instance.OnPlayerDie += GameOver;
        UIEventHandler.Instance.OnBossDie += GameClear;

        _gameOverCanvasGroup = SetupCanvasGroup(_gameOverPanel);
        _gameClearCanvasGroup = SetupCanvasGroup(_gameClearPanel);
    }

    private CanvasGroup SetupCanvasGroup(GameObject panel)
    {
        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        if (cg == null) cg = panel.AddComponent<CanvasGroup>();
        cg.alpha = 0;
        cg.transform.localScale = Vector3.zero;
        panel.SetActive(false);
        return cg;
    }

    private void GameOver()
    {
        AnimatePanel(_gameOverPanel, _gameOverCanvasGroup);
    }

    private void GameClear()
    {
        AnimatePanel(_gameClearPanel, _gameClearCanvasGroup);
    }

    private void AnimatePanel(GameObject panel, CanvasGroup canvasGroup)
    {
        panel.SetActive(true);
        canvasGroup.DOFade(1f, 1.5f).SetEase(Ease.OutQuad);
        canvasGroup.transform.DOScale(Vector3.one, 1.5f).SetEase(Ease.OutBack);
    }
}
