using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardElement : MonoBehaviour, IPointerClickHandler
{
    [Header("Card Options")]
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI cardName;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] Image cardImage;

    [Header("References")]
    [SerializeField] MMF_Player hoverFeedback;
    [SerializeField] MMF_Player clickFeedback;
    [SerializeField] CardSelector selector;
    Card card;


    public void AttachCard(Card card)
    {
        cardName.text = card.cardName;
        description.text = card.description;
        icon.sprite = card.cardImage;
        this.card = card;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        clickFeedback?.PlayFeedbacks();
        selector.ApplyModifiers(card);
        StartCoroutine(selector.CloseCardSelection());
    }

    public void Interactable(bool state)
    {
        cardImage.raycastTarget = state;
    }
}
