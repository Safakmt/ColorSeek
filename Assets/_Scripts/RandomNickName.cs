using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RandomNickName : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    [SerializeField] private List<string> _names = new List<string> { 
        "thehustler","ryethedude","darkmatter","lionabloom","gollumwind","trailbat","batpanda","pigearth","beeolive","farmland","floral","amélie","melody","saltspider","mementofog","blackberry","lavaplanet","kidneymars","pancakesswan","onionchicken"};

    private void OnEnable()
    {
        EventManager.OnSceneLoad += GetRandomName;
    }
    private void OnDisable()
    {
        EventManager.OnSceneLoad -= GetRandomName;
    }

    private void GetRandomName()
    {
        _textMeshPro.text = _names[Random.Range(0,_names.Count)];
    }

}
