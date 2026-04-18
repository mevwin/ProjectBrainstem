using UnityEngine;

public class AthleteLineRenderer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private LineRenderer lineRenderer;
    private Vector3 p1Offset = new(0, 0.5f, 0f);

    public void UpdateLine(Vector3 endPoint, Color lineColor)
    {
        lineRenderer.SetPosition(0, playerTransform.position - p1Offset);
        lineRenderer.SetPosition(1, endPoint);
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
    }

}