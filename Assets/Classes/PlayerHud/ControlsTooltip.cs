using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlsTooltip : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> textboxes = new();
    public Slider djSlider;

    public void UpdateTextbox(int index, string text)
    {
        textboxes[index].text = text;
    }

    public void ToggleTextbox(int index, bool toggle)
    {
        textboxes[index].transform.parent.gameObject.SetActive(toggle);
    }

    public void ToggleAllTextboxes(bool toggle)
    {
        foreach(TextMeshProUGUI textbox in textboxes)
        {
            textbox.transform.parent.gameObject.SetActive(toggle);
        }
    }
}
