using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp_action2 : abst_Plr_action {
    public temp_action2() : base() {
        action_name = "temp_action2";
        initial_max_cost = 2;
        max_cost = 2;
        cur_cooltime = 0;
        is_savable = false;
    }
    protected override void effect() {
        GameManager.g.attack(this.owner, GameManager.g.get_selected_enemy(), 17);
        for (int i = owner.get_cur_hp() + 3; i < owner.get_max_hp(); i += 3) {
            GameManager.g.attack(this.owner, GameManager.g.get_selected_enemy(), 17);
        }
    }

    public override void update() {
        //¡ÚÀÒÀº Ã¼·Â¿¡ ºñ·ÊÇØ ¹Ýº¹ È½¼ö Ç¥±â
    }
}
