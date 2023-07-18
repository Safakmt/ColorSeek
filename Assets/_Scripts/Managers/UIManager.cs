using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject HidePanel;
    [SerializeField] private GameObject StartMenuPanel;
    [SerializeField] private GameObject EndMenuPanel;
    private GameObject _activePanel;

    private void Start()
    {
        _activePanel = StartMenuPanel;
    }
    private void OnEnable()
    {
        EventManager.OnGameStart += GameStart;
    }
    private void OnDisable()
    {
        EventManager.OnGameStart -= GameStart;
    }

    private void GameStart()
    {
        ActivatePanel(HidePanel);
    }
    private void ActivatePanel(GameObject panel)
    {
        _activePanel.SetActive(false);
        _activePanel = panel;
        _activePanel.SetActive(true);
    }
}
