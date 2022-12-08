using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class intercept_power : abst_power {
    public intercept_power(thing t) : base(t) {
        set_visible(true);
    }

    public override void on_before_hp_down(ref int v) {
        v += 10;
        count--;
        if (v > 0) {
            v = 0;
        }
        if (count < 1)
            owner.power_remove(this);
    }
}
