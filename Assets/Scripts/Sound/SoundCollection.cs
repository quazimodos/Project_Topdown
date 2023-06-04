using System.Collections.Generic;
using UnityEngine;

/// A collection of sounds. Use two collections in the game: one for the menu sounds and another for the game sounds.
[CreateAssetMenu(fileName = "SoundCollection", menuName = "TopdownShooter/Sound collection", order = 2)]
public class SoundCollection : ScriptableObject
{
    public List<AudioClip> Sounds;
}
