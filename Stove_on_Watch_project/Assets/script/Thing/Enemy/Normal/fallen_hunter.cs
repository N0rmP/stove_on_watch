using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallen_hunter : abst_enemy
{
    public fallen_hunter(int x, int y) : base(x, y) {
        actions_per_turn = 1;
        //��2�ܰ���  upgrade �Լ� ���� �� ����
        initial_action_list = new List<abst_enemy_action> { new aim(this), new aim(this), new intercept(this) };
        chase_module = new near_chaser(this);
        list_reset();
    }
}
