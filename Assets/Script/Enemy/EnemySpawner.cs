using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EventBroadcast eventBroadcast;
    [SerializeField] public PlayerStat playerStat;
    [SerializeField] private GameObject target;
    [SerializeField] private Transform[] spawnPos;
    [SerializeField] public EnemyStat[] enemyStats;
    [SerializeField] private GameObject[] enemyObj;

    private List<float[]> EnemyField = new List<float[]> { };
    private int mapPointer = 0;
    private List<int> enemyIDList = new List<int>();
    [SerializeField] private int spawnAmount = 0;
    [SerializeField] private int spawnWave = 0;
    [SerializeField] private int killedAmount = 0;

    private void OnEnable()
    {
        eventBroadcast.enemyKilled += CountDaDeath;
    }
    private void OnDisable()
    {
        eventBroadcast.enemyKilled -= CountDaDeath;
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
    private void SpawnNewEnemy()
    {
        RefreshMap();
        spawnAmount = 2 + spawnWave;
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
            int enemyID;
            if (i < 2)
            {
                enemyID = enemyIDList[0];
            }
            else 
            {
                enemyID = enemyIDList[1];
            }
            if (possibleField.Count == 0)
            {
                return;
            }
            mapPointer = Random.Range(0, possibleField.Count);
            EnemyField.First(r => r.SequenceEqual(possibleField[mapPointer]))[2] = 1;
            GameObject spawnedEne;
            spawnedEne = Instantiate(enemyObj[enemyID]);
            spawnedEne.transform.position = new Vector3(possibleField[mapPointer][0], possibleField[mapPointer][1], 0f);
            EnemyStateManager enemyStateManager = spawnedEne.GetComponent<EnemyStateManager>();
            enemyStateManager.eventBroadcast = eventBroadcast;
            enemyStateManager.playerStat = playerStat;
            enemyStateManager.enemyStat = enemyStats[enemyID];
            enemyStateManager.target = target.transform;
        }
    }

    private void GenerateEnemyID()
    {
        if(enemyObj.Length < 2)
        {
            Debug.LogError("Not enough enemy!");
            return;
        }
        int i = 0;
        do
        {
            i = Random.Range(0, enemyObj.Length);
            if (enemyIDList.Count > 0)
            {
                if (!enemyIDList.Contains(i))
                {
                    enemyIDList.Add(i);
                }
            }
            else
            {
                enemyIDList.Add(i);
            }
        }
        while (enemyIDList.Count < 2);
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
                eventBroadcast.AllDeadNoti();
            }
        }
    }
}
