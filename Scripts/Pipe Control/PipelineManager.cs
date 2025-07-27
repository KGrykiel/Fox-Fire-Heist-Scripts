using UnityEngine;
using System.Collections.Generic;

public class PipelineManager : MonoBehaviour
{
    public List<PipelineComponent> startComponents = new List<PipelineComponent>();
    public PipelineComponent newSource_idc;

    private void Start()
    {
        StartFlow();
    }

    public void StartFlow()
    {
        foreach (var component in startComponents)
        {
            if (component != null)
            {
                component.SetFlow(true);
            }
        }
    }

    public void StopFlow()
    {
        foreach (var component in startComponents)
        {
            if (component != null)
            {
                component.SetFlow(false);
            }
        }
    }

    public void addSource()
    {
        startComponents.Add(newSource_idc);
        newSource_idc.SetFlow(true);
    }

}









