using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class preparation_power : abst_power
{
    public preparation_power(thing t) : base(t) {
        set_visible(true);
    }

    public override void on_before_attack(thing receiver, ref int v) {
        v += 2;
    }

    public override void on_Plr_turn_end() {
        owner.power_remove(this);
    }
}
