﻿using UnityEngine;
using UnityEngine.Assertions;

/// Static utility class that provides easy access to the current scene's sound system.
public static class SoundPlayer
{
    private static SoundSystem soundSystem;

    public static void Initialize()
    {
        soundSystem = Object.FindObjectOfType<SoundSystem>();
        Assert.IsNotNull(soundSystem);
    }

    public static void PlaySoundFx(string soundName)
    {
        soundSystem.PlaySoundFx(soundName);
    }

    public static void StopSoundFx(string soundName)
    {
        soundSystem.StopSoundFx(soundName);
    }

    public static void SetSoundEnabled(bool soundEnabled)
    {
        soundSystem.SetSoundEnabled(soundEnabled);
    }

    public static void SetMusicEnabled(bool musicEnabled)
    {
        soundSystem.SetMusicEnabled(musicEnabled);
    }
}
