using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static List<Summon> currentWaveSummons = new List<Summon>();

    public Summon primarySummon;
    public Summon secondarySummon;
    public Summon tertiarySummon;
    public Transform spawnPointLeft;
    public Transform spawnPointRight;
    public float spawnTime = 3;

    public GameObject smoke;


    public int waves;

    public int currentWave;

    private bool waveStarted;


    private void Start()
    {
        StartWave();
    }

    public void Update()
    {
        if(waveStarted && currentWaveSummons.Count <= 0)
        {
            waveStarted = false;
            if(currentWave == waves)
                WavesComplete();
            else
                StartWave();
        }
    }

    private void WavesComplete()
    {
        Debug.Log("Waves Complete!");
    }

    private void StartWave()
    {
        currentWave++;
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        var spawnPointRoll = Random.Range(0, 2);
        var spawnPoint = spawnPointRoll == 0 ? spawnPointLeft : spawnPointRight;
        var summon = Instantiate(primarySummon, spawnPoint);
        summon.transform.position = spawnPointRight.position;
        summon.startDirection = spawnPointRoll == 0 ? Vector3.right : Vector3.left;
        currentWaveSummons.Add(summon);
        var smokeClone = Instantiate(smoke, summon.transform);
        smokeClone.transform.position = summon.transform.position + Vector3.down;
        waveStarted = true;
        yield return new WaitForSeconds(spawnTime);
        var currentMaxSummons = GameManager.GetWaveMaxSummon(currentWave);
        if (currentWaveSummons.Count < currentMaxSummons)
        {
            StartCoroutine(SpawnCoroutine());
        }
    }
}
