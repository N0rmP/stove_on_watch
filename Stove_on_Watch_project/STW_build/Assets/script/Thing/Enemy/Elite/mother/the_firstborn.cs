using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class the_firstborn : abst_elite_enemy
{
    public the_firstborn(int x, int y) : base(x, y) {
        actions_per_turn = 1;
        //★2단계라면  upgrade 함수 구현 및 실행
        initial_action_list = new List<abst_enemy_action> { new preach(this), new preach(this), new despair(this, 30) };
        passives.Add(new mercy(this));
        chase_module = new stationary(this);
        list_reset();
    }
}
