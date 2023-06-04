using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelector : MonoBehaviour
{
    [SerializeField] List<Card> deck;
    [SerializeField] CardElement[] cardUI;
    [SerializeField] Modifiers modifiers;

    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Canvas cardCanvas;
    [SerializeField] Spawner spawner;


    public void SelectCardFromList()
    {
        List<Card> deckCopy = new List<Card>(deck);
        foreach (var item in cardUI)
        {
            var randomCard = deckCopy[Random.Range(0, deckCopy.Count)];
            item.AttachCard(randomCard);
            deckCopy.Remove(randomCard);
        }
    }


    public void ApplyModifiers(Card card)
    {
        foreach (var item in card.modifiers)
        {
            modifiers.ApplyModifiers(item.type, item.multiplier);
        }
    }

    public IEnumerator OpenCardSelection()
    {
        SelectCardFromList();
        spawner.CardSelecting(true);
        cardCanvas.gameObject.SetActive(true);
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += 1 * Time.deltaTime;
            yield return null;
        }
        InteractableCards(true);
    }

    public IEnumerator CloseCardSelection()
    {
        InteractableCards(false);
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= 1 * Time.deltaTime;
            yield return null;
        }
        cardCanvas.gameObject.SetActive(false);
        spawner.CardSelecting(false);
    }

    private void InteractableCards(bool state)
    {
        foreach (var item in cardUI)
        {
            item.Interactable(state);
        }
    }
}
