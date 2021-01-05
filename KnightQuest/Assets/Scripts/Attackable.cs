using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackable : MonoBehaviour
{
    public virtual void OnHit()
    {
        Debug.Log($"{gameObject} got hit!");
    }
}
