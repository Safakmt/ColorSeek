using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SeekPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _huntedName;
    Coroutine activeCoroutine;
    private void OnEnable()
    {
        _huntedName.enabled = false;
        EventManager.OnHuntedName += ShowHuntedName;
    }
    private void OnDisable()
    {
        EventManager.OnHuntedName -= ShowHuntedName;
    }
    private void ShowHuntedName(string name)
    {
       if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
        }
       _huntedName.text = name + " Caught!";
       activeCoroutine = StartCoroutine(NameShowDuration(2f));
    }

    IEnumerator NameShowDuration(float duration)
    {
        _huntedName.enabled = true;
        yield return new WaitForSeconds(duration);
        _huntedName.enabled = false;
    }
}
