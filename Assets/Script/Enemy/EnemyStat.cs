using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStat", menuName = "ScriptableObject/EnemyStat")]
public class EnemyStat : ScriptableObject
{
    public int enemyMaxHP = 500;
    public float enemyMovementSpeed = 0.25f;
    public string enemyType = "Default";
    public int enemyRewardWeaponID = 0;
    public string enemyBehavior = "Default";
    public int explosionImmunity = 0;
    public int poisonImmunity = 0;
    public List<int> enemyWeaponId = new List<int>();
    public List<float> enemyAtkRange = new List<float>();
    public List<bool> requireLOS = new List<bool>();
    public List<float> enemyCD = new List<float>();
    public List<float> enemyAimTime = new List<float>();
}
