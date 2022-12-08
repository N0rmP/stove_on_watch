using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pick_up_anything_power : abst_power
{
    public pick_up_anything_power(thing t) : base(t) {
        set_visible(false);
    }

    public override void on_combat_start() {
        GameManager.g.attack(owner, GameManager.g.get_selected_enemy(), 30);
        owner.power_remove(this);
        owner.passives_.Remove(this);
    }
}
