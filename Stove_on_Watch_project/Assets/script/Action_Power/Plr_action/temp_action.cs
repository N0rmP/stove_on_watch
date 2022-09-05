using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp_action : abst_Plr_action
{
    public temp_action() : base() {
        action_name = "temp_action";
        initial_max_cost = 2;
        max_cost = 2;
        cur_cooltime = 0;
        is_savable = false;
        tags.Add(concepts.haste);
        power_made_by_this = new temp_power(this);
    }
    protected override void effect()
    {
        GameManager.g.attack(this.owner, GameManager.g.get_selected_enemy(), 40);
        GameManager.g.haste(this.owner);
        this.owner.powers.Add(this.power_made_by_this);
    }

    public override void update()
    {
        if (this.cur_cooltime <= 0) { this.owner.powers.Remove(this.power_made_by_this); }
    }
}
