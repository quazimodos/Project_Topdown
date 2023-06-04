/// This class contains the logic associated to the in-game pause popup.
public class PausePopup : Popup
{
    public void OnContinueButtonPressed()
    {
        //pause edilen şeyler devam edecek.
        Close();
    }

    public void OnRestartButtonPressed()
    {
        ParentScreen.OpenPopup<ConfirmationPopup>("Popups/ConfirmationPopup", popup =>
        {
            popup.SetInfo("Restart game", "Do you really want to restart the game?", () =>
            {
                //oyuna restart atılacak.
                popup.Close();
                Close();
            });
        });
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
