using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp_enemy : abst_enemy
{
    public temp_enemy() {
        actions_per_turn = 1;
        enemy_tier = enemy_tiers.normal;
        initial_action_list = new List<abst_enemy_action> { new temp_enemy_action(this), new temp_enemy_action(this), new temp_enemy_action(this) };
        powers.Add(new temp_enemy_power());
        list_reset();
    }
}
