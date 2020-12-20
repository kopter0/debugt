using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCScript : MonoBehaviour
{

    [System.Serializable] public struct DamagablePart
    {
        public int max_hit;
        public GameObject go;
        public bool isDead;
        public DamagablePart(int mx, GameObject pgo)
        {
            max_hit = mx;
            go = pgo;
            isDead = false;
        }

    }

    [SerializeField] public List<DamagablePart> damagableParts;
    [HideInInspector] public Dictionary<string, int> nameToIndex;

    protected List<int> curHits;

    protected GameControllerScript gameController;

    public Slider timerSlider;

    public GameObject shortRest;

    protected float counter = 0.0f;
    protected float range = 15.0f;
    protected Transform player;
    protected NPCState state = NPCState.Patrolling;




    protected enum NPCState
    {
        Patrolling,
        Engage,
        Dead
    };

    protected delegate void OnUpdateAction();
    Dictionary<NPCState, OnUpdateAction> actionDictionary;


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    protected void Init()
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
            {NPCState.Engage, Engage }
        };

        if (timerSlider)
            timerSlider.value = 0f;
    }

    // Update is called once per frame
    void Update()
    {

        UpdateState();
        actionDictionary[state]();
        if (timerSlider)
        {
            timerSlider.value += Time.deltaTime;
            if (timerSlider.value >= 1f)
            {
                player.GetComponent<PlayerMechanicsScript>().UpdateStressLevel(0.1f);
                timerSlider.value = 0f;
            }
        }
        
    }

    virtual public void HitTaken(GameObject go)
    {

    }


    virtual protected IEnumerator Death()
    {
        yield return null;
        
    }

    virtual public void PopulateParts()
    {
        damagableParts = new List<DamagablePart>();

        MeshCollider[] mcslist = transform.GetComponentsInChildren<MeshCollider>();
        foreach (MeshCollider mc in mcslist) {
            damagableParts.Add(new DamagablePart(0, mc.gameObject));
        }
    }



    #region OnUpdateActions
    virtual protected void UpdateState()
    {
        
    }
    virtual protected void Patrol()
    {

    }

    virtual protected void Engage()
    {
        
    }

    protected bool IsPlayerInSight()
    {
        Ray r = new Ray(transform.position, player.position - transform.position);
        RaycastHit rh;
        if (Physics.Raycast(r, out rh, 40.0f))
        {
            if (rh.transform.name.Equals("Player"))
            {
                return true;
            }

        }
        return false;
    }
    
    #endregion
}



