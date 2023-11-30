using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisibilityBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetVisibility(float vis)
    {
        slider.value = vis;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
