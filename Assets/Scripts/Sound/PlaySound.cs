using UnityEngine;

/// Helper component to play a sound effect using the sound player. It is used in all the popups and buttons of the game.
public class PlaySound : MonoBehaviour
{
    public void Play(string soundName)
    {
        SoundPlayer.PlaySoundFx(soundName);
    }

    public void Stop(string soundName)
    {
        SoundPlayer.StopSoundFx(soundName);
    }
}
