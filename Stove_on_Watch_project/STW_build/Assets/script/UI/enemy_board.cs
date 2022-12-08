using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class enemy_board : MonoBehaviour, IScrollHandler {
    public void OnScroll(PointerEventData eventData) {

        if (eventData.scrollDelta.y > 0) {
            GameManager.g.get_combat_enemies().next();
        } else if (eventData.scrollDelta.y < 0) {
            GameManager.g.get_combat_enemies().post();
        }

    }
}
