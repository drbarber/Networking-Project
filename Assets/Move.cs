using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Move : NetworkBehaviour
{
    // Movement keys (customizable in Inspector)
    public KeyCode upKey;
    public KeyCode downKey;
    public KeyCode rightKey;
    public KeyCode leftKey;

    //Movement Speed
    public float speed = 16;

    //Wall prefab
    public GameObject wallPrefab;

    //Current wall
    Collider2D wall;

    //Last wall's end
    Vector2 lastWallEnd;

    string wallValue;

    // Use this for initialization
    void Start()
    {

        // Initial Velocity
        wallValue = wallPrefab.tag.ToString();
        GetComponent<Rigidbody2D>().velocity = Vector2.up * speed;
        spawnWall();
    }

    // Update is called once per frame
    void Update()
    {

        //blocks players from moving both bikes at the same time
        if (!isLocalPlayer)
        {
            spawnWall();
            return;
        }
        // Check for key presses, changes direction vector and starts a new wall
        if (Input.GetKeyDown(upKey))
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.up * speed;
            spawnWall();
        }
        else if (Input.GetKeyDown(downKey))
        {
            GetComponent<Rigidbody2D>().velocity = -Vector2.up * speed;
            spawnWall();
        }
        else if (Input.GetKeyDown(rightKey))
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.right * speed;
            spawnWall();
        }
        else if (Input.GetKeyDown(leftKey))
        {
            GetComponent<Rigidbody2D>().velocity = -Vector2.right * speed;
            spawnWall();
        }

        fitColliderBetween(wall, lastWallEnd, transform.position);

    }

    public override void OnStartLocalPlayer()
    {
        wallPrefab.GetComponent<SpriteRenderer>().color = Color.magenta;
        if (isLocalPlayer) { wallPrefab.GetComponent<SpriteRenderer>().color = Color.yellow; }
    }

    //create a new wall and scale it from its start position and where the player is, until a new direction is chosen.
    void spawnWall()
    {
        // Last wall's position
        lastWallEnd = transform.position;
        // Spawn a new Lightwall
        GameObject g = (GameObject)Instantiate(wallPrefab, transform.position, Quaternion.identity);
        wall = g.GetComponent<Collider2D>();
    }

    // this function smooths out the wall when direction is changed, without it the wall starts to look janky
    void fitColliderBetween(Collider2D co, Vector2 a, Vector2 b)
    {
        // Calculate the Center Position
        co.transform.position = a + (b - a) * 0.5f;

        // Scale it (horizontally or vertically)
        float dist = Vector2.Distance(a, b);
        if (a.x != b.x)
            co.transform.localScale = new Vector2(dist + 1, 1);
        else
            co.transform.localScale = new Vector2(1, dist + 1);
    }
    void OnTriggerEnter2D(Collider2D co)
    {
        // Not the current wall?
        if (co != wall )

        {
            //was it you that crashed? or the other player?
            if (isLocalPlayer)
            {
                print("dead");
                Destroy(gameObject); // Destroy your pixel
                SceneManager.LoadScene("Lost"); // display your shame
            }
                     
        }
    }
}