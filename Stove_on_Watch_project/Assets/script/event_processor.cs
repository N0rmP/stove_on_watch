using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class event_processor : MonoBehaviour
{
    public void event_call(string s) {
        StartCoroutine(s);
    }

    private IEnumerator temp_event() {
        Debug.Log("event testing");
        yield return null;
    }
}
