using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class HunterMovementController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private PlayerController player;
    [SerializeField] private List<GameObject> hidingList;
    [SerializeField] private int seekingCount;
    private List<ISticker> _stickerList = new List<ISticker>();
    private List<GameObject> _seekedList = new List<GameObject>();
    private void Start()
    {
        foreach (var item in hidingList)
        {
            _stickerList.Add(item.GetComponent<ISticker>());
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            StartCoroutine(Seek());
        }
    }

    IEnumerator Seek()
    {
        while (seekingCount > 0)
        {
            GameObject hiding = hidingList[Random.Range(0, hidingList.Count)];
            Debug.Log("walking");
            agent.destination = hiding.transform.position;
            yield return new WaitUntil(() => Vector3.Distance(transform.position,agent.destination) <= 2f);
            hidingList.Remove(hiding);
            _seekedList.Add(hiding);
            hiding.SetActive(false);
            Debug.Log("found hiding one");
            seekingCount = seekingCount - 1;
        }
    }
}
