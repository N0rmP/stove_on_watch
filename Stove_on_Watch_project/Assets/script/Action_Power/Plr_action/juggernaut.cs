using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class juggernaut : abst_Plr_action {
    public juggernaut() : base() {
        max_cost = initial_max_cost = 0;
        cur_cooltime = 0;
        is_savable = false;
        tags.Add(concepts.complex);
    }

    protected override void effect() {
        GameManager.g.attack(owner, GameManager.g.get_selected_enemy(), max_cost);
    }

    public override void acquired() {
        owner.passives_.Add(new juggernaut_power(owner, this));
    }
}
