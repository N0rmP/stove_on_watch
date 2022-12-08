using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skilled : abst_power
{
    public skilled(abst_enemy a) : base(a) {
        set_visible(true);
    }

    public override void on_after_ROLL(ref int roll) {
        roll++;
    }
}
