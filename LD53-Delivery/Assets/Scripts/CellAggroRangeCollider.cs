using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellAggroRangeCollider : MonoBehaviour
{
    [SerializeField] private Cell cell;


    private void Start()
    {
        GetComponent<CircleCollider2D>().radius = cell.aggroRange;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(cell.foeString))
        {
            cell.target = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(cell.target == collision.gameObject)
        {
            cell.target = cell.targetFunctionCell;
        }
    }
}
