using UnityEngine.Events;

public class Socket : PipelineComponent
{

    public UnityEvent onPowerOn = new UnityEvent();
    public UnityEvent onPowerOff = new UnityEvent();

    public override void SetFlow(bool isFlowing)
    {
        if (this.isFlowing != isFlowing)
        {
            this.isFlowing = isFlowing;
            ChangeColor(isFlowing);
            onFlowChanged.Invoke(isFlowing);

            if (isFlowing)
            {
                onPowerOn.Invoke();
            }
            else
            {
                onPowerOff.Invoke();
            }
        }
    }
}







