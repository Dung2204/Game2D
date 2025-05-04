using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderAuto : MonoBehaviour
{
    private Renderer parent;
    private Renderer own;

    public void init(Renderer target)
    {
        parent = target;
        own = this.transform.GetComponent<Renderer>();
    }

    private void FixedUpdate()
    {
        if (parent.sortingOrder - 1 != own.sortingOrder)
        {
            own.sortingOrder = parent.sortingOrder - 1;
        }
    }
}
