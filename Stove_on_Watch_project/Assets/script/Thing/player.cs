using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : thing
{
    private const int initial_max_hp = 20;
    private int max_hope;
    private int cur_hope;

    //inventory divides into 2+1, incomplete_action / tool / actions(complete and available ones)
    private List<abst_Plr_action> action_inventory;
    public List<abst_tool> tool_inventory;
    public List<abst_Plr_action> actions;
    public int shards;  //shard is described only by int

    public player() { init(); } //★게임 시작 시 어차피 초기화한다면 이것은 필요없다

    protected override void personal_init()
    {
        max_hp = initial_max_hp;
        shards = 0;
        //★if (inventory == null) { inventory = new List<abst_Plr_action>(); } else { inventory.Clear(); }
        if (actions == null) { actions = new List<abst_Plr_action>(); } else { actions.Clear(); }
    }

    #region get_set
    public int get_cur_hope() { return cur_hope; }
    public void set_cur_hope(bool is_plus, int i) { if (is_plus) { cur_hope += i; } else { cur_hope = i; } }
    #endregion get_set
}
