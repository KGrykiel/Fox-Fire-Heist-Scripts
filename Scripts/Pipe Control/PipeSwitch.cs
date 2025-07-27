using UnityEngine;

public class PipeSwitch : PipelineComponent
{
    public PipelineComponent nextComponentUp;
    public PipelineComponent nextComponentLeft;
    public PipelineComponent nextComponentDown;
    public PipelineComponent nextComponentRight;


    public void UpdateNext(int id)
    {
        if (nextComponent != null)
            nextComponent.SetFlow(false);
        switch (id)
        {
            case 0:
                nextComponent = null;
                break;
            case 1:
                nextComponent = nextComponentUp;
                nextComponent.SetFlow(isFlowing);
                break;
            case 2:
                nextComponent = nextComponentLeft;
                nextComponent.SetFlow(isFlowing);
                break;
            case 3:
                nextComponent = nextComponentDown;
                nextComponent.SetFlow(isFlowing);
                break;
            case 4:
                nextComponent = nextComponentRight;
                nextComponent.SetFlow(isFlowing);
                break;
            default:
                Debug.LogWarning("Invalid ID. Please use an ID from 1 to 4.");
                break;
        }
    }
}



