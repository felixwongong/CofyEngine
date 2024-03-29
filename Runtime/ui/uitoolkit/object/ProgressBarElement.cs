using CofyEngine;
using UnityEngine;
using UnityEngine.UIElements;

public class ProgressBarElement: UIElement
{
    private VisualElement fill;
    
    public float value
    {
        set
        {
            if (Mathf.Approximately(fill.transform.scale.x, value)) return;

            fill.transform.scale.Set(value, fill.transform.scale.y, fill.transform.scale.z);
        }
    }

    public ProgressBarElement()
    {
    }

    protected internal override void Construct(VisualElement el)
    {
        fill = el.Q("fill");
    }
}