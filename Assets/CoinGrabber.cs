using System;
using UnityEngine;

public class CoinGrabber : MonoBehaviour
{
    public static Action<int> CoinsUpdated;

    [SerializeField] AudioClip coinSound;

    private int _coins;
    private KartController _kartController;

    private void Start()
    {
       
        _kartController = GetComponent<KartController>();
        if (_kartController == null)
            Debug.LogWarning("KartController noy found!");
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag, other.gameObject);
        if (other.CompareTag("Coin"))
        {
            Debug.Log("cjeck tag other");
            Debug.Log(other.tag, other.gameObject);
            //_coins += other.GetComponent<Coins>().CoinsEarned;
            if (_coins >= 20)
                return;
            int earned = other.GetComponent<Coins>().CoinsEarned;
            if (_coins < 20 && earned!=0 )
            {
                _kartController?.IncreaseMaxSpeed();
            }
            _coins = Mathf.Min(_coins + earned, 20);
            CoinsUpdated?.Invoke(_coins);
            //AudioSource.PlayClipAtPoint(coinSound, other.transform.position);
        }
    }
}
