using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class abst_root_enemy : abst_enemy
{
    public abst_root_enemy(int x, int y) : base(x, y) {
    }

    protected override void personal_init() {
        base.personal_init();
        enemy_tier_ = enemy_tiers.root;
    }

    public override void act() {
        if (cur_hp > 0) {
            if (combat_board != null) {
                combat_act();
            } else {
                map_move();
            }
        }
    }

    public override void give_reward() {
        GraphicManager.g.clear();
        GameManager.g.stop_coroutine();
    }
}
