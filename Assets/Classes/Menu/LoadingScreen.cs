using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : Menu
{
    public Canvas loadingScreenCanvas;
    public Slider progressBar;
    public Image fadeImage;

    protected override void InitializeButtonFunction() { }

    public IEnumerator Fade(float target, float duration = 0.5f)
    {
        while(fadeImage.color.a != target)
        {
            float newAlpha = Mathf.MoveTowards(fadeImage.color.a, target, duration * Time.fixedDeltaTime);
            SetFadeAlpha(newAlpha);

            yield return null;
        }
    }

    public void SetFadeAlpha(float newAlpha)
    {
        Color color = new(0, 0, 0, newAlpha);
        fadeImage.color = color;
    }
}
