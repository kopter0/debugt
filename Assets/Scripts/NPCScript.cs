using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCScript : MonoBehaviour
{
    [System.Serializable]
    public struct DamagablePart
    {
        public int max_hit;
        public GameObject go;
        public DamagablePart(int mx, GameObject pgo)
        {
            max_hit = mx;
            go = pgo;
        }
    }

    [SerializeField]
    public List<DamagablePart> damagableParts;
    [HideInInspector]
    public Dictionary<string, int> nameToIndex;
    private List<int> curHits;

    private GameControllerScript gameController;

    public Slider timerSlider;

    private float counter = 0.0f;
    private float range = 15.0f;
    private Transform player;
    private NPCState state = NPCState.Patrolling;

    private bool leftWingExists, rightWingExists;
    private Transform leftWing, rightWing;


    enum NPCState
    {
        Patrolling,
        Chasing,
        Dead
    };

    private delegate void OnUpdateAction();
    Dictionary<NPCState, OnUpdateAction> actionDictionary;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        gameController = GameObject.FindGameObjectsWithTag("GameController")[0].GetComponent<GameControllerScript>();

        nameToIndex = new Dictionary<string, int>();
        curHits = new List<int>();
        int idx = 0;
        foreach (DamagablePart dp in damagableParts)
        {
            nameToIndex.Add(dp.go.name, idx);
            curHits.Add(0);
            idx++;
        }

        actionDictionary = new Dictionary<NPCState, OnUpdateAction>()
        {
            {NPCState.Patrolling, Patrol },
            {NPCState.Chasing, Chase }
        };

        timerSlider.value = 0f;

        leftWingExists = rightWingExists = true;
        leftWing = transform.Find("PA_ArchfireLeftRing");
        rightWing = transform.Find("PA_ArchfireRightRing");
    }

    // Update is called once per frame
    void Update()
    {

        UpdateState();
        actionDictionary[state]();
        timerSlider.value += Time.deltaTime;
        if (timerSlider.value >= 1f)
        {
            player.GetComponent<PlayerMechanicsScript>().UpdateStressLevel(0.1f);
            timerSlider.value = 0f;
        }
        
    }

    public void HitTaken(GameObject go)
    {
        DamagablePart dp = damagableParts[nameToIndex[go.name]];
        curHits[nameToIndex[go.name]]++;
        if (curHits[nameToIndex[go.name]] > dp.max_hit)
        {
            if (dp.go.name.Contains("Ring"))
            {
                if (dp.go.name.Contains("Right"))
                    rightWingExists = false;
                else
                    leftWingExists = false;
                Destroy(dp.go);
                bool a = rightWing && leftWing;
                Debug.Log(a);
            }
            else
                StartCoroutine("Death");
        }
    }

    IEnumerator Death()
    {
        Vector3 fallTo = transform.position; fallTo.y = 0;
        while (transform.position.y > 0.5f)
        {
            transform.position = Vector3.Lerp(transform.position, fallTo, Time.deltaTime);
            yield return null;
        }

        gameController.DestroyMe(this);
        yield return null;
    }

    public void PopulateParts()
    {
        damagableParts = new List<DamagablePart>();

        MeshCollider[] mcslist = transform.GetComponentsInChildren<MeshCollider>();
        foreach (MeshCollider mc in mcslist) {
            damagableParts.Add(new DamagablePart(0, mc.gameObject));
        }
    }



    #region OnUpdateActions
    void UpdateState()
    {
        bool condition = Vector3.Distance(transform.position, player.position) < range;
        NPCState prev = state;
        state = condition ? NPCState.Chasing : NPCState.Patrolling;

        if (state != prev)
        {
            switch (state)
            {
                case NPCState.Patrolling:
                    if (leftWingExists)
                        leftWing.GetComponent<ConstantForce>().relativeForce = 3 * 9.81f * Vector3.forward;
                    if (rightWingExists)
                        rightWing.GetComponent<ConstantForce>().relativeForce = 3 * 9.81f * Vector3.forward;
                    break;
                case NPCState.Chasing:
                    if (leftWingExists)
                    {
                        //leftWing.GetComponent<ConstantForce>().relativeForce = 6 * 9.81f * Vector3.up;
                        leftWing.GetComponent<ConstantForce>().force = Vector3.zero;
                        leftWing.GetComponent<ConstantForce>().relativeForce = Vector3.zero;    
                    }
                    if (rightWingExists)
                    {
                        //rightWing.GetComponent<ConstantForce>().relativeForce = 6 * 9.81f * Vector3.up;
                        rightWing.GetComponent<ConstantForce>().force = Vector3.zero;
                        rightWing.GetComponent<ConstantForce>().relativeForce = Vector3.zero;
                    }
                    break;
            }
        }
    }
    void Patrol()
    {
        //transform.position = new Vector3(3 * Mathf.Cos(counter), transform.position.y, 3 * Mathf.Sin(counter));

        if (leftWingExists || rightWingExists)
        {
            Quaternion a = Quaternion.LookRotation(new Vector3(-Mathf.Sin(counter), 0, Mathf.Cos(counter)), Vector3.up);
            transform.rotation = a;
            counter += Time.deltaTime / 2;
            Mathf.Clamp(counter, 0, 2 * Mathf.PI);
        }


    }

    void Chase()
    {
        Vector3 toPlayer = player.position - transform.position;
        toPlayer.Normalize();
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(toPlayer, Vector3.up), 4 * Time.deltaTime);
        //transform.position =  Vector3.Lerp(transform.position, player.position - 5 * toPlayer, Time.deltaTime);

        float d = Vector3.Distance(transform.position, player.position) - range / 2;

        Debug.Log(d);

        if (leftWingExists)
            leftWing.GetComponent<ConstantForce>().relativeForce = -3 * d * 9.81f * Vector3.forward * Time.deltaTime + 6 * 9.81f * Vector3.up;
        if (rightWingExists)
            rightWing.GetComponent<ConstantForce>().relativeForce = -3 * d * 9.81f * Vector3.forward * Time.deltaTime + 6 * 9.81f * Vector3.up;
    }

    
    #endregion
}



