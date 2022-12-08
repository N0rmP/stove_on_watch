using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class take_cover_power : abst_power
{
    public take_cover_power(thing t) : base(t) {
        set_visible(true);
    }

    public override void on_Plr_turn_start() {
        GameManager.g.block(owner, 9);
        count--;
        if (count < 1)
            owner.power_remove(this);
    }
}
