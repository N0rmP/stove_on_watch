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
    public int shards;  //shard is described only by int
    public List<abst_Plr_action> action_inventory_ { get { return action_inventory; } }
    public List<abst_tool> tool_inventory_ { get { return tool_inventory; } }
    public List<abst_Plr_action> actions_ { get { return actions; } }

    public player() { init(); } //�ڰ��� ���� �� ������ �ʱ�ȭ�Ѵٸ� �̰��� �ʿ����

    protected override void personal_init()
    {
        max_hp = initial_max_hp;
        set_cur_hope(false, initial_max_hope);
        shards = 0;
        if (action_inventory == null) { action_inventory = new List<abst_Plr_action>(); } else { action_inventory.Clear(); }
        if (actions == null) { actions = new List<abst_Plr_action>(); } else { actions.Clear(); }
    }

    #region get_set
    public override void set_cur_hp(bool is_plus, int i) {
        if (is_plus)
            cur_hp += i;
        else
            cur_hp = i;
        GraphicManager.g.set_text(GraphicManager.g.Plr_hp, cur_hp.ToString());
    }
    public int get_cur_hope() { return cur_hope; }
    public void set_cur_hope(bool is_plus, int i) { 
        if (is_plus) 
            cur_hope += i; 
        else 
            cur_hope = i; 
        GraphicManager.g.set_text(GraphicManager.g.Plr_hope, cur_hope.ToString());
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
