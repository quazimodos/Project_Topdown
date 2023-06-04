using UnityEngine;

/// This class manages the high-level logic of the home screen.
public class HomeScreen : BaseScreen
{
#pragma warning disable 649
    [SerializeField]
    private GameObject bgMusicPrefab;

    [SerializeField]
    private AnimatedButton musicButton;

    [SerializeField]
    private AnimatedButton soundButton;
#pragma warning restore 649

    protected override void Start()
    {
        base.Start();
        DatabaseManager.Instance.CreateUser();
        var bgMusic = FindObjectOfType<BackgroundMusic>();
        if (bgMusic == null)
            Instantiate(bgMusicPrefab);
        UpdateButtons();
    }

    public void OnSettingsButtonPressed()
    {
        OpenPopup<SettingsPopup>("Popups/SettingsPopup");
    }

    public void OnMusicButtonPressed()
    {
        var currentMusic = PlayerPrefs.GetInt("music_enabled");
        currentMusic = 1 - currentMusic;
        SoundPlayer.SetMusicEnabled(currentMusic == 1);
        PlayerPrefs.SetInt("music_enabled", currentMusic);
    }

    public void OnSoundButtonPressed()
    {
        var currentSound = PlayerPrefs.GetInt("sound_enabled");
        currentSound = 1 - currentSound;
        SoundPlayer.SetSoundEnabled(currentSound == 1);
        PlayerPrefs.SetInt("sound_enabled", currentSound);
    }

    public void UpdateButtons()
    {
        var music = PlayerPrefs.GetInt("music_enabled");
        musicButton.GetComponent<SpriteSwapper>().SetEnabled(music == 1);
        var sound = PlayerPrefs.GetInt("sound_enabled");
        soundButton.GetComponent<SpriteSwapper>().SetEnabled(sound == 1);
    }
}
