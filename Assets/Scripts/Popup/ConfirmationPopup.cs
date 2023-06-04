using System;
using TMPro;
using UnityEngine;

/// This class contains the logic associated to the generic confirmation popup.
public class ConfirmationPopup : Popup
{
#pragma warning disable 649
    [SerializeField]
    private TextMeshProUGUI primaryText;

    [SerializeField]
    private TextMeshProUGUI secondaryText;
#pragma warning restore 649

    private Action onAcceptAction;

    public void OnAcceptButtonPressed()
    {
        onAcceptAction();
    }

    public void SetInfo(string primary, string secondary, Action onAccept)
    {
        primaryText.text = primary;
        secondaryText.text = secondary;
        onAcceptAction = onAccept;
    }
}
