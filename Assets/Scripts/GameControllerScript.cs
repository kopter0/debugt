using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerMechanicsScript pms;

    public Transform npcFolder;
    private Transform npcFolderCopy;

    public Transform HelperUI;

    NPCScript[] npcs;
    int npcCount;

    public bool gameEnded { get; private set; }


    void Start()
    {
        RestartGame();
    }

    public void DestroyMe(NPCScript npc) {
        npc.StopAllCoroutines();
        Destroy(npc.gameObject);


        if (--npcCount == 0)
            GameEnded(true);
    }

    private void Update()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            if (gameEnded)
            {
                RestartGame();
            }
        }
    }

    public void PlayerDead()
    {
        // TODO: Losing text
        Debug.Log("lost");
        GameEnded(false);
        //RestartGame();
    }

    private void GameEnded(bool isWin)
    {
        gameEnded = true;
        pms.transform.GetComponent<PlayerMovementScript>().gamePaused = true;
        Color bg = isWin ? new Color(140 / 255f, 1, 1, 1) : new Color(1, 0, 0, 1);
        string message = isWin ? "You succesfully deployed product!\nPress SPACE to Start new Project" : "You are too mad to continue\nPress SPACE to try again later";

        HelperUI.gameObject.SetActive(true);
        HelperUI.GetComponentInChildren<Text>().text = message;
        foreach (Image im in HelperUI.GetComponentsInChildren<Image>())
        {
            im.color = bg;
        }
    }

    private void RestartGame()
    {

        gameEnded = false;
        if (npcFolderCopy)
            DestroyImmediate(npcFolderCopy.gameObject);
        npcFolderCopy = Instantiate(npcFolder.gameObject).transform;
        npcFolderCopy.gameObject.SetActive(true);
        npcFolder.gameObject.SetActive(false);
        npcs = npcFolder.GetComponentsInChildren<NPCScript>();
        npcCount = npcs.Length;

        HelperUI.gameObject.SetActive(false);

        if (pms.isInititated)
            pms.Restart();
        pms.GetComponent<PlayerMovementScript>().gamePaused = false;
    }
}
