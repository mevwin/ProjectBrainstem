using System.Collections;
using UnityEngine;

public class WorldSpaceText : Interactable, ITriggerListener
{
    [SerializeField] private GameObject canvasParent;
    [SerializeField] private float fadeDuration = 0.2f;

    public override void Start()
    {
        foreach (Transform transform in canvasParent.transform)
        {

                transform.gameObject.GetComponent<CanvasRenderer>().SetAlpha(0f);
  
        }   
    }

    protected override void InitializeStates() { }

    public virtual void OnTriggerEvent(TriggerEventType eventType)
    {
        isActive = eventType == TriggerEventType.Activated;

        float target = isActive ? 1f : 0f;
        foreach (Transform transform in canvasParent.transform)
        {
            StartCoroutine(
                Fade(transform.gameObject.GetComponent<CanvasRenderer>(), target, fadeDuration)
            );
        }   
    }

    private IEnumerator Fade(CanvasRenderer canvasRenderer, float target, float duration = 0.5f)
    {
        while(canvasRenderer.GetAlpha() != target)
        {
            float newAlpha = Mathf.MoveTowards(canvasRenderer.GetAlpha(), target, duration * Time.fixedDeltaTime);
            canvasRenderer.SetAlpha(newAlpha);

            yield return null;
        }
    }
}
