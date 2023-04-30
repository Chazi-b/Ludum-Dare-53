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

    private void Start()
    {
        if (faction == Faction.Virus) { foeString = "ImmuneSystem"; transform.tag = "Virus"; }
        else { foeString = "Virus"; transform.tag = "ImmuneSystem"; }
    }
    private void Update()
    {
        transform.up = target.transform.position - transform.position;
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
        }
    }
}
