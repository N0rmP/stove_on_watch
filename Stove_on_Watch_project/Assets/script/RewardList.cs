using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;

public class RewardList
{
    private List<abst_Plr_action> rewards_action;
    private List<abst_tool> rewards_tool;
    private int rewards_shard;

    public List<abst_Plr_action> rewards_action_ { get{ return rewards_action; } }
    public List<abst_tool> rewards_tool_ { get { return rewards_tool; } }
    public int rewards_shard_ { get { return rewards_shard; } }

    public RewardList() {
        rewards_action = new List<abst_Plr_action>();
        rewards_tool = new List<abst_tool>();
        rewards_shard = 0;
    }

    public void init() {
        rewards_action.Clear();
        rewards_tool.Clear();
        rewards_shard = 0;
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
