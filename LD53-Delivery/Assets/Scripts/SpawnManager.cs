using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Ref;

    private void Awake()
    {
        Ref = this;
    }

    [SerializeField] private GameObject Cell;

    public void SpawnCell(Vector3 position, string faction)
    {
        var newCell = Instantiate(Cell, position, Quaternion.identity);
        newCell.tag = faction;
    }
}
