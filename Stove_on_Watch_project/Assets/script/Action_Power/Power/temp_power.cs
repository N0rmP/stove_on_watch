using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp_power : abst_power
{
    private int act_count;
    private abst_Plr_action target;

    public temp_power(thing p_owner, abst_Plr_action a) : base (p_owner) {
        is_visable = false;
        target = a;
        //init();   //player can check this card's count number and prepare his initial 2~3 turn stategy, so initializing before each combat is unnecessary
    }

    public override void update_power() {
        //★대상이 되는 카드 근처에 숫자 띄워주기
    }

    public override void on_action()
    {
        this.act_count++;
        if (this.act_count >= 3) {
            this.target.set_cur_cooltime(-1, true);
            this.act_count = 0;
        }
    }
}
