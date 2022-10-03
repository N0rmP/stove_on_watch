using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventory_button : abst_Plr_action_image
{
    public void inventory_click() {
        GraphicManager.g.detail_init(target);
    }
}
