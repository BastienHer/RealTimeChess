using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerIOClient;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class NetworkManager : MonoBehaviour
{
	public Connection pioconnection;
	private List<Message> msgList = new List<Message>(); 
	private bool joinedroom = false;
	public bool isOpponentReady = false;
	public static NetworkManager instance;
    public string player = "";
    private Vector2 scrollPosition;
	private ArrayList entries = new ArrayList();
	private Rect window = new Rect(10, 10, 300, 150);
	private string infomsg = "";
	private string userid;
	private Client clientRoom;

    private void Awake()
    {
		DontDestroyOnLoad(this);
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
		}
		instance = this;
	}
    void Start()
	{
		Application.runInBackground = true; 
		System.Random random = new System.Random();
		userid = "Guest" + random.Next(0, 10000);

		Debug.Log("Starting");

		PlayerIO.Authenticate(
			"realtimechess-ygdgj8suz02hsccy1mkxg",            
			"public",                               
			new Dictionary<string, string> {        
				{ "userId", userid },
			},
			null,                                   
			delegate (Client client) {
				Debug.Log("Successfully connected to Player.IO");
				infomsg = "Successfully connected to Player.IO";
				clientRoom = client;
				

				Debug.Log("Create ServerEndpoint");
				// Comment out the line below to use the live servers instead of your development server
				client.Multiplayer.DevelopmentServer = new ServerEndpoint("localhost", 8184);

				Debug.Log("CreateJoinRoom");
				//Create or join the room 
				client.Multiplayer.CreateJoinRoom(
					"UnityLobbyRoom",                    //Room id. If set to null a random roomid is used
					"UnityRealTimeChess",                   //The room type started on the server
					true,                               //Should the room be visible in the lobby?
					null,
					null,
					delegate (Connection connection) {
						Debug.Log("Joined Room.");
						infomsg = "Joined Room.";
						// We successfully joined a room so set up the message handler
						pioconnection = connection;
						pioconnection.OnMessage += handlemessage;
						joinedroom = true;
					},
					delegate (PlayerIOError error) {
						Debug.Log("Error Joining Room: " + error.ToString());
						infomsg = error.ToString();
					}
				);
			},
			delegate (PlayerIOError error) {
				Debug.Log("Error connecting: " + error.ToString());
				infomsg = error.ToString();
			}
		);

	}

	void handlemessage(object sender, Message m)
	{
		msgList.Add(m);
	}
	public void ready()
    {
		pioconnection.Send("Ready");
    }
	public void startGame()
    {
		pioconnection.Send("StartGame");
    }


	void FixedUpdate()
	{
		// process message queue
		foreach (Message m in msgList)
		{
			switch (m.Type)
			{
				case "PlayerJoined":
					//GameObject newplayer = GameObject.Instantiate(target) as GameObject;
					//newplayer.transform.position = new Vector3(m.GetFloat(1), 0, m.GetFloat(2));
					//newplayer.name = m.GetString(0);
					//newplayer.transform.Find("NameTag").GetComponent<TextMesh>().text = m.GetString(0);
					break;
				case "Move":
					Debug.Log("Move on X ="+ m.GetInt(0)+"Y="+m.GetInt(1)+" From X="+ m.GetInt(2)+"Y="+m.GetInt(3));
					GameManager.instance.DestroyPosition(m.GetInt(0), m.GetInt(1));
                    GameManager.instance.DestroyPosition(m.GetInt(2), m.GetInt(3));
                    GameObject obj = GameManager.instance.Create(m.GetString(4), m.GetInt(0), m.GetInt(1));
					GameManager.instance.SetPosition(obj);


                    break;

				case "PlayerLeft":
					// remove characters from the scene when they leave
					GameObject playerd = GameObject.Find(m.GetString(0));
					Destroy(playerd);
					break;
				case "ChangeRoom":
					clientRoom.Multiplayer.CreateJoinRoom(
						m.GetInt(0).ToString(),                    //Room id. If set to null a random roomid is used
						"UnityRealTimeChessGameplay",                   //The room type started on the server
						true,                               //Should the room be visible in the lobby?
						null,
						null,
						delegate (Connection connection) {
							Debug.Log("Joined GameplayRoom." + m.GetInt(0).ToString());
							infomsg = "Joined Room.";
							// We successfully joined a room so set up the message handler
							pioconnection = connection;
							pioconnection.OnMessage += handlemessage;
							joinedroom = true;
						},
						delegate (PlayerIOError error) {
							Debug.Log("Error Joining Room: " + error.ToString());
							infomsg = error.ToString();
						}
					);
					break;
				case "OpponentReady":
					isOpponentReady = true;
					Debug.Log(isOpponentReady);
					break;
				case "StartGame":
					SceneManager.LoadScene("GameScene");
                    break;
				case "white":
					player = "white";
					break;
				case "black":
					player = "black";
					break;
				case "WhiteWins":
					if (player == "white")
					{
                        SceneManager.LoadScene("WinScene");
                    }
					else
					{
                        SceneManager.LoadScene("LoseScene");
                    }
					break;
				case "BlackWins":
                    if (player == "Black")
                    {
                        SceneManager.LoadScene("WinScene");
                    }
                    else
                    {
                        SceneManager.LoadScene("LoseScene");
                    }
                    break;
			}
		}

		// clear message queue after it's been processed
		msgList.Clear();
	}
}
