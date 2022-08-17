using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Plr_action_button : abst_Plr_action_image
{
    public int button_order;
    public void in_combat_use() {
        GameManager.g.get_order_list().Enqueue(this.get_target());
    }
}
