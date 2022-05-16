using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    public GameObject[] tutorialPages;
    private int next;
    private int page = 0;
    private int actualPage;
    public GameObject guideGroup;
    public Text pageCount;

    // Start is called before the first frame update
    void Start()
    {
        page = 0;
        actualPage = page + 1;
        pageCount.text = actualPage.ToString() + " / " + tutorialPages.Length.ToString();
    }

    public void nextPage()
    {
        next = page + 1;
        if(next < tutorialPages.Length) {
        tutorialPages[page].SetActive(false);
        page += 1;
        tutorialPages[page].SetActive(true);
        } else
        {
        tutorialPages[page].SetActive(false);
        page = 0;
        tutorialPages[page].SetActive(true);
        }
        actualPage = page + 1;
        pageCount.text = actualPage.ToString() + " / " + tutorialPages.Length.ToString();

    }

    public void previousPage()
    {
        next = page - 1;
        if(next >= 0) {
        tutorialPages[page].SetActive(false);
        page -= 1;
        tutorialPages[page].SetActive(true);
        } else
        {
        tutorialPages[page].SetActive(false);
        page = tutorialPages.Length - 1;
        tutorialPages[page].SetActive(true);
        }
        actualPage = page + 1;
        pageCount.text = actualPage.ToString() + " / " + tutorialPages.Length.ToString();
    }

    public void resetTutorial()
    {
        tutorialPages[page].SetActive(false);
        page = 0;
        tutorialPages[page].SetActive(true);
        actualPage = page + 1;
        pageCount.text = actualPage.ToString() + " / " + tutorialPages.Length.ToString();
    }
}
