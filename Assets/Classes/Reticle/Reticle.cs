using UnityEngine;
using UnityEngine.UI;

public class Reticle : MonoBehaviour
{
    [SerializeField] private Image image;

    public void Toggle(bool toggle)
    {
        image.gameObject.SetActive(toggle);
        if (toggle)
        {
            
        }
    }

    public void ChangeColor(Color color)
    {
        image.color = color;
    }
}
