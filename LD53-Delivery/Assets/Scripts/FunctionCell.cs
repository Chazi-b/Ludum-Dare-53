using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionCell : MonoBehaviour
{

    [SerializeField] private float spawnCountdown;
    [SerializeField] private GameObject enemyCell;
    [SerializeField] private int health;
    private int maxHealth;
    [SerializeField] private int damageIncrementCountdown;
    public Faction faction;
    [Header("Probability that any cell spawned will be defensive instead of offensive (0 to 1)")]
    [SerializeField] private float defensiveChance;
    [Header("Ignore this junk")]
    [SerializeField] private GameObject cellToSpawn;
    [SerializeField] Color virusColor;
    [SerializeField] Color immuneSystemColor;
    public int friendlies = 0;
    public int enemies = 0;
    private string foeString;

    private SpriteRenderer sr;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        maxHealth = health;
        if (faction == Faction.Virus) foeString = "ImmuneSystem";
        else foeString = "Virus";
        StartCoroutine(SpawnCell());
        StartCoroutine(TakeDamage());
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
            }
        }
        else
        {
            health++;
            if (health > maxHealth) health = maxHealth;
        }
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
            c.targetFunctionCell = gameObject;
        }
        else
        {
            c.targetFunctionCell = enemyCell;
        }

        c.target = c.targetFunctionCell;
        yield return new WaitForSeconds(spawnCountdown + Random.Range(-1f,1f));
        StartCoroutine(SpawnCell());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Virus"))
        {
            if (foeString == "Virus") enemies++;
            else friendlies++;
        }
        else if (collision.CompareTag("ImmuneSystem"))
        {

            if (foeString == "ImmuneSystem") enemies++;
            else friendlies++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Virus"))
        {
            if (foeString == "Virus") enemies--;
            else friendlies--;
        }
        else if (collision.CompareTag("ImmuneSystem"))
        {

            if (foeString == "ImmuneSystem") enemies--;
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
