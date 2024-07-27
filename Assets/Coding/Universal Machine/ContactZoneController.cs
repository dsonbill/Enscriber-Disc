using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactZoneController: MonoBehaviour
{
    public List<ContactZone> Zones;
    public List<ContactZone> Caps;

    public List<ContactZone> Walls;
    public List<ContactZone> Halls;

    public List<ContactZone> Slots;

    public List<ContactZone> Ticks;
    public List<ContactZone> Ticks2;
    public List<ContactZone> Ticks3;
    public List<ContactZone> Ticks4;
    public List<ContactZone> Ticks5;
    public List<ContactZone> Ticks6;
    public List<ContactZone> Ticks7;
    public List<ContactZone> Ticks8;
    public List<ContactZone> Ticks9;


    public Action<Collision> ZoneExit;
    public Action<Collision> CapEnter;

    void Start()
    {
        foreach (ContactZone zone in Zones)
        {
            zone.ZoneExit = ZoneExit;
        }

        foreach (ContactZone zone in Caps)
        {
            zone.ZoneEntry = CapEnter;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
