using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class SettingsPopup : Popup
{
#pragma warning disable 649
    [SerializeField]
    private Slider soundSlider;

    [SerializeField]
    private Slider musicSlider;
#pragma warning restore 649

    private int currentSound;
    private int currentMusic;

    protected override void Awake()
    {
        base.Awake();
        Assert.IsNotNull(soundSlider);
        Assert.IsNotNull(musicSlider);
    }

    protected override void Start()
    {
        base.Start();
        soundSlider.value = PlayerPrefs.GetInt("sound_enabled");
        musicSlider.value = PlayerPrefs.GetInt("music_enabled");
    }

    public void OnSoundSliderValueChanged()
    {
        currentSound = (int)soundSlider.value;
        SoundPlayer.SetSoundEnabled(currentSound == 1);
        PlayerPrefs.SetInt("sound_enabled", currentSound);
        var homeScreen = ParentScreen as HomeScreen;
        homeScreen.UpdateButtons();
    }

    public void OnMusicSliderValueChanged()
    {
        currentMusic = (int)musicSlider.value;
        SoundPlayer.SetMusicEnabled(currentMusic == 1);
        PlayerPrefs.SetInt("music_enabled", currentMusic);
        var homeScreen = ParentScreen as HomeScreen;
        homeScreen.UpdateButtons();
    }
}
