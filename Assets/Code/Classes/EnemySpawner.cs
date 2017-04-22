using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int _MinWaveSize = 0, _MaxWaveSize = 0;
    [SerializeField] private float _MinMapWidth = 0, _MaxMapWidth = 0;
    [SerializeField] private float _MinMapHeight = 0, _MaxMapHeight = 0;
    [SerializeField] private float _MinSpawnDelay = 0, _MaxSpawnDelay = 0;
    [SerializeField] private Pool _Pool = new Pool();

    private int _WaveAmount { get { return Random.Range (_MinWaveSize, _MaxWaveSize); } }
    private float _SpawnTime { get { return Random.Range (_MinSpawnDelay, _MaxSpawnDelay); } }
    private float _YSpawnPosition { get { return Random.Range (_MinMapHeight, _MaxMapHeight); } }
    private float _XSpawnPosition { get { return Random.Range (_MinMapWidth, _MaxMapWidth); } }


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
        Spawn ();
    }

    //TODO: Implement fade in for enemies and invoke each enemy with a minor .001f delay just for FX.
    private IEnumerator SpawnWave ()
    {
        yield return new WaitForSeconds (_SpawnTime);

        Spawn ();

        StopAllCoroutines ();
        StartCoroutine ("SpawnWave");
    }

    private void Spawn ()
    {
        for (int i = 0; i < _WaveAmount; i++)
        {
            GetEnemy ();
        }
    }

    private void  GetEnemy ()
    {
        var enemy = _Pool.RetrieveFromPool ();

        if (enemy)
        {
            var spawnPos = new Vector3 (_XSpawnPosition, _YSpawnPosition);

            enemy.transform.position = spawnPos;

            //for (int j = 0; j < _Pool.ActivePool.Count; j++)
            //{
            //    if(spawnPos == _Pool.ActivePool[j].transform.position)

            //}
        }
    }
}
