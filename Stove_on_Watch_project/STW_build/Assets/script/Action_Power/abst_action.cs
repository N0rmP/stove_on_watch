using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class abst_action
{
    protected string action_name;
    protected string action_description;
    public string action_name_ { get { return action_name; } }
    public virtual string action_description_ { get { return action_description; } }

    protected abst_power power_made_by_this;
    //protected thing owner;    ★enemy action에도 전용 owner를 만들 것

    public abst_action() {
        string whose_action = "";
        if (this.GetType().IsSubclassOf(typeof(abst_Plr_action))) {
            whose_action = "Player_Action/";
        } else if (this.GetType().IsSubclassOf(typeof(abst_enemy_action))) {
            whose_action = "Enemy_Action/";
        } 

        temp_json temp_j = GraphicManager.g.get_json(whose_action + this.GetType().ToString());
        action_name = temp_j.s1;
        action_description = temp_j.s2;
    }

    public abstract void use(); //★버튼을 클릭하자마자 사용불가능하게 만들 구조 강구
    public virtual void update() { }    //update its description or max cost
}