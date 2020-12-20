using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorColorSwapper : MonoBehaviour
{
    public Material mat;
    private Color[] mainColors;
    private int cur_idx;

    private void Start()
    {
        mainColors = new Color[3] { Color.blue, Color.green, Color.red };
        cur_idx = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Color next = Color.Lerp(mat.GetColor("_EmissionColor"), mainColors[cur_idx], Time.deltaTime / 2.0f);
        mat.SetColor("_EmissionColor", next);
        if (Dist(mat.GetColor("_EmissionColor"), mainColors[cur_idx]) < 0.2f)
        {
            cur_idx = (cur_idx + 1) % 3;
        }

    }

    private float Dist(Color a, Color b)
    {
        return Mathf.Abs(a.r - b.r) + Mathf.Abs(a.g - b.g) + Mathf.Abs(a.b - b.b);
    }
}
