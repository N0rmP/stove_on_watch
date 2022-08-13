using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class abst_enemy : thing
{
    protected int actions_per_turn;

    protected /*readonly*/ List<abst_enemy_action> initial_action_list;
    protected List<abst_enemy_action> cur_action_list;
    protected List<abst_enemy_action> discarded_action_list;
    protected Queue<abst_enemy_action> next_actions;

    private List<abst_power> passives;

    protected static abst_action ender = new turn_end();

    public enum enemy_tiers { normal, elite, boss }
    protected enemy_tiers enemy_tier;

    public void init() {
        switch ((int)this.enemy_tier)
        {
            case 0:
                this.max_hp = 100; break;
            case 1:
                this.max_hp = 200; break;
            case 2:
                this.max_hp = 300; break;
        }
        if (this.cur_action_list == null) { this.cur_action_list = new List<abst_enemy_action>(); } else { this.cur_action_list.Clear(); }
        if (this.discarded_action_list == null) { this.discarded_action_list = new List<abst_enemy_action>(); } else { this.discarded_action_list.Clear(); }
        if (this.next_actions == null) { this.next_actions = new Queue<abst_enemy_action>(); } else { this.next_actions.Clear(); }
        base.init();
    }
    protected virtual void upgrade() { }

    public void act() {
        int temp = this.next_actions.Count;
        abst_enemy_action temp_enemy_action;

        for (int i=0; i<temp; i++) {
            temp_enemy_action = this.next_actions.Dequeue();
            GameManager.g.order_list.Enqueue(temp_enemy_action);
            this.discarded_action_list.Add(temp_enemy_action);
        }
        GameManager.g.order_list.Enqueue(abst_enemy.ender);
    }

    #region action_list
    protected void action_choice() {
        for (int i = 0; i < this.actions_per_turn; i++)
        {
            int temp = GameManager.g.ran.xoshiro_range(this.cur_action_list.Count);
            this.next_actions.Enqueue(this.cur_action_list[temp]);
            this.cur_action_list.RemoveAt(temp);
            if (this.cur_action_list.Count <= 0) {
                this.list_reset();
            }
        }
    }

    protected void list_reset() {
        //★행동이 일정한 순서로 배열되게 만들 것
        this.cur_action_list = this.discarded_action_list.ToList();
        this.discarded_action_list.Clear();
    }
    #endregion action_list
}
