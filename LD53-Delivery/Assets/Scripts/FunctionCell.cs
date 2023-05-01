using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionCell : MonoBehaviour
{

    [SerializeField] private float spawnCountdown;
    [SerializeField] private int health;
    private int maxHealth;
    [SerializeField] private float damageIncrementCountdown;
    public Faction faction;
    [Header("Probability that any cell spawned will be defensive instead of offensive (0 to 1)")]
    [SerializeField] private float defensiveChance;
    [Header("Ignore this junk")]
    [SerializeField] private GameObject cellToSpawn;
    [SerializeField] private Transform healthBar;
    [SerializeField] Color virusColor;
    [SerializeField] Color immuneSystemColor;
    public int friendlies = 0;
    public int enemies = 0;

    private string enemyTag;
    private SpriteRenderer sr;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        SetStats();


        maxHealth = health;
        StartCoroutine(SpawnCell());
        StartCoroutine(TakeDamage());
    }

    private void SetStats()
    {
        if(faction == Faction.Virus)
        {
            sr.color = virusColor;
            transform.tag = "BacterialFunctionCell";
            enemyTag = "HealthyFunctionCell";
            spawnCountdown = SimulationHub.Ref.bSpawnCooldown;
            damageIncrementCountdown = SimulationHub.Ref.bDamageCooldown;
            defensiveChance = SimulationHub.Ref.bDefensiveChance;
        }
        else
        {
            sr.color = immuneSystemColor;
            transform.tag = "HealthyFunctionCell";
            enemyTag = "BacterialFunctionCell";
            spawnCountdown = SimulationHub.Ref.isSpawnCooldown;
            damageIncrementCountdown = SimulationHub.Ref.isDamageCooldown;
            defensiveChance = SimulationHub.Ref.isDefensiveChance;
        }
    }
    IEnumerator TakeDamage()
    {
        if(enemies > friendlies)
        {
            health--;
            if(health <= 0)
            {
                if(faction == Faction.Virus)
                {
                    sr.color = immuneSystemColor;
                    faction = Faction.ImmuneSystem;
                }
                else
                {
                    sr.color = virusColor;
                    faction = Faction.Virus;
                }
                health = maxHealth;
                var oldEnemies = enemies;
                enemies = friendlies;
                friendlies = oldEnemies;
                SetStats();
            }
        }
        else if(enemies < friendlies)
        {
            health++;
            if (health > maxHealth) health = maxHealth;
        }
        Vector3 newScale = healthBar.localScale;
        newScale.x = (1f / maxHealth) * health;
        healthBar.localScale = newScale;
        yield return new WaitForSeconds(damageIncrementCountdown + Random.Range(-1f,1f));
        StartCoroutine(TakeDamage());
    }
    IEnumerator SpawnCell()
    {
        Vector3 spawnPoint = transform.position + new Vector3(Random.Range(-8, 8), Random.Range(-8, 8), 0);
        var newCell = Instantiate(cellToSpawn, spawnPoint, Quaternion.identity);
        Color newCellColor;
        if (faction == Faction.ImmuneSystem) newCellColor = immuneSystemColor;
        else newCellColor = virusColor;
        newCell.GetComponent<SpriteRenderer>().color = newCellColor;
        Cell c = newCell.GetComponent<Cell>();
        c.faction = faction;
        c.homeFunctionCell = gameObject;

        if(Random.Range(0f,1f) <= defensiveChance)
        {
            GameObject[] enemyFunctionCells = GameObject.FindGameObjectsWithTag(transform.tag);
            GameObject targetFunctionCell = enemyFunctionCells[Random.Range(0, enemyFunctionCells.Length)];
            c.targetFunctionCell = targetFunctionCell;
            c.aggroRange = c.aggroRange * 2;
        }
        else
        {
            GameObject[] enemyFunctionCells = GameObject.FindGameObjectsWithTag(enemyTag);
            GameObject targetFunctionCell = enemyFunctionCells[Random.Range(0, enemyFunctionCells.Length)];
            c.targetFunctionCell = targetFunctionCell;

        }

        c.target = c.targetFunctionCell;
        yield return new WaitForSeconds(spawnCountdown + Random.Range(-1f,1f));
        StartCoroutine(SpawnCell());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Virus"))
        {
            if (faction == Faction.ImmuneSystem) enemies++;
            else friendlies++;
        }
        else if (collision.CompareTag("ImmuneSystem"))
        {

            if (faction == Faction.Virus) enemies++;
            else friendlies++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Virus"))
        {
            if (faction == Faction.ImmuneSystem) enemies--;
            else friendlies--;
        }
        else if (collision.CompareTag("ImmuneSystem"))
        {

            if (faction == Faction.Virus) enemies--;
            else friendlies--;
        }
    }

}

public enum Faction
{
    ImmuneSystem,
    Virus,
    Player
}
