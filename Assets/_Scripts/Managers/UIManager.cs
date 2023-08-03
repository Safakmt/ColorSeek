using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject HidePanel;
    [SerializeField] private GameObject StartMenuPanel;
    [SerializeField] private GameObject LoseMenuPanel;
    [SerializeField] private GameObject WinMenuPanel;
    [SerializeField] private GameObject SeekMenuPanel;
    private GameObject _activePanel;

    private void OnEnable()
    {
        _activePanel = StartMenuPanel;
        EventManager.OnSceneLoad += GameStart;
        EventManager.OnSelectionStart += SelectionStart;
        EventManager.OnHuntingFinished += EndPanel;
        EventManager.OnSeekState += SeekPanel;
    }
    private void OnDisable()
    {
        EventManager.OnSceneLoad -= GameStart;
        EventManager.OnSelectionStart -= SelectionStart;
        EventManager.OnHuntingFinished -= EndPanel;
        EventManager.OnSeekState -= SeekPanel;
    }
    private void SeekPanel()
    {
        ActivatePanel(SeekMenuPanel);
    }
    private void SelectionStart()
    {
        ActivatePanel(HidePanel);
    }
    private void GameStart()
    {
        ActivatePanel(StartMenuPanel);
    }
    private void EndPanel(bool isPlayerCatch)
    {
        if (isPlayerCatch)
        {
            ActivatePanel(LoseMenuPanel);
        }
        else
        {
            ActivatePanel(WinMenuPanel);
        }
    }
    private void ActivatePanel(GameObject panel)
    {
        _activePanel.SetActive(false);
        _activePanel = panel;
        _activePanel.SetActive(true);
    }
}
