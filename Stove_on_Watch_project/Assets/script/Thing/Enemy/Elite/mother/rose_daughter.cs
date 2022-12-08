using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rose_daughter : abst_elite_enemy
{
    public rose_daughter(int x, int y) : base(x, y) {
        actions_per_turn = 2;
        //★2단계라면  upgrade 함수 구현 및 실행
        initial_action_list = new List<abst_enemy_action> { new double_attack(this), new double_attack(this), new despair(this) };
        passives.Add(new fear(this));
        chase_module = new stationary(this);
        list_reset();
    }
}
