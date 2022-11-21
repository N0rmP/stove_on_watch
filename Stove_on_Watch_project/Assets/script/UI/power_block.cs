using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class power_block : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private abst_power assigned_power;

    public abst_power get_power(){
        return assigned_power;
    }

    public void set_power(abst_power ap) {
        assigned_power = ap;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        GraphicManager.g.show_power_information(this);
    }

    public void OnPointerExit(PointerEventData eventData) {
        GraphicManager.g.power_information.SetActive(false);
    }
}
