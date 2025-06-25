using UnityEngine;

public class Coins : MonoBehaviour
{
    
    [SerializeField] [Range(1,100)] private int coinsMaxValue = 2;
    [SerializeField] private bool randomCoins = true;

    public int CoinsEarned { get; private set; } = 0;

    void Start()
    {
        CoinsEarned = randomCoins ? Random.Range(2, coinsMaxValue) : coinsMaxValue;
    }
}