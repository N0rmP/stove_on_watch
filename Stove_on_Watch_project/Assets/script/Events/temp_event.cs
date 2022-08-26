using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp_event : Abst_event
{
    public temp_event() : base() {
        event_name = "event_test";
    }

    protected override void choice1() {
        GameManager.g.hp_change(GameManager.g.get_Plr(), 3);
    }

    protected override void choice2() { 
        //★플레이어가 이벤트를 마치고 떠나는 것을 어떻게 판정할 것인가? 까먹지 말고 한글 폰트도 집어넣을 것
    }
}
