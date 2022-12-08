using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mercenary_power : abst_power
{
    public mercenary_power(thing t) : base(t) {
        set_visible(false);
    }

    public override void on_combat_start() {
        GameManager.g.attack(owner, GameManager.g.get_selected_enemy(), 90);
        owner.power_remove(this);
        owner.passives_.Remove(this);
    }
}
