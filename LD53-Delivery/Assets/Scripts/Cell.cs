using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] float moveForce;
    [SerializeField] float attackRadius;
    [SerializeField] float attackRange;
    [SerializeField] float attackForce;
    [SerializeField] float attackDelay;
    [SerializeField] float attackCoolDown;
    private Rigidbody2D rb;

    private bool attacking = false;
    private bool canMove = true;
    [HideInInspector] public Transform target = null;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        
        if (target != null) 
        {
            transform.up = target.position - transform.position; 
            if(Vector2.Distance(rb.position, target.position) <= attackRange && !attacking)
            {
                StartCoroutine(Attack(attackDelay, target));
            }
        }
    }
    private void FixedUpdate()
    {
        if(canMove && target != null) rb.AddForce(transform.up * moveForce);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!canMove)
        {
            Transform targetTransform = collision.transform;
            if (!targetTransform.CompareTag("Player")) Destroy(targetTransform.gameObject);
            else
            {
                for (var i = 0; i < 4; i++)
                {
                    targetTransform.GetComponent<CharacterController>().Throw(i);
                }
                targetTransform.GetComponent<Rigidbody2D>().AddForce(transform.up * attackForce, ForceMode2D.Impulse);
            }
        }
    }

    IEnumerator Attack(float wait, Transform targetTransform)
    {
        attacking = true;
        canMove = false;
        yield return new WaitForSeconds(wait);

        if (targetTransform != null)
        {
            rb.AddForce(transform.up * attackForce, ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(attackCoolDown / 2);
        canMove = true;
        yield return new WaitForSeconds(attackCoolDown/2);
        attacking = false;
    }

    void OnDrawGizmos()
    {
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
