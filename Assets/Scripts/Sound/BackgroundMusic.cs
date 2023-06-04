using UnityEngine;

/// This class manages the background music of the game.
[RequireComponent(typeof(AudioSource))]
public class BackgroundMusic : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (PlayerPrefs.HasKey("music_enabled"))
        {
            var musicEnabled = PlayerPrefs.GetInt("music_enabled");
            if (musicEnabled == 0)
                GetComponent<AudioSource>().mute = true;
        }
    }
}
