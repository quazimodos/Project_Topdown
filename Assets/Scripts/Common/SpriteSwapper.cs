using UnityEngine;
using UnityEngine.UI;

/// Utility class for swapping the sprite of a UI Image between two predefined ones representing enabled/disabled states.
public class SpriteSwapper : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField]
    private Sprite enabledSprite;

    [SerializeField]
    private Sprite disabledSprite;
#pragma warning restore 649

    private Image image;

    public void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SwapSprite()
    {
        image.sprite = image.sprite == enabledSprite ? disabledSprite : enabledSprite;
    }

    public void SetEnabled(bool spriteEnabled)
    {
        image.sprite = spriteEnabled ? enabledSprite : disabledSprite;
    }
}
