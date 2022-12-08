using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class preparation : abst_Plr_action
{
    public preparation() : base() {
        max_cost = initial_max_cost = 5;
        cur_cooltime = 0;
        is_savable = false;
        tags.Add(concepts.simple);
        power_made_by_this = new preparation_power(owner);
    }
    protected override void effect() {
        owner.power_add(power_made_by_this);
    }
}
