using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp_action : abst_Plr_action
{
    public temp_action() : base() {
        this.initial_max_cost = 2;
        this.max_cost = 2;
        this.cur_cooltime = 0;
        this.is_savable = false;
        this.tags = new concepts[] { concepts.haste };
        this.power_made_by_this = new temp_power(this);
    }
    protected override void effect()
    {
        GameManager.g.attack(this.owner, GameManager.g.get_selected(), 40);
        GameManager.g.haste(this.owner);
        this.owner.powers.Add(this.power_made_by_this);
        this.cur_cooltime = 2;
    }

    public override void update()
    {
        if (this.cur_cooltime <= 0) { this.owner.powers.Remove(this.power_made_by_this); }
    }
}
