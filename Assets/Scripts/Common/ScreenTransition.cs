using UnityEngine;

/// Helper component to transition from one scene to another.
public class ScreenTransition : MonoBehaviour
{
    public string Scene = "<Insert scene name>";
    public float Duration = 1.0f;
    public Color Color = Color.black;

    public void PerformTransition()
    {
        Transition.LoadLevel(Scene, Duration, Color);
    }
}
