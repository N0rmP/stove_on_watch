using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mercy : abst_power {
    private player Plr;
    private int stolen_hope;

    public mercy(thing p_owner) : base(p_owner) {
        set_visible(true);
        Plr = GameManager.g.get_Plr();
    }

    public override void on_combat_start() {
        stolen_hope = Plr.get_cur_hope() - 100;
        Plr.set_cur_hope(false, 100);
    }

    public override void on_combat_end() {
        Plr.set_cur_hope(true, stolen_hope);
    }

    public override void on_Plr_turn_start() {
        int val = Plr.get_max_hp() - Plr.get_cur_hp();
        if (stolen_hope < val)
            val = stolen_hope;
        Plr.set_cur_hope(true, val);
        stolen_hope -= val;
    }

    public override string get_description() {
        return description + "\nThis character is holding " + stolen_hope + " hope";
    }
}
