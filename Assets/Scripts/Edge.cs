using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Edge : MonoBehaviour
{
    private LineRenderer lineRenderer;

    //Associated with node
    public Transform from, to;
    public string _info;

    //Assocaited with color
    GradientColorKey[] gradientColorKeys;
    GradientAlphaKey[] gradientAlphaKeys;
    Gradient gradient;
    public float colorTime;

    // Start is called before the first frame update
    void Start()
    {
        gradient = new Gradient();
        lineRenderer = GetComponent<LineRenderer>();
        gradientAlphaKeys = lineRenderer.colorGradient.alphaKeys;
    }

    // Update is called once per frame
    void Update()
    {
        DrawRadiusCurve();
        PointCurve();
        if (Config.Instance.mode == 2||Config.Instance.mode==3)
        {
            DisappearLine();
        }
        else
            lineRenderer.enabled = true;
    }

    void DisappearLine()
    {
        if ((from.GetComponent<Node>().selected || from.GetComponent<Node>().grandSelected) && (to.GetComponent<Node>().selected || to.GetComponent<Node>().grandSelected))
            lineRenderer.enabled = true;
        else lineRenderer.enabled = false;
        //Debug.Log(name + ":" + lineRenderer.enabled);
    }

    void DrawRadiusCurve() {
        lineRenderer.positionCount = Config.Instance.lineSegment + 1;
        for (int i = 0; i <= Config.Instance.lineSegment; i++) {
            float t = i / (float)Config.Instance.lineSegment;
            Vector3 thisPoint = ((1 - t) * from.transform.position + t * to.transform.position).normalized * Config.Instance.skyboxRadius;
            lineRenderer.SetPosition(i, thisPoint);
        }
    }

    void PointCurve() {
        
        if (from.GetComponent<Node>().infected)
        {
            colorTime = from.GetComponent<Node>().infectivity - to.GetComponent<Node>().immunity;
            gradientColorKeys = new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.red, colorTime), new GradientColorKey(Color.white, 1.0f) };
            gradient.SetKeys(gradientColorKeys, gradientAlphaKeys);
            lineRenderer.colorGradient = gradient;
            if (colorTime > 0.7f)
                to.GetComponent<Node>().infected = true;
        }
        
    }
}
