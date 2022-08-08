using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class abst_power
{
    protected int main_num;
    protected int sub_num;
    protected bool is_visable;


    public void init()
    {
        this.main_num = 0;
        this.sub_num = 0;
    }

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
}
