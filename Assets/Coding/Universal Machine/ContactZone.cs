using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactZone : MonoBehaviour
{
    public Action<Collision> ZoneEntry;
    public Action<Collision> ZoneExit;

    public string ZoneName;
    public string ZoneDescription;
    public string ZoneType;
    public string ZoneId;


    void OnCollisionEnter(Collision collision)
    {
        ZoneEntry?.Invoke(collision);
    }

    void OnCollisionExit(Collision collision)
    {
        ZoneExit?.Invoke(collision);
    }
}
