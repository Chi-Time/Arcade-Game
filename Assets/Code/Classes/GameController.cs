using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int Score { get { return _Score; } set { UpdateScore (value); } }

    private int _Score = 0;

    private void UpdateScore (int score)
    {
        _Score = score;
    }
}
