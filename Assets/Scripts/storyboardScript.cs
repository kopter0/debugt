using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class storyboardScript : MonoBehaviour
{
    public Sprite[] pages;
    public Text textUI;
    public Image imageUI;
    private string[] texts;
    private int curPage = 0;

    void Start()
    {
        //imageUI = transform.GetComponentInChildren<Image>();
        //textUI = transform.GetComponentInChildren<Text>();
        texts = new string[4] {
            "You are finishing ongoing Project",
            "Suddenly you get error messages",
            "Deadline in two hours, you dont have time to fix everything with traditional methods",
            "But You have superpower"
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int newpage = Mathf.Clamp(curPage + 1, 0, pages.Length - 1);
            if (curPage != newpage)
            {
                curPage = newpage;
                imageUI.sprite = pages[curPage];
                textUI.text = texts[curPage];
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && (curPage == pages.Length - 1))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
