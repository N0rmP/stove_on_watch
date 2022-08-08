using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp_power : abst_power
{
    private abst_Plr_action target;

    public temp_power(abst_Plr_action a) {
        this.is_visable = false;
        this.target = a;
        this.init();
    }

    public override void on_action()
    {
        this.main_num++;
        if (this.main_num >= 3) {
            this.target.set_cur_cooltime(true, -1);
            this.main_num = 0;
        }
    }
}
