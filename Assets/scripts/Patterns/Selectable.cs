using System;
using UnityEngine;
using static WindowInfo;

public abstract class Selectable : MonoBehaviour
{
    new Renderer renderer;
    Color defaultColor;

    public virtual void Start()
    {
        renderer = GetComponent<Renderer>();
        if (renderer == null)
            renderer = GetComponentInChildren<Renderer>();
        defaultColor = renderer.material.color;
    }
    public virtual void Selected()
    {
        renderer.material.color = new Color(defaultColor.r, defaultColor.g + 70, defaultColor.b);
    }

    public virtual void Deselected()
    {
        renderer.material.color = defaultColor;
    }

    public virtual ICommand Clicked()
    {
        // should never be reached
        throw new NotImplementedException();
    }
}