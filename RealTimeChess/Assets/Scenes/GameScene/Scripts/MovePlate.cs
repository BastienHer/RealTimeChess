using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    public GameObject[] controller;

    GameObject reference = null;

    int matrixX;
    int matrixY;

    public bool attack = false;

    public void Start()
    {
        if (attack)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0, 0, 1f);
        }
    }
    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectsWithTag("GameController");

        if (attack)
        {
            GameObject cp = controller[0].GetComponent<GameManager>().getPosition(matrixX,matrixY);
            if(cp.name== "WhiteKing")
            {
                NetworkManager.instance.pioconnection.Send("BlackWins");
            }
            if (cp.name == "BlackKing")
            {
                NetworkManager.instance.pioconnection.Send("WhiteWins");
            }
            Destroy(cp);
        }
        controller[0].GetComponent<GameManager>().SetPositionEmpty(reference.GetComponent<ChessMan>().GetXBoard(), reference.GetComponent<ChessMan>().GetYBoard());

        reference.GetComponent<ChessMan>().SetXboard(matrixX);
        reference.GetComponent<ChessMan>().SetYBoard(matrixY);
        reference.GetComponent<ChessMan>().SetCoords();

        controller[0].GetComponent<GameManager>().SetPosition2(reference, reference.GetComponent<ChessMan>().previousX, reference.GetComponent<ChessMan>().previousY, reference.GetComponent<ChessMan>().name);

        //controller[0].GetComponent<GameManager>().NextTurn();
        reference.GetComponent<ChessMan>().DestroyMovePlates();
    }
    public void SetCoords(int x,int y)
    {
        matrixX = x;
        matrixY = y;
    }
    public void SetReference(GameObject obj)
    {
        reference = obj;
    }
    public GameObject GetReference()
    {
        return reference;
    }
}
