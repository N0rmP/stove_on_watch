using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class Plr_action_button : abst_Plr_action_image
{
    public int button_order;
    public void in_combat_use() {
        GameManager.g.order_list.Enqueue(this.get_target());
        this.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Convert.ToString(this.get_target().get_cur_cooltime());   //¡Ú
    }
}
