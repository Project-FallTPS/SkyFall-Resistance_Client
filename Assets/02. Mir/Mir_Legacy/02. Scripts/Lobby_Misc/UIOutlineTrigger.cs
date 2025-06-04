using UnityEngine;
using UnityEngine.EventSystems;

public class UIOutlineTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public OutlineHighlighter highlighterTarget; // 연결할 대상

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (highlighterTarget != null)
            highlighterTarget.EnableOutline();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (highlighterTarget != null)
            highlighterTarget.DisableOutline();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (highlighterTarget != null)
            highlighterTarget.TriggerAction();
    }
}
