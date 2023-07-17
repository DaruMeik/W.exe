using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageStat", menuName = "ScriptableObjects/Stage")]
public class StageStat : ScriptableObject
{
    public int currentLoad = 0;
    public int currentSceneHP = 0;
    public int stageNum = -1;
    public List<int[]> currentSceneEnemyIdBox = new List<int[]>();
    public int currentSceneIndex = 0;
    public bool finishStage = false;
    public int[] nextSceneIndex = new int[2] { 0, 0 };
    public int currentRewardType = 1;
    public int[] nextRewardType = new int[2] { 0, 0 };

    private void OnEnable()
    {
        currentLoad = 0;
        currentSceneHP = 0;
        stageNum = 0;
        currentSceneEnemyIdBox = new List<int[]>();
        currentSceneIndex = 0;
        currentRewardType = 1;
        nextSceneIndex = new int[2] { 0, 0 };
        nextRewardType = new int[2] { 0, 0 };
    }
}
