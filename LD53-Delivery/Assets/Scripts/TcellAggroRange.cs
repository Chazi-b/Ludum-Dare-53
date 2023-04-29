using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TcellAggroRange : MonoBehaviour
{
    [SerializeField] private Tcell tcell;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Medicine"))
        {
            tcell.target = collision.transform;
        }
    }
}
