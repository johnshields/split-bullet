using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject newCaseFilePanel, accessCaseFilePanel, accessPrompt;
    public Button restartBtn;
    public TextMeshProUGUI passwordInput, passwordPrompt;
    public TMP_InputField passwordField;
    public bool hasAccess;
    private string msg;
    private bool _passwordInputActive;

    private void Awake()
    {
        CaseFilePanelSetsActive();
        accessPrompt.SetActive(false);
        passwordInput.text = string.Empty;
    }

    private void Update()
    {
        restartBtn.interactable = hasAccess;
        PasswordInputActive();
    }

    private void OnGUI()
    {
        passwordPrompt.text = msg;
        if (_passwordInputActive)
            ValidatePassword();
    }

    private void PasswordInputActive()
    {
        _passwordInputActive = passwordField.text != string.Empty;
    }

    // ref - https://stackoverflow.com/a/43085890
    private void ValidatePassword()
    {
        var hasNumber = new Regex(@"[0-9]+");
        var hasUpperChar = new Regex(@"[A-Z]+");
        var hasMiniMaxChars = new Regex(@".{8,}");
        var hasLowerChar = new Regex(@"[a-z]+");
        var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

        if (!hasLowerChar.IsMatch(passwordField.text)) msg = "Password should contain at least one lower case letter";
        else if (!hasUpperChar.IsMatch(passwordField.text))
            msg = "Password should contain at least one upper case letter";
        else if (!hasMiniMaxChars.IsMatch(passwordField.text))
            msg = "Password should not be less than or greater than 8 characters";
        else if (!hasNumber.IsMatch(passwordField.text)) msg = "Password should contain at least one numeric value";
        else if (!hasSymbols.IsMatch(passwordField.text))
            msg = "Password should contain at least one special case characters";
        else msg = string.Empty;
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