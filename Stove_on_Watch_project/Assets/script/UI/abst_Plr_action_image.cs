using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using System;

public abstract class abst_Plr_action_image : MonoBehaviour
{
    protected abst_Plr_action target;

    public void card_update() {
        this.gameObject.SetActive(true);
        this.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = target.action_name_;
        this.gameObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = target.action_description_;
        this.gameObject.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = target.get_cost().ToString();

        if (this.GetType() != typeof(reward_button)) {
            if (target.get_cur_cooltime() > 0) {
                this.gameObject.transform.GetChild(4).gameObject.SetActive(true);
                this.gameObject.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = target.get_cur_cooltime().ToString();
            } else {
                this.gameObject.transform.GetChild(4).gameObject.SetActive(false);
            }
        }
    }

    public void card_update(abst_Plr_action ap) {
        target = ap;
        card_update();
    }

    #region get_set
    public abst_Plr_action get_target() { return target; }
    public void set_target(abst_Plr_action a) { this.target = a; }
    #endregion get_set
}
