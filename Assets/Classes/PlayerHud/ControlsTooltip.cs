using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControlsTooltip : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> textboxes = new();

    public void UpdateTextbox(int index, string text)
    {
        textboxes[index].text = text;
    }

    public void ToggleTextbox(int index, bool toggle)
    {
        textboxes[index].gameObject.SetActive(toggle);
    }

    public void ToggleAllTextboxes(bool toggle)
    {
        foreach(TextMeshProUGUI textbox in textboxes)
        {
            textbox.gameObject.SetActive(toggle);
        }
    }
}
