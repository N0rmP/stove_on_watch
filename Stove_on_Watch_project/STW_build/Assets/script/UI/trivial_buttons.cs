using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class trivial_buttons : MonoBehaviour
{
    public void recover_action_list() {
        GraphicManager.g.show_action_list();
    }

    public void remove_action_list() {
        GraphicManager.g.hide_action_list();
    }

    public void turn_end() {
        if (GameManager.g.get_is_combat())
            GameManager.g.get_order_list().Enqueue(
                GameManager.g.TE);
        else {
            GameManager.g.set_selected_node(null);
        }

    }
    public void reward_end() { 
        GraphicManager.g.reward_remove();
        GameManager.g.rew.init();
    }

    public void inventory_end() {
        GraphicManager.g.inventory_remove();
    }

    public void detail_end() {
        GraphicManager.g.detail_remove();
    }

    public void event_end() {
        if (!GameManager.g.cur_event.force_) {
            GraphicManager.g.event_remove();
            GameManager.g.cur_event.choice_complete = true;
            GameManager.g.cur_event.is_event_end_ = true;
        }
    }

    public void detail_confirm() {
        player p = GameManager.g.get_Plr();
        p.set_shards(true, -100);
        p.actions_.Add(GraphicManager.g.inventory_selection_);
        p.action_inventory_.Remove(GraphicManager.g.inventory_selection_);
        GraphicManager.g.inventory_selection_.acquired();
        GraphicManager.g.detail_remove();
        GraphicManager.g.inventory_update();
    }

    public void return_to_menu() {
        SceneManager.LoadScene("Scenes/Menu");
    }

    public void temp_inventory() {
        GraphicManager.g.inventory_update();
        GraphicManager.g.inventory_recover();
    }
}