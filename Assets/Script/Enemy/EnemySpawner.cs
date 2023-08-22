using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public bool specialSpawn = false;
    private bool stageFinished = false;
    private float nextSpawnTime = 0;
    [SerializeField] private EventBroadcast eventBroadcast;
    [SerializeField] public PlayerStat playerStat;
    [SerializeField] private GameObject target;
    [SerializeField] private Transform[] spawnPos;
    [SerializeField] private Transform[] patrolPos;
    [SerializeField] private GameObject[] rangeEnemyObj;
    [SerializeField] private GameObject[] meleeEnemyObj;
    [SerializeField] private GameObject[] supportEnemyObj;
    [SerializeField] private GameObject[] specialEnemyObj;

    private List<float[]> EnemyField = new List<float[]> { };
    private int mapPointer = 0;
    private List<int[]> enemyIDList = new List<int[]>();
    [SerializeField] private int spawnAmount = 0;
    [SerializeField] private int spawnWave = 0;
    [SerializeField] private int killedAmount = 0;

    // Reward
    [SerializeField] private GameObject[] rewards;

    private void OnEnable()
    {
        eventBroadcast.enemyKilled += CountDaDeath;
        eventBroadcast.finishStage += StopSpawning;
    }
    private void OnDisable()
    {
        eventBroadcast.enemyKilled -= CountDaDeath;
        eventBroadcast.finishStage -= StopSpawning;
    }

    private void Start()
    {
        GenerateEnemyID();
        killedAmount = 0;
        spawnWave = 0;
        for (int i = 0; i < spawnPos.Length; i++)
        {
            EnemyField.Add(new float[] { spawnPos[i].transform.position.x, spawnPos[i].transform.position.y, 0 });
        }
        SpawnNewEnemy();
    }
    private void Update()
    {
        if (specialSpawn && Time.time > nextSpawnTime && killedAmount == spawnAmount)
        {
            killedAmount = 0;
            SpawnNewEnemy();
        }
    }
    private void SpawnNewEnemy()
    {
        if (stageFinished)
            return;
        RefreshMap();
        if (specialSpawn)
        {
            enemyIDList.Clear();
            List<int> type = new List<int> { 0, 1, 2, 3 };
            switch (type[Random.Range(0, type.Count)])
            {
                case 0:
                    enemyIDList.Add( new int[] { Random.Range(0, rangeEnemyObj.Length), 0 });
                    break;
                case 1:
                    enemyIDList.Add(new int[] { Random.Range(0, meleeEnemyObj.Length), 1 });
                    break;
                case 2:
                    enemyIDList.Add(new int[] { Random.Range(0, supportEnemyObj.Length), 2 });
                    break;
                case 3:
                    enemyIDList.Add(new int[] { Random.Range(0, specialEnemyObj.Length), 3 });
                    break;
            }
            spawnAmount = 2;
        }
        else if (MapGenerator.Instance.currentPos[0] <= 2)
        {
            spawnAmount = 2 + spawnWave;
        }
        else
        {
            spawnAmount = 3 + spawnWave;
        }
        for (int i = 0; i < spawnAmount; i++)
        {
            List<float[]> possibleField = new List<float[]> { };
            foreach (float[] fl in EnemyField)
            {
                if (fl[2] == 0)
                {
                    possibleField.Add(fl);
                }
            }
            int[] enemyID;
            if (i < 2)
            {
                enemyID = enemyIDList[0];
            }
            else if (i < 3)
            {
                enemyID = enemyIDList[1];
            }
            else
            {
                enemyID = enemyIDList[2];
            }
            if (possibleField.Count == 0)
            {
                return;
            }
            mapPointer = Random.Range(0, possibleField.Count);
            EnemyField.First(r => r.SequenceEqual(possibleField[mapPointer]))[2] = 1;
            GameObject spawnedEne;
            switch (enemyID[1])
            {
                case 0:
                    spawnedEne = Instantiate(rangeEnemyObj[enemyID[0]]);
                    break;
                case 1:
                    spawnedEne = Instantiate(meleeEnemyObj[enemyID[0]]);
                    break;
                case 2:
                    spawnedEne = Instantiate(supportEnemyObj[enemyID[0]]);
                    break;
                case 3:
                    spawnedEne = Instantiate(specialEnemyObj[enemyID[0]]);
                    break;
                default:
                    spawnedEne = Instantiate(rangeEnemyObj[enemyID[0]]);
                    break;
            }
            spawnedEne.transform.position = new Vector3(possibleField[mapPointer][0], possibleField[mapPointer][1], 0f);
            EnemyStateManager enemyStateManager = spawnedEne.GetComponent<EnemyStateManager>();
            enemyStateManager.eventBroadcast = eventBroadcast;
            enemyStateManager.playerStat = playerStat;
            enemyStateManager.target = target.transform;
            enemyStateManager.patrolPath = patrolPos;
        }
        nextSpawnTime = Time.time + 12f;
    }

    private void GenerateEnemyID()
    {
        if (rangeEnemyObj.Length + meleeEnemyObj.Length + supportEnemyObj.Length + specialEnemyObj.Length < 3)
        {
            Debug.LogError("Not enough enemy!");
            return;
        }
        enemyIDList.Clear();
        int[] i = new int[] { 0, 0 };
        List<int> type = new List<int> { 0, 1, 2, 3 };
        do
        {
            switch (enemyIDList.Count)
            {
                case 0:
                    switch (Random.Range(0, 2))
                    {
                        case 0:
                            i = new int[] { Random.Range(0, rangeEnemyObj.Length), 0 };
                            type.Remove(0);
                            break;
                        case 1:
                            i = new int[] { Random.Range(0, meleeEnemyObj.Length), 1 };
                            type.Remove(1);
                            break;
                    }
                    break;
                case 1:
                    switch (Random.Range(0, 2))
                    {
                        case 0:
                            i = new int[] { Random.Range(0, supportEnemyObj.Length), 2 };
                            type.Remove(2);
                            break;
                        case 1:
                            i = new int[] { Random.Range(0, specialEnemyObj.Length), 3 };
                            type.Remove(3);
                            break;
                    }
                    break;
                case 2:
                    switch (type[Random.Range(0, type.Count)])
                    {
                        case 0:
                            i = new int[] { Random.Range(0, rangeEnemyObj.Length), 0 };
                            break;
                        case 1:
                            i = new int[] { Random.Range(0, meleeEnemyObj.Length), 1 };
                            break;
                        case 2:
                            i = new int[] { Random.Range(0, supportEnemyObj.Length), 2 };
                            break;
                        case 3:
                            i = new int[] { Random.Range(0, specialEnemyObj.Length), 3 };
                            break;
                    }
                    break;
                default:
                    i = new int[] { Random.Range(0, rangeEnemyObj.Length), 0 };
                    break;
            }
            if (enemyIDList.Count > 0)
            {
                if (!enemyIDList.Any(x => x[0] == i[0] && x[1] == i[1]))
                {
                    enemyIDList.Add(i);
                }
            }
            else
            {
                enemyIDList.Add(i);
            }
        }
        while (enemyIDList.Count < 3);
    }
    private void RefreshMap()
    {
        for (int i = 0; i < EnemyField.Count; i++)
        {
            EnemyField[i][2] = 0;
        }
    }

    private void CountDaDeath()
    {
        killedAmount++;
        if (!specialSpawn)
            CheckForFinish();
    }

    private void CheckForFinish()
    {
        if (killedAmount == spawnAmount)
        {
            spawnWave++;
            if (spawnWave < 3)
            {
                killedAmount = 0;
                SpawnNewEnemy();
            }
            else
            {
                int temp = Random.Range(0, 100);
                if(playerStat.currentHP < playerStat.maxHP * 25 / 100f)
                {
                    Instantiate(rewards[1]).transform.position = target.transform.position;
                }
                else if (playerStat.currentHP < playerStat.maxHP * 75 / 100f)
                {

                    if (temp >= 40)
                        Instantiate(rewards[0]).transform.position = target.transform.position;
                    else
                        Instantiate(rewards[1]).transform.position = target.transform.position;
                }
                else if (playerStat.currentHP < playerStat.maxHP)
                {
                    if (temp >= 40)
                        Instantiate(rewards[0]).transform.position = target.transform.position;
                    else
                        Instantiate(rewards[2]).transform.position = target.transform.position;
                }
                else
                {
                    if (temp >= 80)
                        Instantiate(rewards[0]).transform.position = target.transform.position;
                    else
                        Instantiate(rewards[2]).transform.position = target.transform.position;
                }
                eventBroadcast.AllDeadNoti();
                eventBroadcast.GainEXPNoti(1);
            }
        }
    }
    private void StopSpawning()
    {
        stageFinished = true;
        eventBroadcast.AllDeadNoti();
        eventBroadcast.GainEXPNoti(2);
    }
}
