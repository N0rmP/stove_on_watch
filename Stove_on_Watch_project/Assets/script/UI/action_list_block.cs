using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class action_list_block : MonoBehaviour, IPointerEnterHandler
{
    private abst_enemy_action assigned_action;
    private abst_power assigned_power;
    public abst_enemy_action assigned_action_ {
        get { return assigned_action; }
        set {
            assigned_power = null;
            assigned_action = value;
            GraphicManager.g.set_text(this.transform.GetChild(0).gameObject, assigned_action.action_name_);
            this.gameObject.SetActive(true);
        }
    }
    public abst_power assigned_power_ {
        get { return assigned_power; }
        set {
            assigned_power = value;
            assigned_action = null;
            GraphicManager.g.set_text(this.transform.GetChild(0).gameObject, assigned_power.get_title());
            this.gameObject.SetActive(true);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (assigned_action != null)
            GraphicManager.g.show_action_list_description(assigned_action);
        if (assigned_power != null)
            GraphicManager.g.show_action_list_description(assigned_power);
    }
}
