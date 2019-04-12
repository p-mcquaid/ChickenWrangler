using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public Camera cam;
    public NavMeshAgent agent;
    public GameObject[] chickens = new GameObject[5];
    public Text text;
    public Image power_Bar;
    public int sign = 1;

    private Touch touch;
    private float dist_toChicken = 4.0f;
    private float[] dist = new float[5];
    private Vector3 store_Dest;

    private float percentage;
    private float currentFill;


    void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("Camera").GetComponent<Camera>();
        for (int i = 0; i < chickens.Length; i++)
        {
            chickens = GameObject.FindGameObjectsWithTag("Chicken");
        }
        Debug.Log("Chickens.size: " + chickens.Length);
    }

    private void Start()
    {
        for (int i = 0; i < dist.Length; i++)
        {
            dist[i] = 0.0f;
        }
        text = GameObject.Find("capture_Text").GetComponent<Text>();
        text.gameObject.SetActive(false);
        power_Bar = GameObject.Find("power_bar").GetComponent<Image>();
        if (power_Bar != null)
        {
            percentage = power_Bar.fillAmount * 100;
        }
        power_Bar.gameObject.SetActive(false);
       
    }
    void FixedUpdate()
    {

        


        GetChickenDist();

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            Ray ray = cam.ScreenPointToRay(touch.position);
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }

    }

    public void GetChickenDist()
    {
        for (int i = 0; i < dist.Length; i++)
        {
            dist[i] = Vector3.Distance(chickens[i].transform.position, this.transform.position);
        }

        for (int i = 0; i < dist.Length; i++)
        {
            if (dist[i] < dist_toChicken)
            {
                text.gameObject.SetActive(true);

                if (Input.GetKey(KeyCode.Space))
                {

                    for (int j = 0; j < chickens.Length; j++)
                    {
                        chickens[j].gameObject.GetComponent<EnemyBehaivour>().enabled = false;
                    }

                    store_Dest = agent.destination;
                    this.agent.SetDestination(this.transform.position);

                    power_Bar.gameObject.SetActive(true);
                    percentage += sign;
                    Debug.Log(percentage);
                    if (percentage >= 100 || percentage <= 0)
                    {
                        sign *= -1;
                        percentage = ((percentage <= 0) ? 0 : 100);
                    }

                    power_Bar.fillAmount = percentage / 100;


                }

            }
            else
            {
                text.gameObject.SetActive(false);
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            for (int j = 0; j < chickens.Length; j++)
            {
                chickens[j].gameObject.GetComponent<EnemyBehaivour>().enabled = true;
            }
            agent.SetDestination(store_Dest);
            power_Bar.gameObject.SetActive(false);
            percentage = 100;
            power_Bar.fillAmount = 1.0f;
        }
    }

 

    #region Alt Control Scheme
    ///
    //   private GameObject player;
    //   private Vector3 targetVel;
    //   private Rigidbody rb;
    //   private Vector3 m_Vel = Vector3.zero;

    //   [Range (0.0f,0.3f)] public float m_moveSmooth = 0.05f;
    //   private float m_horizontal = 0.0f;
    //   private float m_vertical = 0.0f;

    //// Use this for initialization
    //void Awake () {
    //       player = GameObject.FindGameObjectWithTag("Player");
    //       rb = player.GetComponent<Rigidbody>();
    //       targetVel = new Vector3(0, 0, 0);

    //   }

    //// Update is called once per frame
    //void Update () {
    //       m_horizontal = Input.GetAxisRaw("Horizontal");
    //       m_vertical = Input.GetAxisRaw("Vertical");
    //       Move(m_horizontal, m_vertical);        

    //}

    //   public void Move(float x, float z)
    //   {
    //       targetVel = new Vector3(x * 10f, targetVel.y, z * 10f);
    //       rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVel, ref m_Vel, m_moveSmooth);
    //   }
    #endregion

}
