using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControllerScript : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerMechanicsScript pms;
    

    public Transform npcFolder;
    private Transform npcFolderCopy;
    private float    DoT = 1.0f;
    public Transform HelperUI;

    NPCScript[] npcs;
    int npcCount;

    private RiddleDoorScript door1, door2;

    public enum GameState
    {
        Intro,
        GamePlay,
        Riddles,
        gameEndedWin,
        gameEndedLose
    }

    [HideInInspector] public GameState gameState { get; private set; }

    private void Start()
    {
        door1 = GameObject.Find("SlidingDoor1").GetComponent<RiddleDoorScript>();
        door2 = GameObject.Find("SlidingDoor2").GetComponent<RiddleDoorScript>();

        RestartGame();
    }

    public void DestroyMe(NPCScript npc) {
        npc.StopAllCoroutines();
        Destroy(npc.gameObject);


        if (--npcCount == 0)
            GameEnded(true);
    }

    public void ForceChangeState(GameState newState)
    {
        gameState = newState;
    }

    private void Update()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            if (gameState.Equals(GameState.gameEndedWin))
            {
                SceneManager.LoadScene("Outro");
            }
            else if (gameState.Equals(GameState.gameEndedLose))
            {
                RestartGame();
            }
        }

        DoT -= Time.deltaTime;
        if (DoT < 0)
        {
            pms.UpdateStressLevel(0.1f);
            DoT = 1.0f;
        }
    }

    public void PlayerDead()
    {
        // TODO: Losing text
        Debug.Log("lost");
        GameEnded(false);
        //RestartGame();
    }

    public void GameEnded(bool isWin)
    {
        gameState = isWin ? GameState.gameEndedWin : GameState.gameEndedLose;
        pms.transform.GetComponent<PlayerMovementScript>().gamePaused = true;
        Color bg = isWin ? new Color(140 / 255f, 1, 1, 1) : new Color(1, 0, 0, 1);
        string message = isWin ? "You succesfully deployed product!\nPress SPACE to Proceed" : "You are too mad to continue\nPress SPACE to try again later";

        HelperUI.gameObject.SetActive(true);
        HelperUI.GetComponentInChildren<Text>().text = message;
        foreach (Image im in HelperUI.GetComponentsInChildren<Image>())
        {
            im.color = bg;
        }
    }

    private void RestartGame()
    {

        gameState = GameState.GamePlay;
        if (npcFolderCopy)
            DestroyImmediate(npcFolderCopy.gameObject);
        npcFolderCopy = Instantiate(npcFolder.gameObject).transform;
        npcFolderCopy.gameObject.SetActive(true);
        npcFolder.gameObject.SetActive(false);
        npcFolderCopy.name = "ActiveNPCFolder";
        npcs = npcFolder.GetComponentsInChildren<NPCScript>();
        npcCount = npcs.Length;

        HelperUI.gameObject.SetActive(false);

        if (pms.isInititated)
            pms.Restart();
        pms.GetComponent<PlayerMovementScript>().gamePaused = false;

        door2.Restart(door1.Restart(-1));

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Heal"))
        {
            Destroy(go);
        }
    }
}
