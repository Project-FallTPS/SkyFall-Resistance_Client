using UnityEngine;
using UnityEngine.UI;

public class TempDragImageUI : MonoBehaviour
{
    public Image Icon;

    private void Awake()
    {
        Icon = GetComponent<Image>();
    }
}
