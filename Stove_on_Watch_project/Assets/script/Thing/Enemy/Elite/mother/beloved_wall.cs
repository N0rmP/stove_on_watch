using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beloved_wall : abst_enemy
{
    public beloved_wall(int x, int y) : base(x, y) {
        actions_per_turn = 2;
        enemy_tier = enemy_tiers.elite;
        //��2�ܰ���  upgrade �Լ� ���� �� ����
        initial_action_list = new List<abst_enemy_action> { new sweep(this), new refill(this), new despair(this) };
        list_reset();
    }
}
