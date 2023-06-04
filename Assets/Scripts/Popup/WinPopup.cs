using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinPopup : Popup
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI coinsText;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    public void SetFinalResult(int score, int coins)
    {
        coinsText.text = string.Format("You have collected {0} coins!", coins);
        scoreText.text = string.Format("Your total score is {0}", score);
    }
}
