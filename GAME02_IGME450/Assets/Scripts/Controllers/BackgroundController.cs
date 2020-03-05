using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class BackgroundController : MonoBehaviour
{

    /// <summary>
    /// Image component used for the background.
    /// </summary>
    [SerializeField, Required]
    private Image image; 

    /// <summary>
    /// Color tint for background.
    /// </summary>
    [SerializeField]
    private Color tint;

    /// <summary>
    /// Validate application of tint.
    /// </summary>
    [ExecuteInEditMode]
    public void OnValidate() => this.ApplyTint();

    /// <summary>
    /// Apply the tint if it's different than the current.
    /// </summary>
    public void ApplyTint()
    {
        if (this.image)
        {
            this.image.color = tint;
        }
    }

    /// <summary>
    /// Set the background.
    /// </summary>
    /// <param name="background">Background to set.</param>
    /// <param name="tint">Tint to set.</param>
    public void SetBackground(Sprite background, Color? tint = null)
    {
        if (this.image)
        {
            this.image.sprite = background ?? this.image.sprite;
            this.tint = tint.HasValue ? tint.Value : this.tint;
            this.ApplyTint();
        }
    }


}
