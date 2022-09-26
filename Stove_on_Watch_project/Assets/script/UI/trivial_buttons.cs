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
        Debug.Log("reward end"); 
    }
}