using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp_enemy_power : abst_power
{
    public override int on_before_attack(thing receiver, int v)
    {
        Debug.Log("every time temp_enemy attacks, this activates");
        return (v+1);
    }
}