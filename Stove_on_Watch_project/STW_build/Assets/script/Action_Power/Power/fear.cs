using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fear : abst_power {
    public fear(thing p_owner) : base(p_owner) {
        set_visible(true);
    }

    public override void on_combat_start() {
        int former_left_of_range = GameManager.g.get_left_of_range();
        for (; former_left_of_range > 1; former_left_of_range--) {
            GameManager.g.set_left_of_range(-1);
            GraphicManager.g.range_decrease();
        }
    }

    public override void on_before_ROLL(ref int predetermined_value, ref int max_or_min, ref int creas_set) {
        creas_set = 0;
    }
}
