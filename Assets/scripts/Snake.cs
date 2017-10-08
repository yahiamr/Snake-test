using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Snake : MonoBehaviour {
    public List<Transform> bodyParts;
    [System.NonSerialized]
    public Transform headPart;
    public float stepTime = 0.1f;
    private float nextStep;
    public enum Direction { Right, left, up, down};
    public Direction dir;
    public bool Dead;
    public GameObject bodyprt;
    //--------------touch variables--------------------
    private Vector2 frstpnt;
    private Vector2 lstpnt;
    private float swipedist;
    //--------------mouse variables-----------------------
    private Vector2 frstpress;
    private Vector2 lstpress;
    private Vector2 pressdist;
    //--------------------------------------
    public AudioClip MusicClipfd;
    public AudioSource MusicSourcefd;
    public AudioClip MusicClipded;
    public AudioSource MusicSourceded;
    public AudioClip MusicClipbg;
    public AudioSource MusicSourcebg;
    //-----------------------------------------
    public bool paused;
    public Transform deadcanvas;
    //----------------------------------------
    public int sscore=0;

    void Start () {
        paused = false;
        sscore = 0;
        deadcanvas.gameObject.SetActive(false);
        headPart = bodyParts[0];
        swipedist = Screen.height * 15 / 100;
        MusicSourcefd.clip = MusicClipfd;
        MusicSourceded.clip = MusicClipded;
        MusicSourcebg.clip = MusicClipbg;
    }	
	void Update () {
        score.scorevalue = sscore;
        if (!Dead)
        {
            dirCtrlBtns();
            mouseInput();
            if (Time.time > nextStep)
            {
                Move();
               
            }
        }
	}
    public void reload() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        sscore = 0;
    }
    #region moving function
    void Move()
    {
        RaycastHit hit;
        if (Physics.Raycast(headPart.position, headPart.forward, out hit, 1)) {
            if (hit.transform.tag == "body"|| hit.transform.tag=="Bound") {
                Dead = true;
                MusicSourceded.Play();
                MusicSourcebg.Stop();
                deadcanvas.gameObject.SetActive(true);
                return;
            }
            if(hit.transform.tag == "Food")
            {
                EatFood(hit.transform.gameObject);

            }
        }        
        for (int i = bodyParts.Count - 1; i >-1; i--)
        { 
            Transform bdPart = bodyParts[i];
            if (i != 0) {
                bdPart.position = bodyParts[i - 1].position;

            }

        }
        headPart.position += headPart.forward * 1;
        nextStep = Time.time + stepTime;
    }
#endregion
    #region control buttons
    void dirCtrlBtns() {
        if (Input.GetButtonDown("Up") && dir != Direction.up && dir != Direction.down)
        {
            headPart.eulerAngles = new Vector3(0, 0, 0);
            dir = Direction.up;
            Move();
            
        }
        if (Input.GetButtonDown("Down") && dir != Direction.up && dir != Direction.down)
        {
            headPart.eulerAngles = new Vector3(0, 180, 0);
            dir = Direction.down;
            Move();
        }
        if (Input.GetButtonDown("Right") && dir != Direction.Right && dir != Direction.left)
        {
            headPart.eulerAngles = new Vector3(0, 90, 0);
            dir = Direction.Right;
            Move();
        }
        if (Input.GetButtonDown("Left") && dir != Direction.Right && dir != Direction.left)
        {
            headPart.eulerAngles = new Vector3(0, 270, 0);
            dir = Direction.left;
            Move();
        }
    }
#endregion
    #region eat food
    void EatFood(GameObject food) {
        Transform last = bodyParts[bodyParts.Count - 1];
        GameObject newbdprt = Instantiate(bodyprt, last.position - last.forward, bodyprt.transform.rotation) as GameObject;
        newbdprt.transform.parent = transform;
        bodyParts.Add(newbdprt.transform);
        FoodSpawn.Reference.Spawn();
        sscore += 10;
        Destroy(food);
        MusicSourcefd.Play();
    }
#endregion
    #region mobile touch
    void TouchIP() {
        if (Input.touchCount == 1) {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                frstpnt = touch.position;
                lstpnt = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                lstpnt = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended||touch.phase==TouchPhase.Canceled)
            {
                lstpnt = touch.position;
                if (Mathf.Abs(lstpnt.x - frstpnt.x) > swipedist || Mathf.Abs(lstpnt.y - frstpnt.y) > swipedist)
                {
                    if (Mathf.Abs(lstpnt.x - frstpnt.x) > Mathf.Abs(lstpnt.y - frstpnt.y))
                    {
                        if (lstpnt.x > frstpnt.x && dir != Direction.Right && dir != Direction.left)
                        {
                            headPart.eulerAngles = new Vector3(0, 90, 0);
                            dir = Direction.Right;
                            Move();

                        }
                        else if(lstpnt.x < frstpnt.x && dir != Direction.Right && dir != Direction.left)
                        {
                            headPart.eulerAngles = new Vector3(0, 270, 0);
                            dir = Direction.left;
                            Move();
                        }
                    }
                    else
                    {
                        if (lstpnt.y > frstpnt.y && dir != Direction.up && dir != Direction.down)
                        {
                            headPart.eulerAngles = new Vector3(0, 0, 0);
                            dir = Direction.up;
                            Move();
                        }
                        else if (lstpnt.y < frstpnt.y && dir != Direction.up && dir != Direction.down)
                        {
                            headPart.eulerAngles = new Vector3(0, 180, 0);
                            dir = Direction.down;
                            Move();
                        }
                    }
                }
            }
        }
    }
    #endregion
    #region mouse swipe
    void mouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            frstpress = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            lstpress  = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            lstpress = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            pressdist = new Vector2(lstpress.x - frstpress.x, lstpress.y - frstpress.y);
            pressdist.Normalize();
            //swipe upwards
            if (pressdist.y > 0 && pressdist.x > -0.5f && pressdist.x < 0.5f && dir != Direction.up && dir != Direction.down)
        {
                Debug.Log("up swipe");
                headPart.eulerAngles = new Vector3(0, 0, 0);
                dir = Direction.up;
                Move();

            }
            //swipe down
            if (pressdist.y < 0 && pressdist.x > -0.5f && pressdist.x < 0.5f && dir != Direction.up && dir != Direction.down)
        {
                Debug.Log("down swipe");
                headPart.eulerAngles = new Vector3(0, 180, 0);
                dir = Direction.down;
                Move();
            }
            //swipe left
            if (pressdist.x < 0 && pressdist.y > -0.5f && pressdist.y < 0.5f && dir != Direction.Right && dir != Direction.left)
        {
                Debug.Log("left swipe");
                headPart.eulerAngles = new Vector3(0, 270, 0);
                dir = Direction.left;
                Move();
            }
            //swipe right
            if (pressdist.x > 0 && pressdist.y > -0.5f && pressdist.y < 0.5f && dir != Direction.Right && dir != Direction.left)
        {
                Debug.Log("right swipe");
                headPart.eulerAngles = new Vector3(0, 90, 0);
                dir = Direction.Right;
                Move();
            }
        }

    }
    #endregion
    #region pause
    public void pause() {
        paused = !paused;
        if (paused)
        {
            Time.timeScale = 0;
            MusicSourcebg.Pause();
        }
        else
        {
            Time.timeScale = 1;
            if(!Dead)
            MusicSourcebg.Play();
        }

    }

    #endregion
    #region Quit
    public void Quit() {
        Application.Quit();
        print("exit");
    }
#endregion
}
