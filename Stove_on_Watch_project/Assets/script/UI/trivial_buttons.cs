using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trivial_buttons : MonoBehaviour
{
    private abst_action ender;
    public trivial_buttons() { this.ender = new turn_end(); }

    public void turn_end() { GameManager.g.get_order_list().Enqueue(this.ender);}
    public void reward_end() { 
        GraphicManager.g.temp_reward_remove();
        GameManager.g.rew.init();
    }

    public void inventory_end() {
        GraphicManager.g.temp_inventory_remove();
    }

    public void detail_end() {
        GraphicManager.g.temp_detail_remove();
    }

    public void detail_confirm() {
        player p = GameManager.g.get_Plr();
        p.shards -= 100;
        p.actions_.Add(GraphicManager.g.inventory_selection_);
        p.action_inventory_.Remove(GraphicManager.g.inventory_selection_);
        GraphicManager.g.temp_detail_remove();
        GraphicManager.g.inventory_update();
    }

    public void temp_inventory() {
        GraphicManager.g.inventory_update();
        GraphicManager.g.temp_inventory_recover();
    }
}