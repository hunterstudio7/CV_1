using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    private CubePos nowCube = new CubePos(0f, 1.0f, 9.0f);
    public float cubeChangeSpeed = 0.5f;
    private float camMoveYPos, camMoveSpeed = 2f;
    public Transform cubeToPlace;
    public GameObject allCubes,vfx;
    private Rigidbody allCubesRb;
    public bool isLose, firstCube;
    public GameObject[] canvasStartPage;
    private Coroutine showCubePlace;
    private Transform mainCam;
    private int prevCountMaxHor;
    public Color[] bgColors;
    private Color toCameraColor;
    public Text scoreText;
    public GameObject[] cubesToCreate;
    
    private List<Vector3> allCubesPos = new List<Vector3>
    {
        new Vector3(0,0f,9f),
        new Vector3(1,0f,9f),
        new Vector3(-1,0f,9f),
        new Vector3(0,1f,9f),
        new Vector3(0,0f,10f),
        new Vector3(0,0f,-10f),
        new Vector3(1,0f,10f),
        new Vector3(-1,0f,-10f),
        new Vector3(-1,0f,-10f),
        new Vector3(1,0f,-10f),
    };

    private List<GameObject> possibleCubes = new List<GameObject>();
    private void Start()
    {
        if (PlayerPrefs.GetInt("score") < 5)

            possibleCubes.Add(cubesToCreate[0]);
        else if (PlayerPrefs.GetInt("score") < 10)
            AddPosibCubes(2);
        else if (PlayerPrefs.GetInt("score") < 15)
            AddPosibCubes(3);
        else if (PlayerPrefs.GetInt("score") < 20)
            AddPosibCubes(4);
        else if (PlayerPrefs.GetInt("score") < 25)
            AddPosibCubes(5);
        else if (PlayerPrefs.GetInt("score") < 30)
            AddPosibCubes(6);
        else if (PlayerPrefs.GetInt("score") < 35)
            AddPosibCubes(7);
        else if (PlayerPrefs.GetInt("score") < 40)
            AddPosibCubes(8);
        else if (PlayerPrefs.GetInt("score") < 45)
            AddPosibCubes(9);
        else if (PlayerPrefs.GetInt("score") <= 50)
            AddPosibCubes(10);
        else if (PlayerPrefs.GetInt("score") > 50)
            AddPosibCubes(10);
        scoreText.text = "<size=32>BEST:</size><color=\"#E06055\">" + PlayerPrefs.GetInt("score") + "</color>\n<size=22>CUBE:</size> <color=\"#E06055\">0" + "</color>";
        toCameraColor = Camera.main.backgroundColor;
        mainCam = Camera.main.transform;
        camMoveYPos = 7.95f + nowCube.y - 1f;
        allCubesRb = allCubes.GetComponent<Rigidbody>();
        showCubePlace = StartCoroutine(ShowCubePlace());
    }
    private void Update()
    {

            if ((Input.GetMouseButtonDown(0) || Input.touchCount> 0) && (cubeToPlace != null)&&(allCubes !=null)&&(!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)))
        {
#if !UNITY_ANDROID
            if(Input.GetTouch(0).phase != TouchPhase.Began)
            return;
#endif

           
                if (!firstCube)
            {
                firstCube = true;
                foreach (GameObject obj in canvasStartPage)
                    Destroy(obj);
            }
           

            
            GameObject createCube = null;
            if (possibleCubes.Count == 1)
            {
                createCube = possibleCubes[0];
            }
            else
            {
                createCube = possibleCubes[UnityEngine.Random.Range(0, possibleCubes.Count)];
            }
            GameObject newCube = Instantiate(createCube, cubeToPlace.position, Quaternion.identity) as GameObject;
            newCube.transform.SetParent(allCubes.transform);
            nowCube.setVector(cubeToPlace.position);
            allCubesPos.Add(nowCube.getVector());

            GameObject newVfx = Instantiate(vfx, newCube.transform.position, Quaternion.identity);
            Destroy(newVfx,1.5f);
            allCubesRb.isKinematic = true;
            allCubesRb.isKinematic = false;
            SpawnPos();
            MoveCameraBg();
        }
        if(!isLose && allCubesRb.velocity.magnitude > 0.1f)
        {
            Destroy(cubeToPlace.gameObject);
            isLose = true;
            StopCoroutine(showCubePlace);
        }
        mainCam.localPosition = Vector3.MoveTowards(mainCam.localPosition, new Vector3(mainCam.localPosition.x, camMoveYPos, mainCam.localPosition.z),camMoveSpeed*Time.deltaTime);

        if (Camera.main.backgroundColor != toCameraColor)
            Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, toCameraColor, Time.deltaTime / 1.5f);

    }

    IEnumerator ShowCubePlace()
    {
        while (true)
        {
            SpawnPos();
            yield return new WaitForSeconds(cubeChangeSpeed);
        }
    }
    private void SpawnPos()
    {
        List<Vector3> positions = new List<Vector3>();
        if (IsPosEmpty(new Vector3(nowCube.x + 1,nowCube.y,nowCube.z)) && nowCube.x+1 !=cubeToPlace.position.x)
            positions.Add(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z));

           if (IsPosEmpty(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z)) && nowCube.x - 1 != cubeToPlace.position.x)
                positions.Add(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z));

        if (IsPosEmpty(new Vector3(nowCube.x, nowCube.y+1, nowCube.z)) && nowCube.y + 1 != cubeToPlace.position.x)
            positions.Add(new Vector3(nowCube.x, nowCube.y+1, nowCube.z));

        if (IsPosEmpty(new Vector3(nowCube.x, nowCube.y -1, nowCube.z)) && nowCube.y - 1 != cubeToPlace.position.x)
            positions.Add(new Vector3(nowCube.x, nowCube.y -1, nowCube.z));

        if (IsPosEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z+1)) && nowCube.z + 1 != cubeToPlace.position.x)
            positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z+1));

        if (IsPosEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z-1)) && nowCube.z - 1 != cubeToPlace.position.x)
            positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z-1));
        if (positions.Count > 1)
            cubeToPlace.position = positions[UnityEngine.Random.Range(0, positions.Count)];
        else if (positions.Count == 0)
            isLose = true;
        else
            cubeToPlace.position = positions[0];

    }
    private bool IsPosEmpty(Vector3 targetPos)
    {
       
        if (targetPos.y == 0) {
            return false;
        }
        foreach(Vector3 pos in allCubesPos)
        {
            if(pos.x == targetPos.x && pos.y ==targetPos.y && pos.z == targetPos.z)
            {
                return false;
            }
        }
        return true;
    }
    private void MoveCameraBg()
    {
        int maxX = 0,maxY=0, maxZ=0, maxHor;
        foreach(Vector3 pos in allCubesPos)
        {
            if (Mathf.Abs(Convert.ToInt32(pos.x)) > maxX)
            {
                maxX = Convert.ToInt32(pos.x);
            }
            if (Mathf.Abs(Convert.ToInt32(pos.y)) > maxY)
            {
                maxY = Convert.ToInt32(pos.y);
            }
            if (Mathf.Abs(Convert.ToInt32(pos.z)) > maxZ)
            {
                maxZ = Convert.ToInt32(pos.z);
            }
        }
        maxY--;
        if (PlayerPrefs.GetInt("score") < maxY-1)
        {
            PlayerPrefs.SetInt("score", maxY-1);
        }
        scoreText.text = "<size=32>BEST:</size><color=\"#E06055\">"+PlayerPrefs.GetInt("score")+"</color>\n<size=22>CUBE:</size> <color=\"#E06055\">"+maxY+"</color>";
        camMoveYPos = 7.95f + nowCube.y - 1f;
        maxHor = maxX > maxZ ? maxX : maxZ;
        if(maxHor % 3 == 0 && prevCountMaxHor != maxHor)
        {
            mainCam.localPosition -= new Vector3(0,0,2.5f);
            prevCountMaxHor = maxHor;
        }
        if (maxY >= 7)
            toCameraColor = bgColors[2];
        else if (maxY >= 5)
            toCameraColor = bgColors[1];
        else if (maxY >= 2)
            toCameraColor = bgColors[0];
    }
    private void AddPosibCubes(int till)
    {
        for (int i = 0; i < till; i++)
        {
            possibleCubes.Add(cubesToCreate[i]);
        }
    }
}
struct CubePos
{
    public float x, y, z;

    public CubePos(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;

    }
    public Vector3 getVector()
    {
        return new Vector3(x, y, z);
    }
    public void setVector(Vector3 pos)
    {
        x = Convert.ToInt32(pos.x);
        y = Convert.ToInt32(pos.y);
        z = Convert.ToInt32(pos.z);
    }

}