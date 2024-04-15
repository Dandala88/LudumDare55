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
    public Summon boss;

    public GameObject smoke;


    public int waves;

    public int currentWave;

    private bool waveStarted;


    private void Start()
    {
        waves += CreditsManager.newGames;
        if (GameManager.quickLevels)
            waves = 1;
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
        var summonToSpawn = RandomSummon();
        var currentMaxSummons = GameManager.GetWaveMaxSummon(currentWave);
        if (currentWave == waves)
        {
            summonToSpawn = boss;
            currentMaxSummons = 1;
        }
        var spawnSide = Random.Range(0, 2) == 0 ? spawnPointLeft : spawnPointRight;
        var spawnPoint = spawnSide.position + (Vector3.forward * Random.Range(-3, 4));
        var summon = Instantiate(summonToSpawn, spawnPoint, Quaternion.identity);
        currentWaveSummons.Add(summon);
        summon.startDirection = Vector3.left;
        var smokeClone = Instantiate(smoke, summon.transform.position + Vector3.down, Quaternion.identity);
        waveStarted = true;
        yield return new WaitForSeconds(spawnTime);
        if (currentWaveSummons.Count < currentMaxSummons)
        {
            StartCoroutine(SpawnCoroutine());
        }
    }

    private Summon RandomSummon()
    {
        var roll = Random.Range(0, 3);
        if(roll == 0)
            return primarySummon;
        else if (roll == 1 && secondarySummon != null)
            return secondarySummon;
        else if (roll == 2 && tertiarySummon != null)
            return tertiarySummon;

        return primarySummon;
    }
}
