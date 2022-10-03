using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using System;

public abstract class abst_Plr_action_image : MonoBehaviour
{
    protected abst_Plr_action target;

    public void card_update(abst_Plr_action a) {
        this.gameObject.SetActive(true);
        temp_json tj = GraphicManager.g.get_json(a.action_name_);
        this.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = tj.s1;
        this.gameObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = tj.s2;
        try{
            this.gameObject.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = tj.i1.ToString();
        }catch (Exception e) { }
    }

    #region get_set
    public abst_Plr_action get_target() { return target; }
    public void set_target(abst_Plr_action a) { this.target = a; }
    #endregion get_set
}
