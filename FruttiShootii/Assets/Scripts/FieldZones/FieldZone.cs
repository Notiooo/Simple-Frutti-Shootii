using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FieldZoneMgr.Instance.PlayerEnteredZone();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FieldZoneMgr.Instance.PlayerLeftZone();
        }
    }
}
