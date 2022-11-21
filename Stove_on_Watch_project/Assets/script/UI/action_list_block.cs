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
            classify(1);
            assigned_power = value;
            assigned_action = null;
            GraphicManager.g.set_text(this.transform.GetChild(0).gameObject, assigned_power.get_title());
            this.gameObject.SetActive(true);
        }
    }
    private bool is_next;
    public bool is_next_ {
        set { is_next = value; }
    }
    private float glow_val;

    public action_list_block() {
        is_next = false;
    }

    public void classify(int code) {
        switch (code) {
            case 0: //cur_list, discarded_list
                GraphicManager.g.set_image_color(this.gameObject, new Color(0.2f, 0.2f, 0.2f, 1f));
                is_next = false;
                return;
            case 1: //passives
                GraphicManager.g.set_image_color(this.gameObject, new Color(0f, 0f, 0f, 1f));
                is_next = false;
                return;
            case 2: //next_list
                GraphicManager.g.set_image_color(this.gameObject, new Color(0f, 0f, 0f, 1f));
                is_next = true;
                glow_val = 0.005f;
                return;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (assigned_action != null)
            GraphicManager.g.show_action_list_description(assigned_action);
        if (assigned_power != null)
            GraphicManager.g.show_action_list_description(assigned_power);
    }

    public void FixedUpdate() {
        if (is_next) {
            this.gameObject.GetComponent<Image>().color += new Color(glow_val, 0f, 0f, 1f);
            if ((this.gameObject.GetComponent<Image>().color.r > 0.9f) || (this.gameObject.GetComponent<Image>().color.r < 0.1f))
                glow_val *= -1;
        }
    }
}
