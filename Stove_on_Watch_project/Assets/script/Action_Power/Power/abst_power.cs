using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class abst_power {
    protected thing owner;
    //assigned_block is graphic obejct that shows this power's information, power information is so involved in this class that it's here and GraphicManager manages available blocks collections
    protected GameObject assigned_block;
    protected bool is_visable;
    protected string title;
    protected string description;

    public abst_power(thing p_owner) {
        owner = p_owner;
        temp_json temp_j = GraphicManager.g.get_json("Power/" + this.GetType().ToString());
        title = temp_j.s1;
        description = temp_j.s2;
    }

    //★현 상태에서 power의 init 함수는 큰 필요가 없어 보인다, 추후에도 이러면 삭제할 것
    public virtual void init() { }

    //Plr card UI is only used by Plr and its update method is in its UI script, power is used by both Plr and enemy and its update method is in this class so that programmer can adjust its update timing
    public virtual void update_power() { }

    public void return_block() {
        owner.powers.Remove(this);
        if (owner.get_cur_hp() > 0)
            owner.arrange_powers();
        GraphicManager.g.push_power_block(assigned_block);
    }

    #region get_set
    public thing get_owner() {
        return owner;
    }
    public GameObject get_block() {
        return assigned_block;
    }
    public void set_block(GameObject g) {
        assigned_block = g;
    }
    public string get_title() {
        return title;
    }
    public string get_description() {
        return description;
    }
    #endregion get_set


    #region timing
    public virtual void on_combat_start() { }
    public virtual void on_Plr_turn_start() { }
    public virtual void on_action() { }
    public virtual int on_before_attack(thing receiver, int v) { return v; } //parameter 'receiver' exists for 'mother'
    public virtual int on_after_attack(int v) { return v; }
    public virtual int on_attacked(int v) { return v; }
    public virtual int on_before_hp_down(int v) { return v; }
    public virtual int on_after_hp_down(int v) { return v; }
    public virtual int on_before_hp_up(int v) { return v; }
    public virtual int on_after_hp_up(int v) { return v; }
    public virtual void on_Plr_turn_end() { }
    public virtual void on_enemy_turn__start() { }
    public virtual void on_enemy_turn_end() { }
    public virtual void on_ROLL() { }
    public virtual void on_cost_update() { }
    public virtual void on_attack_update() { }
    public virtual void on_block_update() { }
    /*public delegate void eff_none();
    private List<eff_none> on_combat_start;     //★on 배열 대신, 전투에 참여한 모든 캐릭터의 지정된 함수 실행
    private List<eff_none> on_Plr_turn_start;   //also describe 'on_enemy_turn_end' with barricade
    private List<eff_none> on_calm;
    private List<eff_none> on_Plr_turn_end;     //also describe 'on_enemy_turn_start' with barricade
    private List<eff_none> on_combat_end;
    //some on_ array is in thing class, they should be called only when its owner acts
    public delegate int eff_exist(int value);    //return conclusional value, it's used for describing card's numbers
    public List<eff_exist> on_action;   //!
    public List<eff_exist> on_attack;   //!
    public List<eff_exist> on_attacked;   //!
    public List<eff_exist> on_block;    //!
    public List<eff_exist> on_hp_down;  //!*/
    #endregion timing
}
