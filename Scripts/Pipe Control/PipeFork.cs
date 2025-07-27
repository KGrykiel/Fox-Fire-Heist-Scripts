using UnityEngine;

public class PipeFork : PipelineComponent
{
    public PipelineComponent nextComponent2;

    public override void SetFlow(bool isFlowing)
    {
        base.SetFlow(isFlowing);
        if (nextComponent2 != null)
        {
            nextComponent2.SetFlow(isFlowing);
        }
    }
}



