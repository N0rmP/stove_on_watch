using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class failed_fixer : abst_enemy
{
    public failed_fixer(int x, int y) : base(x, y) {
        actions_per_turn = 1;
        initial_action_list = new List<abst_enemy_action> { new attack(this), new smite(this), new block(this) };
        passives.Add(new skilled(this));
        chase_module = new near_chaser(this);
        list_reset();
    }
}
