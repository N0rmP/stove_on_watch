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

    //protected static abst_action ender = new turn_end();

    public enum enemy_tiers { normal, elite, root }
    protected enemy_tiers enemy_tier;
    public enemy_tiers enemy_tier_ { get; }

    protected override void personal_init() {
        switch ((int)enemy_tier)
        {
            case 0:
                max_hp = 100; break;
            case 1:
                max_hp = 200; break;
            case 2:
                max_hp = 300; break;
        }
        if (cur_action_list == null) { cur_action_list = new List<abst_enemy_action>(); } else { cur_action_list.Clear(); }
        if (discarded_action_list == null) { discarded_action_list = new List<abst_enemy_action>(); } else { discarded_action_list.Clear(); }
        if (next_actions == null) { next_actions = new Queue<abst_enemy_action>(); } else { next_actions.Clear(); }
    }
    protected virtual void upgrade() { }

    public void act() {
        int temp = next_actions.Count;
        abst_enemy_action temp_enemy_action;

        for (int i=0; i<temp; i++) {
            temp_enemy_action = next_actions.Dequeue();
            GameManager.g.order_list.Enqueue(temp_enemy_action);
            discarded_action_list.Add(temp_enemy_action);
        }
        //GameManager.g.order_list.Enqueue(abst_enemy.ender);
    }

    public void give_reward() {
        RewardList temp_r = GameManager.g.rew;
        switch ((int)enemy_tier) {
            case 0:
                for (int i = 0; i < 3; i++) { temp_r.reward_add_action(); }
                temp_r.reward_add(GameManager.g.ran.xoshiro_range(100, 120) );   //��2�ܰ� ���̶�� ���� ����
                break;
            //������ ���� �غ�, �ٿ� ������ ���� Ŭ�����̹Ƿ� GameManager Ŭ�������� �غ��� ��
        }
    }

    public void disappear() {
        location.remove_thing(this);
        GameManager.g.remove_wondering_enemy(this);
    }

    #region action_list
    public void action_choice() {
        int temp;
        for (int i = 0; i < actions_per_turn; i++)
        {
            temp = GameManager.g.ran.xoshiro_range(cur_action_list.Count);
            next_actions.Enqueue(cur_action_list[temp]);
            cur_action_list.RemoveAt(temp);
            if (cur_action_list.Count <= 0) {
                list_reset();
            }
        }
    }

    protected void list_reset() {
        if (discarded_action_list.Count > 0) {
            //���ൿ�� ������ ������ �迭�ǰ� ���� ��
            cur_action_list = discarded_action_list.ToList();
            discarded_action_list.Clear();
        } else {
            cur_action_list = initial_action_list.ToList<abst_enemy_action>();
        }
    }
    #endregion action_list
}
