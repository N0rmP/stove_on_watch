using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp_enemy_power : abst_power
{
    public override int on_before_attack(thing receiver, int v)
    {
        return (v+1);
    }
}
