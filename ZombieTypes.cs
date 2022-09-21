using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Enum for the different stats of the Zombie
/// </summary>
public enum ZombieTyping { Aggressive =0, Defensive, Speedy}
public class ZombieTypes : MonoBehaviour
{
    [Header("Zombie Settings")]
    [Tooltip("Used to set what type of Zombie")]
    public ZombieTyping Type;
    [Tooltip("Used to set the stats of each Zombie type")]
    public int[] ZombieStats;

    NavMeshAgent agent;
    AIController aiController;

    /// <summary>
    /// When enabled, the zombies recieve a ZombieType with corresponding stats
    /// </summary>
    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        aiController = GetComponent<AIController>();

        SetType();
    }

    /// <summary>
    /// Assigns a set of stats to the zombie, rolled randomly between the 3 available
    /// </summary>
    private void SetType()
    {
        int randomNumber = 0;
        randomNumber = Random.Range(0, 3);
        switch (randomNumber)
        {
            case 0:
                SetStatsAndEnum(40, 20, 3, 10, ZombieTyping.Aggressive);
                break;
            case 1:
                SetStatsAndEnum(10, 50, 2, 15, ZombieTyping.Defensive);
                break;
            case 2:
                SetStatsAndEnum(20, 25, 5, 10, ZombieTyping.Speedy);
                break;
        }
    }

    /// <summary>
    /// Sets ZombieType
    /// </summary>
    /// <param name="zombieType"></param>
    private void SetEnum(ZombieTyping zombieType)
    {
        Type = zombieType;
    }

    /// <summary>
    /// Sets stat values to each type of Zombie
    /// </summary>
    public void SetStatsAndEnum(int damage, int defense, int moveSpeed, int sightRange, ZombieTyping zombieType)
    {
        SetEnum(zombieType);
        SetStatValues(damage, defense, moveSpeed, sightRange);
        agent.speed = moveSpeed;
        aiController.sightRange = sightRange;
    }

    /// <summary>
    /// Allows assigning of int of stats to each category of stats
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="defense"></param>
    /// <param name="moveSpeed"></param>
    public void SetStatValues(int damage, int defense, int moveSpeed, int sightRange)
    {
        ZombieStats[0] = damage;
        ZombieStats[1] = defense;
        ZombieStats[2] = moveSpeed;
        ZombieStats[3] = sightRange;
    }
}