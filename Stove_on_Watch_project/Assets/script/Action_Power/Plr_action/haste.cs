using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class haste : abst_Plr_action
{
    public haste() : base() {
        max_cost = initial_max_cost = 0;
        cur_cooltime = 0;
        is_savable = false;
        tags.Add(concepts.haste);
        tags.Add(concepts.simple);
        power_made_by_this = new temp_power(owner, this);
    }
    protected override void effect() {
        GameManager.g.haste(owner);
    }
}
