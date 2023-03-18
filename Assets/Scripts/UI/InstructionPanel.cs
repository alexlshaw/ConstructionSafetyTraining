using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InstructionPanel : MonoBehaviour
{
    public GameObject[] pages;
    [SerializeField] int currentPage = 0;
    public GameObject nextButton;
    public GameObject lastButton;
    public bool facePlayer;
   
    // Start is called before the first frame update
    void Start()
    {
        lastButton.SetActive(false);
    }
    public void nextPage()
    {
        checkPages();
        for (int i = 0; i < pages.Length; i++)
        {
            if(i == currentPage)
            {
                pages[i].SetActive(true);
            }
            else
            {
                pages[i].SetActive(false);
            }
        }
        if(currentPage < pages.Length-1)
            currentPage = currentPage + 1;

    }
    public void checkPages()
    {
        if (currentPage == pages.Length-1)
        {
            nextButton.SetActive(false);
        }
        else if (currentPage == 0)
        {
            lastButton.SetActive(false);
        }
        else
        {
            nextButton.SetActive(true);
            lastButton.SetActive(true);
        }
    }
    public void lastPage()
    {
        checkPages();
        for (int i = 0; i < pages.Length; i++)
        {
            if (i == currentPage)
            {
                pages[i].SetActive(true);
            }
            else
            {
                pages[i].SetActive(false);
            }
        }
        if(currentPage > 0)
            currentPage = currentPage - 1;

    }

    public void close()
    {
        gameObject.SetActive(false);
    }
    private void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
    private void OnEnable()
    {
        if (facePlayer)
        {
            Vector3 position = Camera.main.transform.position + Camera.main.transform.forward * 1.5f;
            transform.position = new Vector3(position.x, 1.5f, position.z);
        }
       
    }

}
