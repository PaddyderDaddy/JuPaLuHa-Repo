using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class EllipseRenderer : MonoBehaviour
{
    //public LineRenderer MyLineRenderer;

    //[Range(3, 36)]
    //public int Segments;
    //public Ellipse Ellipse;

    //void Awake()
    //{
    //    MyLineRenderer = GetComponent<LineRenderer>();
    //    CalculateEllipse();
    //}

    //void CalculateEllipse()
    //{
    //    Vector3[] points = new Vector3[Segments + 1];
    //    for (int i = 0; i < Segments; i++)
    //    {
    //        Vector2 position2D = Ellipse.Evaluate((float)i / (float)Segments);
    //        points[i] = new Vector3(position2D.x, position2D.y, 0f);
    //    }
    //    points[Segments] = points[0];

    //    MyLineRenderer.positionCount = Segments + 1;
    //    MyLineRenderer.SetPositions(points);
    //}

    //void OnValidate()
    //{
    //    if (Application.isPlaying)
    //        CalculateEllipse();
    //}

    public LineRenderer MyLineRenderer;

    [Range(3, 36)]
    public int Segments;
    public float xAxis = 5f;
    public float yAxis = 3f;

    void Awake()
    {
        MyLineRenderer = GetComponent<LineRenderer>();
        CalculateEllipse();
    }

    void CalculateEllipse()
    {
        Vector3[] points = new Vector3[Segments + 1];
        for (int i = 0; i < Segments; i++)
        {
            float angle = ((float)i / (float)Segments) * 360 * Mathf.Deg2Rad;
            float x = Mathf.Sin(angle) * xAxis;
            float y = Mathf.Cos(angle) * yAxis;
            points[i] = new Vector3(x, y, 0f);
        }
        points[Segments] = points[0];

        MyLineRenderer.positionCount = Segments + 1;
        MyLineRenderer.SetPositions(points);
    }

    void OnValidate()
    {
        if (Application.isPlaying)
            CalculateEllipse();
    }
}
