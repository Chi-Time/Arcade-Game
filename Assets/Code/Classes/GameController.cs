using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int Score { get { return _Score; } set { UpdateScore (value); } }
    public int Kills { get { return _Kills; } set { UpdateKills (value); } }
    public Pool CoinPool = new Pool ();

    [SerializeField] private int _Score = 0;
    [SerializeField] private int _Kills = 0;

    private void Awake ()
    {
        CoinPool.Intialise ("Coin Pool", "Coin");
    } 

    private void UpdateScore (int score)
    {
        _Score = score;
    }

    private void UpdateKills (int kills)
    {
        _Kills = kills;

        //TODO: Possibly implement health upgrade after certain number of kills.
    }
}
