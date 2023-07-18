using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MissionScene : MonoBehaviour
{
    [SerializeField] GameObject chapterParent;
    [SerializeField] Chapter[] chapter;
    private GameObject[] chapterPanel;
    [SerializeField] GameObject chapterPanelPref;
    [SerializeField] GameObject levelButtonPref;

    [SerializeField] TextMeshProUGUI titleChapterText;
    [SerializeField] Button rightButton;
    [SerializeField] Button leftButton;

    [Header("Info Panel")]
    [SerializeField] TextMeshProUGUI titleLevelText;
    [SerializeField] TextMeshProUGUI infoLevelText;
    [SerializeField] Button playLevelButton;

    private int idChapterSelected = 0;


    private void Awake()
    {
        AddListener();
    }
    private void OnDestroy()
    {
        RemoveListener();
    }
    private void Start()
    {
        LoadChapter();
        ChangeChapter(0);
    }

    #region Chapter
    private void LoadChapter()
    {
        chapterPanel = new GameObject[chapter.Length];

        for (int i = 0; i < chapter.Length; i++)
        {
            chapterPanel[i] = Instantiate(chapterPanelPref, chapterParent.transform);
            int x = i + 1;
            chapterPanel[i].gameObject.name = "Chapter " + x;

            // load level
            for (int j = 0; j < chapter[i].level.Length; j++)
            {
                GameObject _obj = Instantiate(levelButtonPref, chapterPanel[i].transform);
                int a = i;
                int b = j;
                _obj.GetComponent<Button>().onClick.AddListener(() => { SwitchLevel(a, b); });
                int p = j + 1;
                _obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = p.ToString();

            }
        }
    }

    private void SwitchChapter(bool toTheRight)
    {
        if (toTheRight)
        {
            idChapterSelected++;
            if (idChapterSelected >= chapter.Length - 1)
            {
                idChapterSelected = chapter.Length - 1;
                rightButton.interactable = false;
            }
            ChangeChapter(idChapterSelected);
            leftButton.interactable = true;
        }
        else
        {
            idChapterSelected--;
            if (idChapterSelected <= 0)
            {
                idChapterSelected = 0;
                leftButton.interactable = false;
            }
            ChangeChapter(idChapterSelected);
            rightButton.interactable = true;
        }


    }
    private void ChangeChapter(int id)
    {
        int i = 0;
        foreach (GameObject obj in chapterPanel)
        {
            chapterPanel[i].SetActive(false);
            i++;
        }
        chapterPanel[id].SetActive(true);
        titleChapterText.text = chapter[id].chapterName.ToString();
    }
    #endregion

    #region Level
    private void SwitchLevel(int idChapter, int idLevel)
    {
        titleLevelText.text = chapter[idChapter].level[idLevel].levelTitle;
        infoLevelText.text = chapter[idChapter].level[idLevel].infoLevel;
    }
    private void SelectLevel(int idChapter, int idLevel)
    {

    }
    #endregion
    private void AddListener()
    {
        rightButton.onClick.AddListener(() => { SwitchChapter(true); });
        leftButton.onClick.AddListener(() => { SwitchChapter(false); });
    }
    private void RemoveListener()
    {
        rightButton.onClick.RemoveAllListeners();
        leftButton.onClick.RemoveAllListeners();
    }

}

[System.Serializable]
public class Chapter
{
    public string chapterName;
    public Level[] level;
}
[System.Serializable]
public class Level
{
    public string sceneName;
    public string levelTitle;
    public string infoLevel;
}
