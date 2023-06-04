using Unity.VisualScripting;
using UnityEngine;
/// This class contains the logic associated to the in-game pause popup.
public class PausePopup : Popup
{
    protected override void Start()
    {
        base.Start();
        GameManager.Instance.PauseGame(true);
    }


    public void OnContinueButtonPressed()
    {
        GameManager.Instance.PauseGame(false);
        Close();
    }


    public void OnQuitButtonPressed()
    {
        ParentScreen.OpenPopup<ConfirmationPopup>("Popups/ConfirmationPopup", popup =>
        {
            popup.SetInfo("Quit game", "Do you really want to quit the game?", () =>
            {
                GetComponent<ScreenTransition>().PerformTransition();
            });
        });
    }
}
