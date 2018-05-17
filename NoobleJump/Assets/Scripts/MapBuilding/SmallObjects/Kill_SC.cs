using UnityEngine;
using System.Collections;
using System;

public class Kill_SC : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Unit_SC _unitSc = collision.transform.GetComponent<Unit_SC>();
        if (_unitSc)
            _unitSc.Kill();
    }
}
