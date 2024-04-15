using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    public static List<Summon> currentWaveSummons = new List<Summon>();

    public Summon primarySummon;
    public Summon secondarySummon;
    public Summon tertiarySummon;
    public Transform spawnPointLeft;
    public Transform spawnPointRight;
    public float spawnTime = 3;
    public int nextScene;
    public bool debug;
    public UnityEvent waveCompleteEvent;
    public GameManager.SummonType wonSummon;

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
        if (!debug)
        {
            GameManager.SetSummonFlag(wonSummon);
            waveCompleteEvent.Invoke();
        }
    }

    private void StartWave()
    {
        currentWave++;
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        var spawnSide = Random.Range(0, 2) == 0 ? spawnPointLeft : spawnPointRight;
        var spawnPoint = spawnSide.position + (Vector3.forward * Random.Range(-3, 4));
        var summon = Instantiate(primarySummon, spawnPoint, Quaternion.identity);
        currentWaveSummons.Add(summon);
        summon.startDirection = Vector3.left;
        var smokeClone = Instantiate(smoke, summon.transform.position + Vector3.down, Quaternion.identity);
        waveStarted = true;
        yield return new WaitForSeconds(spawnTime);
        var currentMaxSummons = GameManager.GetWaveMaxSummon(currentWave);
        if (currentWaveSummons.Count < currentMaxSummons)
        {
            StartCoroutine(SpawnCoroutine());
        }
    }
}
