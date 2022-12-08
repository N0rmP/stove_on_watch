using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mercy_mother : abst_power {
    private player Plr;

    public mercy_mother(thing p_owner) : base(p_owner) {
        set_visible(true);
        Plr = GameManager.g.get_Plr();
    }

    public override void on_Plr_turn_start() {
        Plr.set_cur_hope(true,
            Plr.get_max_hp() - Plr.get_cur_hp()
            );
    }
}
