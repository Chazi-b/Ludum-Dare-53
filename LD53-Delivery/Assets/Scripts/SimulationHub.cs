using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationHub : MonoBehaviour
{
    public static SimulationHub Ref;

    private void Awake()
    {
        Ref = this;
    }

    [Header("Spawn Variables For Bacteria")]
    public float bSpawnCooldown;
    public float bDamageCooldown;
    public float bDefensiveChance;
    [Header("Spawn Variables For Immune System")]
    public float isSpawnCooldown;
    public float isDamageCooldown;
    public float isDefensiveChance;
    [Header("Variables for individual Bacteria Cells")]
    public float bKillChance;
    [Header("Variables for individual T Cells")]
    public float isKillChance;
}
