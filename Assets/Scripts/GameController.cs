using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    private CubePosition nowCube = new CubePosition(0, 1, 0);
    public float cubeChangePlaceSpeed = 0.5f;
    public Transform cubeToPlace;

    private float camMoveToYPos, camMoveSpeed=2f;

    public Text scoreTxt;

    public GameObject cubeToCreate, allCubes, vfx;
    public GameObject[] canvasStartPage;

    private Color toCameraColor;
    public Color[] bgColors;

    private Rigidbody allCubesRB;

    private bool isLose, FirstCube;

    private List<Vector3> allCubesPosition = new List<Vector3>
    {
        new Vector3(0,0,0),
        new Vector3(1,0,0),
        new Vector3(-1,0,0),
        new Vector3(0,1,0),
        new Vector3(0,0,1),
        new Vector3(0,0,-1),
        new Vector3(1,0,1),
        new Vector3(-1,0,1),
        new Vector3(1,0,-1),
        new Vector3(-1,0,-1)
    };

    private int prevCountMaxHor;
    private Transform mainCam;
    private Coroutine showCubePlace;



    private void Start()
    {
        scoreTxt.text = scoreTxt.text = "<size=35><color=#B32E30>Best</color> score:</size>" + PlayerPrefs.GetInt("score") + "\n<size=35>now:</size> 0";

        toCameraColor = Camera.main.backgroundColor;
        mainCam = Camera.main.transform;
        camMoveToYPos = 5.9f + nowCube.y - 1f;

        allCubesRB = allCubes.GetComponent<Rigidbody>();
        showCubePlace = StartCoroutine(ShowCubePlace());
    }
    private void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.touchCount > 0) && cubeToPlace != null && allCubes!=null && !EventSystem.current.IsPointerOverGameObject())
        {
#if !UNITY_EDITOR
            if (Input.GetTouch(0).phase !== TouchPhase.Began)
                return;
#endif
            if (!FirstCube)
            {
                FirstCube = true;
                foreach (GameObject obj in canvasStartPage)
                    Destroy(obj);
            }
         GameObject newCube =  Instantiate(
                cubeToCreate, cubeToPlace.position, Quaternion.identity) as GameObject;
            newCube.transform.SetParent(allCubes.transform);
            nowCube.setVector(cubeToPlace.position);
            allCubesPosition.Add(nowCube.GetVector());

            if (PlayerPrefs.GetString("music") != "No")
                GetComponent<AudioSource>().Play();

           GameObject newVfx = Instantiate(vfx, newCube.transform.position, Quaternion.identity) as GameObject;
            Destroy(newVfx, 1.5f);

            allCubesRB.isKinematic = true;
            allCubesRB.isKinematic = false;

            SpawnPosition();
            MoveCameraChangeBG();
        }
        

        if (!isLose && allCubesRB.velocity.magnitude > 0.1f)
        {
            Destroy(cubeToPlace.gameObject);
            isLose = true;
            StopCoroutine(showCubePlace);
        }
        
        mainCam.localPosition = Vector3.MoveTowards(mainCam.localPosition, 
            new Vector3(mainCam.localPosition.x, camMoveToYPos, mainCam.localPosition.z),
            camMoveSpeed*Time.deltaTime);

        if (Camera.main.backgroundColor != toCameraColor)
            Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, toCameraColor, Time.deltaTime / 1.5f);



        
    }
    IEnumerator ShowCubePlace()
    {
        while (true)
        {
            SpawnPosition();

            yield return new WaitForSeconds(cubeChangePlaceSpeed);
        }
    }
    private void SpawnPosition()
    {
        List<Vector3> position = new List<Vector3>();
        if (isPositionEmpty(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z))
            && nowCube.x + 1 != cubeToPlace.position.x)
            position.Add(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z));
        if (isPositionEmpty(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z))
            && nowCube.x - 1 != cubeToPlace.position.x)
            position.Add(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z));
        if (isPositionEmpty(new Vector3(nowCube.x, nowCube.y + 1, nowCube.z))
            && nowCube.y + 1 != cubeToPlace.position.y)
            position.Add(new Vector3(nowCube.x , nowCube.y + 1, nowCube.z));
        if (isPositionEmpty(new Vector3(nowCube.x, nowCube.y - 1, nowCube.z))
            && nowCube.y - 1 != cubeToPlace.position.y)
            position.Add(new Vector3(nowCube.x, nowCube.y - 1, nowCube.z));
        if (isPositionEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z + 1))
            && nowCube.z + 1 != cubeToPlace.position.z)
            position.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z + 1));
        if (isPositionEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z - 1))
            && nowCube.z - 1 != cubeToPlace.position.z)
            position.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z - 1));

        if (position.Count > 1)
            cubeToPlace.position = position[UnityEngine.Random.Range(0, position.Count)];
        else if (position.Count == 0)
            isLose = true;
        else
            cubeToPlace.position = position[0];

    }
    private bool isPositionEmpty(Vector3 targetpos)
    {
        if (targetpos.y == 0)
            return false;
        foreach (Vector3 pos in allCubesPosition)
        {
            if (pos.x == targetpos.x && pos.y == targetpos.y && pos.z == targetpos.z)
                return false;
        }
        return true;
    }

    private void MoveCameraChangeBG()
    {
        int maxX = 0, maxY = 0, maxZ = 0,  maxHor;

        foreach (Vector3 pos in allCubesPosition)
        {
            if (Convert.ToInt32(pos.x) > maxX)
                maxX = Mathf.Abs(Convert.ToInt32(pos.x));
            if (Convert.ToInt32(pos.y) > maxY)
                maxY = Convert.ToInt32(pos.y);
            if (Convert.ToInt32(pos.z) > maxZ)
                maxZ = Mathf.Abs(Convert.ToInt32(pos.z));
        }

        if (PlayerPrefs.GetInt("score") < maxY)
            PlayerPrefs.SetInt("score", maxY);

        scoreTxt.text = "<size=35><color=#B32E30>Best</color> score:</size>" + PlayerPrefs.GetInt("score") + "\n<size=35>now:</size> " + maxY;

        camMoveToYPos = 5.9f + nowCube.y - 1f;

        maxHor = maxX > maxZ ? maxX : maxZ;
        if (maxHor % 3 == 0 && prevCountMaxHor != maxHor)
        {
            mainCam.localPosition -= new Vector3(0, 0, 2.5f);
            prevCountMaxHor = maxHor;
        }

        if (maxY >= 7)
            toCameraColor = bgColors[2];
        else if (maxY >= 5)
            toCameraColor = bgColors[1];
        else if (maxY >= 2)
            toCameraColor = bgColors[0];
    }

}
struct CubePosition
{
    public int x,y,z;
     public CubePosition (int x,int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3 GetVector()
    {
        return new Vector3(x, y, z);
    }
    public void setVector (Vector3 pos)
    {
        x = Convert.ToInt32(pos.x);
        y = Convert.ToInt32(pos.y);
        z = Convert.ToInt32(pos.z);
    }

}