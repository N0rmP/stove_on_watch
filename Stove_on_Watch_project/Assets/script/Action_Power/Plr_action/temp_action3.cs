using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp_action3 : abst_Plr_action {
    public temp_action3() : base() {
        initial_max_cost = 2;
        max_cost = 2;
        cur_cooltime = 0;
        is_savable = false;
    }
    protected override void effect() {
        GameManager.g.attack(this.owner, GameManager.g.get_selected_enemy(), 30);
    }

    public override void update() {
        //¡ÚÀÒÀº Ã¼·Â¿¡ ºñ·ÊÇØ ¹Ýº¹ È½¼ö Ç¥±â
    }
}
