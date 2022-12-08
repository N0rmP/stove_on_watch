using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class take_cover : abst_Plr_action
{
    public take_cover() : base() {
        max_cost = initial_max_cost = 22;
        cur_cooltime = 0;
        is_savable = false;
        tags.Add(concepts.complex);
        power_made_by_this = new take_cover_power(owner);
    }
    protected override void effect() {
        power_made_by_this.count_ = 3;
        owner.power_add(power_made_by_this);
    }
}
