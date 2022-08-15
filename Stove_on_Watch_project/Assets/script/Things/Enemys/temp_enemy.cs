using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp_enemy : abst_enemy
{
    public temp_enemy() {
        this.actions_per_turn = 1;
        this.enemy_tier = enemy_tiers.normal;
        this.initial_action_list = new List<abst_enemy_action> { new temp_enemy_action(this), new temp_enemy_action(this), new temp_enemy_action(this) };
        base.init();
        this.powers.Add(new temp_enemy_power());
    }
}
