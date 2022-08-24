using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class edge : MonoBehaviour
{
    public void liner(Vector3 start, Vector3 end) {
        RectTransform temp = this.gameObject.GetComponent<RectTransform>();
        //Debug.Log($"start : {start}, end :  {end}");

        Vector3 direction = end - start;
        temp.sizeDelta = new Vector2(direction.magnitude, 10f);
        temp.pivot = new Vector2(0f, 0.5f);
        temp.localPosition = start;

        float temp_angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        temp.rotation = Quaternion.Euler(0, 0, temp_angle);
    }
}
