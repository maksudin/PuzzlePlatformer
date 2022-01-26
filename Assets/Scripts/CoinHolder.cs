using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinHolder : MonoBehaviour
{
    [SerializeField] public int Coins = 0;

    public void PickedSilverCoin()
    {
        Coins += 1;
        Debug.Log(Coins);
    }

    public void PickedGoldenCoin()
    {
        Coins += 10;
        Debug.Log(Coins);
    }
}
