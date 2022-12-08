using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class juggernaut_power : abst_power {
    juggernaut target;

    public juggernaut_power(thing t, juggernaut j) : base(t) {
        set_visible(false);
        target = j;
    }

    public override void on_combat_start() {
        owner.power_add(this);
    }

    public override void on_after_block(int v) {
        Debug.Log("testing");
        target.set_cost(true, 2);
    }

    public override void on_combat_end() {
        owner.power_remove(this);
    }
}
