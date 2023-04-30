using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float horizontal;
    private float vertical;

    [HideInInspector] public List<Transform> heldObjects;
    [HideInInspector] public List<Transform> heldObjectsTarget;

    [Header("Stats")]
    [SerializeField] private float moveForce;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float throwForce;

    [Header("Component Refs (ignore this stuff)")]
    [SerializeField] private Transform[] armHitboxes;


    public void Throw(int armIndex)
    {
        if (heldObjects[armIndex] == null) return;
        


        Vector3 direction = Vector3.zero;
        switch (armIndex)
        {
            case 0: direction = transform.up; break;
            case 1: direction = transform.right; break;
            case 2: direction = -transform.up; break;
            case 3: direction = -transform.right; break;
        }
        heldObjects[armIndex].GetComponent<Rigidbody2D>().AddForce((direction * throwForce) + (direction * moveForce * vertical), ForceMode2D.Impulse);
        StartCoroutine(ThrowDelay(0.2f, heldObjects[armIndex]));
        heldObjects[armIndex] = null;

    }

    private void GrabOrThrow(int armIndex)
    {
        if (heldObjects[armIndex] == null)
        {
            if (heldObjectsTarget[armIndex] != null)
            {
                heldObjects[armIndex] = heldObjectsTarget[armIndex];
                heldObjectsTarget[armIndex] = null;
                Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), heldObjects[armIndex].GetComponent<CircleCollider2D>(), true);
            }
        }
        else
        {
            Throw(armIndex);
        }
    }

    private IEnumerator ThrowDelay(float wait, Transform thrownObj)
    {
        yield return new WaitForSeconds(wait);
        if(thrownObj != null) Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), thrownObj.GetComponent<CircleCollider2D>(), false);

    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        heldObjects = new List<Transform>();

        for(var i = 0; i < 4; i++)
        {
            heldObjects.Add(null);
            heldObjectsTarget.Add(null);
        }
    }
    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        if (Input.GetButtonDown("W"))
        {
            GrabOrThrow(0);
        }
        if (Input.GetButtonDown("D"))
        {
            GrabOrThrow(1);
        }
        if (Input.GetButtonDown("S"))
        {
            GrabOrThrow(2);
        }
        if (Input.GetButtonDown("A"))
        {
            GrabOrThrow(3);
        }
    }

    private void FixedUpdate()
    {
        //Move
        rb.AddForce(transform.up * moveForce * vertical);
        rb.rotation -= horizontal * rotationSpeed;

        //Grab
        for(var i = 0; i < 4; i++)
        {
            if (heldObjects[i] != null)
            {
                heldObjects[i].GetComponent<Rigidbody2D>().MovePosition(armHitboxes[i].position);
            }
        }

        
    }
}
