using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSpawner : MonoBehaviour
{
    [SerializeField] private EventBroadcast eventBroadcast;
    [SerializeField] public PlayerStat playerStat;
    [SerializeField] private GameObject target;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private Transform[] patrolPos;
    [SerializeField] public EnemyStat enemyStat;
    [SerializeField] private GameObject enemyObj;
    [SerializeField] private GameObject spawnedEne;

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
        SpawnNewEnemy();
    }
    private void SpawnNewEnemy()
    {
        spawnedEne = Instantiate(enemyObj);
        spawnedEne.transform.position = spawnPos.position;
        EnemyStateManager enemyStateManager = spawnedEne.GetComponent<EnemyStateManager>();
        enemyStateManager.eventBroadcast = eventBroadcast;
        enemyStateManager.playerStat = playerStat;
        enemyStateManager.enemyStat = enemyStat;
        enemyStateManager.target = target.transform;
        enemyStateManager.patrolPath = patrolPos;

    }

    private void CountDaDeath()
    {
            if(spawnedEne.GetComponent<EnemyStateManager>().currentHP <= 0)
                SpawnNewEnemy();
    }
}