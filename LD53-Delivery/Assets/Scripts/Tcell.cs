using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tcell : MonoBehaviour
{
    [SerializeField] string targetTag;
    [SerializeField] float moveForce;
    [SerializeField] float attackRange;
    [SerializeField] float attackDelay;
    private Rigidbody2D rb;

    private bool attacking = false;
    [HideInInspector] public Transform target = null;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if(target != null) transform.up = target.position - transform.position;
    }
    private void FixedUpdate()
    {
        if(!attacking && target != null) rb.AddForce(transform.up * moveForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            for(var i = 0; i < 4; i++)
            {
                collision.transform.GetComponent<CharacterController>().Throw(i);
            }
        }
        else if (collision.transform.CompareTag("Medicine"))
        {
            StartCoroutine(Attack(attackDelay, collision.transform));
        }
    }

    IEnumerator Attack(float wait, Transform targetTransform)
    {
        attacking = true;

        yield return new WaitForSeconds(wait);

        if (targetTransform != null)
        {
            if (Vector2.Distance(rb.position, targetTransform.GetComponent<Rigidbody2D>().position) <= attackRange)
            {
                Destroy(targetTransform.gameObject);
            }
        }

        attacking = false;
    }

    void OnDrawGizmos()
    {
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
