using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class husk : abst_power
{
    public husk(abst_enemy a) : base(a) {
        set_visible(true);
    }

    public override void on_after_ROLL(ref int roll) {
        owner.set_cur_hp(true, -2);
    }
}
