using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : thing
{
    private const int initial_max_hp = 20;
    private const int initial_max_hope = 1400;
    private int max_hope;
    private int cur_hope;

    //inventory divides into 2+1, incomplete_action / tool / actions(complete and available ones)
    private List<abst_Plr_action> action_inventory;
    public List<abst_tool> tool_inventory;
    public List<abst_Plr_action> actions;
    private int shards;  //shard is described only by int
    public List<abst_Plr_action> action_inventory_ { get { return action_inventory; } }
    public List<abst_tool> tool_inventory_ { get { return tool_inventory; } }
    public List<abst_Plr_action> actions_ { get { return actions; } }

    public player() {
        init(); 
    }

    protected override void personal_init()
    {
        if(combat_board == null)
            combat_board = GraphicManager.g.Plr_board;
        max_hp = initial_max_hp;
        set_cur_hope(false, initial_max_hope);
        shards = 0;
        if (action_inventory == null) { action_inventory = new List<abst_Plr_action>(); } else { action_inventory.Clear(); }
        if (actions == null) { actions = new List<abst_Plr_action>(); } else { actions.Clear(); }
    }

    public new void move_to(node n) {
        n.set_visited(true);
        base.move_to(n);
    }

    #region get_set
    public int get_cur_hope() { return cur_hope; }
    public void set_cur_hope(bool is_plus, int i) { 
        if (is_plus) 
            cur_hope += i; 
        else 
            cur_hope = i;
        GraphicManager.g.set_text(combat_board.transform.GetChild(1).GetChild(0).gameObject, cur_hope.ToString());
    }
    public int get_shards() { return shards; }
    public void set_shards(bool is_plus, int i) {
        if (is_plus)
            shards += i;
        else
            shards = i;

        if (shards < 0)
            shards = 0;
    }
    public void add_tool(abst_tool a) {
        if (tool_inventory.Count < 5) { //�ڵ��� �κ��丮 Ȯ�� ���¶�� ���� ����
            tool_inventory.Add(a);
        } else { 
            //�ڵ��� �ϳ� ������ ���� �� �ֵ��� ó��
        }
    }
    #endregion get_set
}
