using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class abst_Plr_action_image : MonoBehaviour
{
    private abst_Plr_action target;

    #region get_set
    public abst_Plr_action get_target() { return target; }
    public void set_target(abst_Plr_action a) { this.target = a; }
    #endregion get_set
}
