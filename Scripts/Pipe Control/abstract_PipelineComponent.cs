using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public abstract class PipelineComponent : MonoBehaviour
{
    public PipelineComponent nextComponent;
    private Renderer componentRenderer;
    public bool isFlowing;
    [HideInInspector]
    public UnityEvent<bool> onFlowChanged = new UnityEvent<bool>();
    private float flowDelay = 0.2f; // Delay before setting the flow

    void Start()
    {
        componentRenderer = GetComponent<Renderer>();
        if (componentRenderer == null)
        {
            Debug.LogWarning("No Renderer component found on " + gameObject.name);
        }
    }

    public virtual void SetFlow(bool isFlowing)
    {
        StartCoroutine(SetFlowWithDelay(isFlowing));
    }

    private IEnumerator SetFlowWithDelay(bool isFlowing)
    {
        yield return new WaitForSeconds(flowDelay);

        if (this.isFlowing != isFlowing)
        {
            this.isFlowing = isFlowing;
            //ChangeColor(isFlowing);
            onFlowChanged.Invoke(isFlowing);
        }

        if (nextComponent != null)
        {
            nextComponent.SetFlow(isFlowing);
        }
    }

    protected void ChangeColor(bool isFlowing)
    {
        //if (componentRenderer != null)
        //{
        //    componentRenderer.material.color = isFlowing ? Color.red : Color.white;
        //}
    }
}






