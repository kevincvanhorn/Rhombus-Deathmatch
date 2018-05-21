using UnityEngine;

public class ExampleClass : MonoBehaviour
{
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, Vector3.zero);
    }

    void Update()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        lineRenderer.SetPosition(1, mousePosition);
    }
}