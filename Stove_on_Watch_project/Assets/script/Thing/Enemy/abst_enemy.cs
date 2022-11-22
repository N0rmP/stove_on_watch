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
    protected List<abst_power> passives;

    public List<abst_enemy_action> cur_action_list_ { get { return cur_action_list; } }
    public List<abst_enemy_action> discarded_action_list_ { get { return discarded_action_list; } }
    public Queue<abst_enemy_action> next_actions_ { get { return next_actions; } }
    public List<abst_power> passives_ { get { return passives; } }

    protected abst_chasable chase_module;

    //protected static abst_action ender = new turn_end();

    public enum enemy_tiers { normal, elite, root }
    protected enemy_tiers enemy_tier;
    public enemy_tiers enemy_tier_ { get; }

    public abst_enemy(int x, int y) {
        move_to(GameManager.g.get_map()[x, y]);
        passives = new List<abst_power>();
    }

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
        chase_module = new near_chaser(this);
    }

    public void engage() {
        GraphicManager.g.prepare_combat_board(this);
        combat_board.GetComponent<RectTransform>().anchoredPosition = new Vector2(-450,100);   //★적 여러 명 등장 시 어떻게?
        foreach (abst_power psv in passives) {
            powers.Add(psv);
        }
        foreach (abst_power ap in powers) {
            GraphicManager.g.prepare_power_block(ap);
        }
        arrange_powers();
        list_reset();
    }

    public void combat_escape() {
        GraphicManager.g.push_combat_board(combat_board);
        combat_board.GetComponent<RectTransform>().localPosition = new Vector2(0, 1600);   //★적 여러 명 등장 시 어떻게?
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
                temp_r.reward_add_shards(GameManager.g.ran.xoshiro_range(100, 120) );   //★2단계 맵이라면 보상 증가
                break;
            case 1:
                //★정예 보상 준비
                GameManager.g.set_elite_defeated(true);
                temp_r.reward_add_heal();
                break;
            case 2:
                //★GameManager에 게임 클리어 함수 마련 후, 그거 실행
                break;
        }
    }

    public void map_move() {
        int direction = chase_module.get_next_move();
        if (direction != -1)
            move_to(location.get_link()[direction]);
        else {
            //Debug.Log("route not found");
        }
    }

    public void disappear() {
        location.remove_thing(this);
        GameManager.g.remove_wandering_enemy(this);
    }

    #region action_list
    public void action_choice() {
        int temp;
        for (int i = 0; i < actions_per_turn; i++)
        {
            if (cur_action_list.Count <= 0)
                list_reset();
            temp = GameManager.g.ran.xoshiro_range(cur_action_list.Count);
            next_actions.Enqueue(cur_action_list[temp]);
            cur_action_list.RemoveAt(temp);
            
        }
    }

    public void list_reset() {
        if (discarded_action_list.Count > 0) {
            //★행동이 일정한 순서로 배열되게 만들 것
            cur_action_list = discarded_action_list.ToList();
            discarded_action_list.Clear();
        } else {
            cur_action_list = initial_action_list.ToList<abst_enemy_action>();
        }
    }
    #endregion action_list
}
