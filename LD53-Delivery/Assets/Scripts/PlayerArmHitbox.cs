using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArmHitbox : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [Header("Up 0, Right 1, Down 2, Left 3")]
    [SerializeField] private int armIndex;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(characterController.heldObjects[armIndex] == null)
        {
            characterController.heldObjectsTarget[armIndex] = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform == characterController.heldObjectsTarget[armIndex])
        {
            characterController.heldObjectsTarget[armIndex] = null;
        }
    }
}
