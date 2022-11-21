using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reward_button : abst_Plr_action_image
{
    int shards_amount;
    /*
    private abst_Plr_action reward1;
    private abst_tool reward2;
    private int reward3;*/

    public void set_reward(abst_Plr_action a) { target = a; /*reward2 = null; reward3 = 0;*/ }
    public void set_reward(shards s) { target = s; shards_amount = s.amount_; }
    //public void set_reward(abst_tool a) { reward1 = null; reward2 = a; reward3 = 0; }
    //public void set_reward(int i) { reward1 = null; reward2 = null; reward3 = i; }

    public void reward_taken() {
        /*
        if (reward1 != null) {
            GameManager.g.get_Plr().action_inventory_.Add(reward1);
        } else if (reward2 != null) {
            GameManager.g.get_Plr().add_tool(reward2);
        } else if (reward3 > 0) {
            GameManager.g.get_Plr().shards += reward3;
        }*/
        if (target.GetType() == typeof(shards)) {
            GameManager.g.get_Plr().shards += shards_amount;
            shards_amount = 0;
        } else {
            GameManager.g.get_Plr().action_inventory_.Add(target);
        }

        this.gameObject.SetActive(false);
        GraphicManager.g.reward_update();
    }
}
