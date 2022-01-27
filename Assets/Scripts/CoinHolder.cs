using PixelCrew.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinHolder : MonoBehaviour
{
    private GameSession _session;

    private void Start()
    {
        _session = FindObjectOfType<GameSession>();
    }

    public void PickedSilverCoin()
    {
        _session.Data.Coins += 1;
        Debug.Log(_session.Data.Coins);
    }

    public void PickedGoldenCoin()
    {
        _session.Data.Coins += 10;
        Debug.Log(_session.Data.Coins);
    }
}
