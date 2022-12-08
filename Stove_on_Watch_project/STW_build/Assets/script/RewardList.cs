using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;

public class RewardList
{
    private List<abst_Plr_action> rewards_action;
    //private List<abst_tool> rewards_tool;
    private shards rewards_shards;
    private take_a_breath rewards_heal;

    public List<abst_Plr_action> rewards_action_ { get{ return rewards_action; } }
    //public List<abst_tool> rewards_tool_ { get { return rewards_tool; } }
    public shards rewards_shard_ { get { return rewards_shards; } }

    public RewardList() {
        rewards_action = new List<abst_Plr_action>();
        //rewards_tool = new List<abst_tool>();
        rewards_shards = new shards();
        rewards_heal = new take_a_breath();
    }

    public void init() {
        rewards_action.Clear();
        //rewards_tool.Clear();
        rewards_shards.amount_ = 0;
    }

    public void reward_add_action() {
        rewards_action.Add(LibraryManager.li.return_action());
    }
    public void reward_add(abst_Plr_action a) {
        rewards_action.Add(a);
        //Plr.actions.Add((abst_Plr_action)rewards[0]); ¡Úuse it
    }
    //public void reward_add_tool() { rewards_tool.Add(LibraryManager.li.return_tool()); }
    //public void reward_add(abst_tool t) { rewards_tool.Add(LibraryManager.li.return_tool()); }
    public void reward_add_shards(int i = 0) {
        rewards_shards.amount_ = i;
        rewards_action.Add(rewards_shards);
    }
    public void reward_add_heal() {
        rewards_action.Add(rewards_heal);
    }
}
