using UnityEngine;
using UnityEngine.UI;

public class CrosshairManager : MonoBehaviour
{
    public Image crosshairImage;
    public Sprite defaultCrosshair;
    public Sprite interactableCrosshair;
    public Sprite holdingCrosshair;
    public Sprite noteCrosshair;
    public Vector2 defaultCrosshairSize = new Vector2(50, 50); // Default size
    public Vector2 interactableCrosshairSize = new Vector2(70, 70); // Interactable size
    public Vector2 holdingCrosshairSize = new Vector2(50, 50); // Holding size
    public Vector2 noteCrosshairSize = new Vector2(50, 50); // Note size

    public void SetDefaultCrosshair()
    {
        crosshairImage.sprite = defaultCrosshair;
        crosshairImage.rectTransform.sizeDelta = defaultCrosshairSize;
    }

    public void SetInteractableCrosshair()
    {
        crosshairImage.sprite = interactableCrosshair;
        crosshairImage.rectTransform.sizeDelta = interactableCrosshairSize;
    }

    public void SetHoldingCrosshair()
    {
        crosshairImage.sprite = holdingCrosshair;
        crosshairImage.rectTransform.sizeDelta = holdingCrosshairSize;
    }

    public void SetNoteCrosshair()
    {
        crosshairImage.sprite = noteCrosshair;
        crosshairImage.rectTransform.sizeDelta = noteCrosshairSize;
    }


}
