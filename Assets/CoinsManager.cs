using System;
using TMPro;
using UnityEngine;

public class CoinsManager : MonoBehaviour
{
    [SerializeField] private TMP_Text coinsText;

    private void OnEnable()
    {
        CoinGrabber.CoinsUpdated += GrabbedCoins;
    }

    private void OnDisable()
    {
        CoinGrabber.CoinsUpdated -= GrabbedCoins;
    }

    private void GrabbedCoins(int coins)
    {
        coinsText.text = $"{coins}";
    }
}
