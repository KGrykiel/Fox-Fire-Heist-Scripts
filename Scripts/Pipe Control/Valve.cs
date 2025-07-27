using UnityEngine;

public class Valve : PipelineComponent
{
    public bool isOpen = false;

    public override void SetFlow(bool isFlowing)
    {
        this.isFlowing = isFlowing;
        ChangeColor(isFlowing);
        if (nextComponent != null)
        {
            nextComponent.SetFlow(this.isFlowing && isOpen);
        }
    }

    public void ToggleValve(bool open)
    {
        if (open == isOpen)
        {
            return;
        }
        isOpen = open;
        SetFlow(isFlowing);
    }
}
