using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp_enemy_power : abst_power
{
    public temp_enemy_power(thing p_owner) : base(p_owner) {
        set_visible(true);
    }

    public override void update_power() {
        
    }

    public override void on_before_attack(thing receiver, ref int v)
    {
        Debug.Log("every time temp_enemy attacks, this activates");
        v++;
    }
}
