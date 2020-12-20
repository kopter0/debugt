using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RiddleDoorScript : MonoBehaviour
{
    public Material closedMaterial, openMaterial;
    public GameObject PromptText, QuestionText, sign;

    public TextAsset riddles_txt;
    private (string, string)[] riddlesList;
    private string question, answer;
    private InputField inField;
    private GameControllerScript gcs;
    private void Awake()
    {
        PopulateRiddles();
        inField = GetComponentInChildren<InputField>();
        gcs = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>(); 
    }
    
    public int Restart(int forbidden)
    {

        GetComponent<BoxCollider>().isTrigger = false;
        GetComponent<MeshRenderer>().material = closedMaterial;
        inField.DeactivateInputField();
        inField.placeholder.GetComponent<Text>().text = "Enter One String Or Int...";

        PromptText.SetActive(false);
        inField.GetComponent<Image>().color = Color.red;
        sign.SetActive(true);

        int idx = forbidden;
        Random.InitState((int)System.DateTime.Now.Ticks);
        while (idx == forbidden)
            idx = Random.Range(0, riddlesList.Length);
        question = riddlesList[idx].Item1;
        answer = riddlesList[idx].Item2;
        QuestionText.GetComponent<Text>().text = question;  
        return idx;
    }

    public void CheckValidity()
    {
        if (inField.text.ToLower().Equals(answer.ToLower()))
        {
            inField.DeactivateInputField();
            // TODO: Success message
            GetComponent<MeshRenderer>().material = openMaterial;
            GetComponent<BoxCollider>().isTrigger = true;
            inField.GetComponent<Image>().color = Color.green;
            sign.SetActive(false);
        }
        else
        {
            inField.text = "";
            inField.placeholder.GetComponent<Text>().text = "Wrong";
        }

        gcs.ForceChangeState(GameControllerScript.GameState.GamePlay);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Player"))
        {
            PromptText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Equals("Player"))
        {
            PromptText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (PromptText.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            inField.ActivateInputField();
            gcs.ForceChangeState(GameControllerScript.GameState.Riddles);
            
        }
        
    }

    private void PopulateRiddles()
    {
        string txtfile = riddles_txt.text;
        string[] QnAs = txtfile.Split(new string[] { "STOP" }, System.StringSplitOptions.RemoveEmptyEntries);
        riddlesList = new (string, string)[QnAs.Length];
        for (int i = 0; i < QnAs.Length; i++)
        {
            string[] qa = QnAs[i].Split(new string[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);
            riddlesList[i] = (qa[0], qa[1]);
        }
    }

}
