using UnityEngine;
using TMPro;
using SmartLocalization.Editor;
using System.Text.RegularExpressions;
using SmartLocalization;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;

public class TitleLevelHUD : MonoSingleton<TitleLevelHUD>
{
    public CanvasGroup canvasGroup;
    public LocalizedTextMeshPro GroupLevelName;
    public TextMeshProUGUI ChapterName;
    public LocalizedTextMeshPro LevelName;
    public LocalizedText CompleteName;

    public Animator anim;

    Regex defaultWorldRegex = new Regex(@"W\d+L\d+-\d+");
    Regex numbersRegex = new Regex(@"\d+");

    public void Show(string GropulLevelNameText, string ChapterNameText, string LevelNameText)
    {
        ;
        string levelName = SceneManager.GetActiveScene().name;

        if (defaultWorldRegex.IsMatch(levelName))
        {
            world = 1;
            chapter = 1;
            section = 1;

            // Get world
            Match match = numbersRegex.Match(levelName);
            if (match.Success) world = int.Parse(match.Value);

            // Get chapter
            match = match.NextMatch();
            if (match.Success) chapter = int.Parse(match.Value);

            // Get section
            match = match.NextMatch();
            if (match.Success) section = int.Parse(match.Value);

            GroupLevelName.UpdateKey("Wname." + world.ToString("00"));
            ChapterName.text = chapter.ToString();
            LevelName.UpdateKey("LName" + world.ToString("00") + "." + chapter.ToString("00"));
        }
        else
        {
            GroupLevelName.UpdateKey(GropulLevelNameText);
            ChapterName.text = ChapterNameText.Substring(ChapterNameText.Length - 1);
            LevelName.UpdateKey(LevelNameText);
        }

        anim.SetTrigger("Show");
    }

    public void SetCompleteName(string CompleteNameText, bool forceKey = false)
    {
        string levelName = SceneManager.GetActiveScene().name;

        if (!forceKey && defaultWorldRegex.IsMatch(levelName))
        {
            world = 1;
            chapter = 1;
            section = 1;

            // Get world
            Match match = numbersRegex.Match(levelName);
            if (match.Success) world = int.Parse(match.Value);

            // Get chapter
            match = match.NextMatch();
            if (match.Success) chapter = int.Parse(match.Value);

            // Get section
            match = match.NextMatch();
            if (match.Success) section = int.Parse(match.Value);

            // Set the key
            Assert.IsNotNull(CompleteName);
            CompleteName.UpdateKey("Misc02");
        }
    }

    int world = 1, chapter = 1, section = 1;

    void ChangeText()
    {
        Assert.IsNotNull(CompleteName);

        string replace = "";

        replace = CompleteName.ObjectText;

        replace = replace.Replace("<ROOM>", chapter.ToString());
        replace = replace.Replace("<SECTION>", section.ToString());
        replace = replace.Replace("<WORLD>", LanguageManager.Instance.GetTextValue("Wname." + world.ToString("00")));


        CompleteName.ObjectText = replace.ToUpper();
    }

    void OnDisable()
    {
        if (!ManagerHudUI.IsInstanceNull())
            ManagerHudUI.Instance.effectsMultiplier = 0;
    }

    void Update()
    {
        if (!ManagerPause.Pause)
            ManagerHudUI.Instance.effectsMultiplier = canvasGroup.alpha;

        ChangeText();
    }
}
