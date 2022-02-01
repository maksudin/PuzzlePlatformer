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
        _session.LocalData.Coins += 1;
        Debug.Log(_session.LocalData.Coins);
    }

    public void PickedGoldenCoin()
    {
        _session.LocalData.Coins += 10;
        Debug.Log(_session.LocalData.Coins);
    }
}
