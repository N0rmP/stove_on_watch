using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Plr_action_button : abst_Plr_action_image
{
    private int combat_button_order;
    public int combat_button_order_ {
        get { return combat_button_order; }
        set {
            if (value <= 5 & value >= 0) {
                combat_button_order = value;
            } else {
                Debug.Log("combat_button_order is out of range");
            }
        }
    }
    public void in_combat_use() {
        if (GameManager.g.get_is_Plr_turn()) {
            GameManager.g.get_order_list().Enqueue(this.get_target());
        }
    }
}
