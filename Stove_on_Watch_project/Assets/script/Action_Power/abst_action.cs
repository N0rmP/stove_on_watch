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
    //protected thing owner;    ��enemy action���� ���� owner�� ���� ��

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

    public abstract void use(); //�ڹ�ư�� Ŭ�����ڸ��� ���Ұ����ϰ� ���� ���� ����
    public virtual void update() { }    //update its description or max cost
}