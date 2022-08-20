using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject newCaseFilePanel, accessCaseFilePanel, accessPrompt;
    public bool hasAccess;

    private void Awake()
    {
        CaseFilePanelSetsActive();
        accessPrompt.SetActive(false);
    }

    public void PlayGame()
    {
        // only if user does not have Access
        if (!hasAccess)
        {
            newCaseFilePanel.SetActive(true);
            AccessPromptSetActive();
        }
        else
        {
            // load into current scene
            print("Loading into current scene...");
            CaseFilePanelSetsActive();
        }
    }

    public void CaseFilePanelSetsActive()
    {
        newCaseFilePanel.SetActive(false);
        accessCaseFilePanel.SetActive(false);
    }

    private void AccessPromptSetActive()
    {
        if (accessPrompt.activeInHierarchy)
        {
            accessPrompt.SetActive(false);
            CancelInvoke(nameof(AccessPrompt));   
        }
        else
        {
            accessPrompt.SetActive(true);
            Invoke(nameof(AccessPrompt), 2f);
        }
    }

    private void AccessPrompt()
    {
        accessPrompt.SetActive(false);
    }
}
