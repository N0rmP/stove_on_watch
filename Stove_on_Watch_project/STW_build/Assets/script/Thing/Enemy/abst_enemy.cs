using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading;

public abstract class abst_enemy : thing
{
    protected int actions_per_turn;

    protected /*readonly*/ List<abst_enemy_action> initial_action_list;
    protected List<abst_enemy_action> cur_action_list;
    protected List<abst_enemy_action> discarded_action_list;
    protected Queue<abst_enemy_action> next_actions;
    protected abst_chasable chase_module;

    public List<abst_enemy_action> cur_action_list_ { get { return cur_action_list; } }
    public List<abst_enemy_action> discarded_action_list_ { get { return discarded_action_list; } }
    public Queue<abst_enemy_action> next_actions_ { get { return next_actions; } }

    public enum enemy_tiers { normal, elite, root }
    private enemy_tiers enemy_tier;
    public enemy_tiers enemy_tier_ {
        get { return enemy_tier; } 
        set {
            enemy_tier = value;
            switch ((int)enemy_tier) {
                case 0:
                    max_hp = 100; break;
                case 1:
                    max_hp = 200; break;
                case 2:
                    max_hp = 300; break;
            }
        }
    }

    public abst_enemy(int x, int y) {
        //enemy_tier_ = enemy_tiers.normal;
        move_to(GameManager.g.get_map()[x, y]);
        passives = new List<abst_power>();
    }

    protected override void personal_init() {
        enemy_tier_ = enemy_tiers.normal;
        if (cur_action_list == null) { cur_action_list = new List<abst_enemy_action>(); } else { cur_action_list.Clear(); }
        if (discarded_action_list == null) { discarded_action_list = new List<abst_enemy_action>(); } else { discarded_action_list.Clear(); }
        if (next_actions == null) { next_actions = new Queue<abst_enemy_action>(); } else { next_actions.Clear(); }
    }

    public void engage() {
        GraphicManager.g.prepare_combat_board(this);
        GraphicManager.g.set_image_color(combat_board.transform.GetChild(2).gameObject, new Color(1f, 1f, 1f, 1f));
        GraphicManager.g.set_image(
            combat_board.transform.GetChild(2).gameObject,
            Resources.Load<Sprite>("Sprite/" + this.GetType().ToString())
            );
        GraphicManager.g.movings_add(combat_board, new Vector2(470f - GameManager.g.get_combat_enemies().get_number() * 45, 100f - GameManager.g.get_combat_enemies().get_number() * 15));
        GraphicManager.g.set_text(combat_board.transform.GetChild(0).GetChild(0).gameObject, cur_hp.ToString());
        //★enemy_image 교체, child 중 2번 인덱스에 있다
        foreach (abst_power psv in passives)
            power_add(psv);
        foreach (abst_power ap in powers)
            ap.on_combat_start();
        arrange_powers();
        list_reset();
        GameManager.g.get_combat_enemies().add(this);
        if (!GameManager.g.get_is_combat())
            GameManager.g.general_combat_start();
        //GraphicManager.g.notice_fade("Enemy engaged");
    }

    public void combat_escape() {
        abst_power[] temp_ap = new abst_power[powers.Count];
        powers.CopyTo(temp_ap, 0);
        foreach (abst_power ap in temp_ap) {
            power_remove(ap);
        }
        GraphicManager.g.movings_add(combat_board, new Vector2(1310f, 100f));
        GraphicManager.g.push_combat_board(combat_board);
        //combat_board.GetComponent<RectTransform>().localPosition = new Vector2(0, 1600);   //★적 여러 명 등장 시 어떻게?
    }

    protected virtual void upgrade() { }
    
    public virtual void act() {
        if ((cur_hp > 0) && !GameManager.g.get_is_prime_combat()) {
            if (combat_board != null) {
                combat_act();
            } else {
                map_move();
            }
        }
    }

    protected void combat_act() {
        int temp = next_actions.Count;
        abst_enemy_action temp_enemy_action;

        for (int i = 0; i < temp; i++) {
            temp_enemy_action = next_actions.Dequeue();
            GameManager.g.order_list.Enqueue(temp_enemy_action);
            discarded_action_list.Add(temp_enemy_action);
        }
        //GameManager.g.order_list.Enqueue(abst_enemy.ender);
    }

    public virtual void give_reward() {
        RewardList temp_r = GameManager.g.rew;
        switch ((int)enemy_tier) {
            case 0:
                for (int i = 0; i < 2; i++) { temp_r.reward_add_action(); }
                temp_r.reward_add_shards(GameManager.g.ran.xoshiro_range(80, 90) );   //★2단계 맵이라면 보상 증가
                break;
            case 1:
                //★정예 보상 준비
                GameManager.g.set_elite_defeated(true);
                temp_r.reward_add_heal();
                GameManager.g.set_is_prime_combat(false);
                break;
            case 2:
                //★GameManager에 게임 클리어 함수 마련 후, 그거 실행
                break;
        }
    }

    public void map_move() {
        int direction = chase_module.get_next_move();
        if (direction != -1 && combat_board==null) {
            move_to(location.get_link()[direction]);
            if (location.is_Plr_here())
                engage();
        } else {
            //Debug.Log("route not found");
        }
    }

    public void disappear() {
        combat_escape();
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

    #region get_set
    public override void set_cur_hp(bool is_plus, int val)  {
        base.set_cur_hp(is_plus, val);
        if (cur_hp <= 0 && (combat_board != null))
            GraphicManager.g.set_image_color(combat_board.transform.GetChild(2).gameObject, new Color(0.5f, 0.5f, 0.5f, 1f));
    }
    #endregion get_set
}
