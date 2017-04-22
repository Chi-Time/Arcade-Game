using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _PlaceHolder = null;
    [SerializeField] private int _MinWaveSize = 0, _MaxWaveSize = 0;
    [SerializeField] private float _MinMapWidth = 0, _MaxMapWidth = 0;
    [SerializeField] private float _MinMapHeight = 0, _MaxMapHeight = 0;
    [SerializeField] private float _MinSpawnDelay = 0, _MaxSpawnDelay = 0;
    [SerializeField] private float _PreSpawnDelay = 0.0f;
    [SerializeField] private Pool _Pool = new Pool();

    private int _WaveAmount { get { return Random.Range (_MinWaveSize, _MaxWaveSize); } }
    private float _SpawnTime { get { return Random.Range (_MinSpawnDelay, _MaxSpawnDelay); } }
    private float _YSpawnPosition { get { return Random.Range (_MinMapHeight, _MaxMapHeight); } }
    private float _XSpawnPosition { get { return Random.Range (_MinMapWidth, _MaxMapWidth); } }

    private Vector3[] _SpawnPositions = null;

    private void Awake ()
    {
        _Pool.Intialise ("Enemy Pool", "Enemy");
    }

    private void Start ()
    {
        SpawnInitialWave ();
        StartCoroutine ("SpawnWave");
    }

    private void SpawnInitialWave ()
    {
        SpawnPositions ();
    }

    //TODO: Implement fade in for enemies and invoke each enemy with a minor .001f delay just for FX.
    private IEnumerator SpawnWave ()
    {
        yield return new WaitForSeconds (_SpawnTime);

        SpawnPositions ();

        StopAllCoroutines ();
        StartCoroutine ("SpawnWave");
    }

    private void SpawnPositions ()
    {
        int currentWave = _WaveAmount;

        _SpawnPositions = new Vector3[currentWave];

        for (int i = 0; i < currentWave; i++)
        {
            var spawnPos = new Vector3 (_XSpawnPosition, _YSpawnPosition);

            _SpawnPositions[i] = spawnPos;
            //GetEnemy ();
        }

        SpawnPlaceHolder ();
        Invoke ("GetEnemy", _PreSpawnDelay);
    }

    private void SpawnPlaceHolder ()
    {
        for (int i = 0; i < _SpawnPositions.Length; i++)
        {
            var go = (GameObject)Instantiate (_PlaceHolder, _SpawnPositions[i], Quaternion.identity);
            Destroy (go, _PreSpawnDelay);
        }
    }

    private void  GetEnemy ()
    {
        for(int i = 0; i < _SpawnPositions.Length; i++)
        {
            var enemy = _Pool.RetrieveFromPool ();

            if (enemy)
            {
                //var spawnPos = new Vector3 (_XSpawnPosition, _YSpawnPosition);

                //enemy.transform.position = spawnPos;
                enemy.transform.position = _SpawnPositions[i];
            }
        }
    }
}
