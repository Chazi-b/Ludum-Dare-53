using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Faction faction;
    [HideInInspector] public string foeString;
    [Header("Stats")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float attackForce;
    [SerializeField] private float collisionForce;
    public float aggroRange;
    [SerializeField] private float attackRange;
    [SerializeField] private int health;

    [Header("Other Stuffs")]
    [SerializeField] private Rigidbody2D rb;
    public GameObject targetFunctionCell;
    public GameObject homeFunctionCell;

    public GameObject target;

    public Vector3 targetPosition;
    public Vector3 targetFunctionCellPosition;
    private void Start()
    {
        if (faction == Faction.Virus) { foeString = "ImmuneSystem"; transform.tag = "Virus"; }
        else { foeString = "Virus"; transform.tag = "ImmuneSystem"; }

        targetFunctionCellPosition = targetFunctionCell.transform.position + new Vector3(Random.Range(-6f, 6f), Random.Range(-6f, 6f), 0);
    }
    private void Update()
    {
        if (target != targetFunctionCell) transform.up = target.transform.position - transform.position;
        else
        {
            transform.up = targetFunctionCellPosition - transform.position;
            if(Vector3.Distance(targetFunctionCellPosition, transform.position) <= 0.1f)
            {
                targetFunctionCellPosition = targetFunctionCell.transform.position + new Vector3(Random.Range(-6f, 6f), Random.Range(-6f, 6f), 0);
            }
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.up * moveSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag(foeString))
        {
            collision.transform.GetComponent<Rigidbody2D>().AddForce((collision.transform.position - transform.position)* collisionForce, ForceMode2D.Impulse);
            float roll = Random.Range(0f, 1f);
            if (faction == Faction.Virus)
            {
                if(roll < SimulationHub.Ref.isKillChance)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                if (roll < SimulationHub.Ref.bKillChance)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
