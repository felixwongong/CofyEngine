using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ProgressBarElement : VisualElement
{
    public new class UxmlFactory: UxmlFactory<ProgressBarElement>{ }

    private VisualElement _fill;
    private VisualElement fill => _fill ?? this.Q("fill");

    public float value
    {
        set
        {
            if (Mathf.Approximately(fill.transform.scale.x, value)) return;

            fill.transform.scale.Set(value, fill.transform.scale.y, fill.transform.scale.z);
        }
    }
    
    public ProgressBarElement() {}
}
