using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp_enemy : abst_enemy
{
    public temp_enemy(int x, int y):base(x,y) {
        actions_per_turn = 1;
        enemy_tier = enemy_tiers.normal;
        //★2단계라면  upgrade 함수 구현 및 실행
        initial_action_list = new List<abst_enemy_action> { new temp_enemy_action(this), new temp_enemy_action(this), new temp_enemy_action(this) };
        passives.Add(new temp_enemy_power(this));
        list_reset();
    }
}
