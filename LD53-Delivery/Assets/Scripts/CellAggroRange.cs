using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellAggroRange : MonoBehaviour
{
    [SerializeField] private Cell tcell;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Medicine"))
        {
            tcell.target = collision.transform;
        }
        else if (collision.CompareTag("Player"))
        {
            if (tcell.target == null) tcell.target = collision.transform;
        }
    }
}
