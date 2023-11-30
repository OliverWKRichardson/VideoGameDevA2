using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectionBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public float decay;
    private float delay = 0.0f;

    public void AddDetection(float vis)
    {
        slider.value += vis;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        delay = 1.0f;
    }

    void Update()
    {
        if(delay <= 0.0f)
        {
            slider.value -= decay * Time.deltaTime;
        }
        else
        {
            delay -= Time.deltaTime;
        }
    }
}
