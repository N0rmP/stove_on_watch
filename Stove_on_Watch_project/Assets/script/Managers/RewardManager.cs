using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager
{
    private List<abst_Plr_action> rewards_action;
    private List<abst_tool> rewards_tool;
    private int rewards_shard;


    public RewardManager() {
        rewards_action = new List<abst_Plr_action>();
        rewards_tool = new List<abst_tool>();
        rewards_shard = 0;
    }

    public void init() {
        rewards_action.Clear();
        rewards_tool.Clear();
        rewards_shard = 0;
    }

    public void reward_init() {
        GraphicManager temp_g = GraphicManager.g;
        int temp = 0;
        try {   //get rewards from GameManager and set it into reward_buttons
            foreach (abst_Plr_action a in rewards_action) { 
                temp_g.reward_buttons_[temp].GetComponent<reward_button>().set_reward(a);
                temp_g.reward_buttons_[temp].SetActive(true);
                temp++;
            }
            foreach (abst_tool a in rewards_tool) { 
                temp_g.reward_buttons_[temp].GetComponent<reward_button>().set_reward(a);
                temp_g.reward_buttons_[temp].SetActive(true);
                temp++;
            }
            temp_g.reward_buttons_[temp].GetComponent<reward_button>().set_reward(rewards_shard);
            temp_g.reward_buttons_[temp].SetActive(true);
            while (++temp < 10) {
                temp_g.reward_buttons_[temp].SetActive(false);
            }
        } catch (Exception e) {
            Debug.Log(temp+"th reward setting error : " + e);
        }
        GraphicManager.g.reward_update();
        GraphicManager.g.temp_reward_recover();
    }

    public void reward_add_action() {
        rewards_action.Add(LibraryManager.li.return_action());
    }
    public void reward_add(abst_Plr_action a) {
        rewards_action.Add(a);
        //Plr.actions.Add((abst_Plr_action)rewards[0]); ¡Úuse it
    }
    public void reward_add_tool() {
        rewards_tool.Add(LibraryManager.li.return_tool());
    }
    public void reward_add(abst_tool t) {
        rewards_tool.Add(LibraryManager.li.return_tool());
    }
    public void reward_add(int i) {
        rewards_shard += i;
    }

}
