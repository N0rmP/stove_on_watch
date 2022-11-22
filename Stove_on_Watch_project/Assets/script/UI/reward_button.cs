using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reward_button : abst_Plr_action_image
{
    public void set_reward(abst_Plr_action a) { target = a; }

    public void reward_taken() {
        if ((target.GetType() == typeof(shards)) || (target.GetType()== typeof(take_a_breath))){
            target.use();
        } else {
            GameManager.g.get_Plr().action_inventory_.Add(target);
        }

        this.gameObject.SetActive(false);
        GraphicManager.g.reward_update();
    }
}
