using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int Score { get { return _Score; } set { UpdateScore (value); } }
    public Pool CoinPool = new Pool ();

    [SerializeField] private int _Score = 0;

    private void Awake ()
    {
        CoinPool.Intialise ("Coin Pool", "Coin");
    } 

    private void UpdateScore (int score)
    {
        _Score = score;
    }
}
