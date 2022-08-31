using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class abst_action
{
    protected abst_power power_made_by_this;
    //protected thing owner;    ★enemy action에도 전용 owner를 만들 것

    public abstract void use();
    public virtual void update() { }    //update its description or max cost
}