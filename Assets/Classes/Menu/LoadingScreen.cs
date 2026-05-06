using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : Menu
{
    public Canvas loadingScreenCanvas;
    public Slider progressBar;
    public Image fadeImage;

    public const float FADE_DURATION = 0.5f;
    public const float FADE_OUT_DURATION = 0.5f;

    protected override void InitializeButtonFunction() { }

    public IEnumerator Fade(float target)
    {
        while(fadeImage.color.a != target)
        {
            float newAlpha = Mathf.MoveTowards(fadeImage.color.a, target, FADE_DURATION * Time.fixedDeltaTime);
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
