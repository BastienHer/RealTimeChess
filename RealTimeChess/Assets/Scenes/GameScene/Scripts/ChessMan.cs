using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessMan : MonoBehaviour
{
    public GameObject[] controller;
    public GameObject movePlate;

    public Sprite BlackQueen, BlackKing, BlackBishop, BlackKnight, BlackRook, BlackPawn;
    public Sprite WhiteQueen, WhiteKing, WhiteBishop, WhiteKnight, WhiteRook, WhitePawn;

    private int xboard = -1;
    private int yboard = -1;

    private string player;
    
    public void Activate()
    {
        controller = GameObject.FindGameObjectsWithTag("GameController");

        SetCoords();

        switch (this.name)
        {
            case"BlackQueen":this.GetComponent<SpriteRenderer>().sprite = BlackQueen;player = "black"; break;
            case "BlackKing": this.GetComponent<SpriteRenderer>().sprite = BlackKing; player = "black"; break;
            case "BlackBishop": this.GetComponent<SpriteRenderer>().sprite = BlackBishop; player = "black"; break;
            case "BlackKnight": this.GetComponent<SpriteRenderer>().sprite = BlackKnight; player = "black"; break;
            case "BlackRook": this.GetComponent<SpriteRenderer>().sprite = BlackRook; player = "black"; break;
            case "BlackPawn": this.GetComponent<SpriteRenderer>().sprite = BlackPawn; player = "black"; break;

            case "WhiteQueen": this.GetComponent<SpriteRenderer>().sprite = WhiteQueen; player = "white"; break;
            case "WhiteKing": this.GetComponent<SpriteRenderer>().sprite = WhiteKing; player = "white"; break;
            case "WhiteBishop": this.GetComponent<SpriteRenderer>().sprite = WhiteBishop; player = "white"; break;
            case "WhiteKnight": this.GetComponent<SpriteRenderer>().sprite = WhiteKnight; player = "white"; break;
            case "WhiteRook": this.GetComponent<SpriteRenderer>().sprite = WhiteRook; player = "white"; break;
            case "WhitePawn": this.GetComponent<SpriteRenderer>().sprite = WhitePawn; player = "white"; break;
        }
    }
    public void SetCoords()
    {
        float x = xboard;
        float y = yboard;

        x *= 0.66f;
        y *=0.66f;
        x += -2.3f;
        y += -2.3f;

        this.transform.position = new Vector3(x, y, 0);

    }
    public int GetXBoard()
    {
        return xboard;
    }
    public int GetYBoard()
    {
        return yboard;
    }
    public void SetXboard(int x)
    {
        xboard = x;
    }
    public void SetYBoard(int y)
    {
        yboard = y;
    }
    private void OnMouseUp()
    {
        if (!controller[0].GetComponent<GameManager>().IsGameOver() && controller[0].GetComponent<GameManager>().GetCurrentPlayer() == player)
        {
            DestroyMovePlates();
            InitiateMovePlates();
        }
    }
    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for(int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }
    }
    public void InitiateMovePlates()
    {
        switch (this.name)
        {
            case "BlackQueen": 
            case "WhiteQueen":
                LineMovePlate(1,0);
                LineMovePlate(0, 1);
                LineMovePlate(1, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                LineMovePlate(-1, -1);
                LineMovePlate(1, -1);
                LineMovePlate(-1, 1);
                break;
            case "BlackKnight": 
            case "WhiteKnight":
                LMovePlate();
                break;
            case "BlackBishop": 
            case "WhiteBishop":
                LineMovePlate(1, 1);
                LineMovePlate(-1, -1);
                LineMovePlate(1, -1);
                LineMovePlate(-1, 1);
                break;
            case "BlackKing": 
            case "WhiteKing":
                SurroundMovePlate();
                break;

            case "BlackRook":
            case "WhiteRook": 
                LineMovePlate(1,0);
                LineMovePlate(0, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                break;
            case "BlackPawn":
                PawnMovePlate(xboard, yboard - 1);
                break;
            case "WhitePawn":
                PawnMovePlate(xboard, yboard + 1);
                break;
        }
    }

    public void LineMovePlate(int xIncrement,int yIncrement)
    {
        GameManager sc = controller[0].GetComponent<GameManager>();
        int x = xboard + xIncrement;
        int y = yboard + yIncrement;

        while(sc.PositionOnBoard(x, y)&&sc.getPosition(x,y)==null) {
            MovePlateSpawn(x, y);
            x += xIncrement;
            y += yIncrement;
        }
        if (sc.PositionOnBoard(x, y) && sc.getPosition(x, y).GetComponent<ChessMan>().player!=player)
        {
            MovePlateAttackSpawn(x, y);
        }
    }
    public void LMovePlate()
    {
        PointMovePlate(xboard + 1, yboard + 2);
        PointMovePlate(xboard - 1, yboard + 2);
        PointMovePlate(xboard + 2, yboard + 1);
        PointMovePlate(xboard + 2, yboard - 2);
        PointMovePlate(xboard + 1, yboard - 2);
        PointMovePlate(xboard - 1, yboard - 2);
        PointMovePlate(xboard - 2, yboard + 1);
        PointMovePlate(xboard - 2, yboard -1);
    }
    public void SurroundMovePlate()
    {
        PointMovePlate(xboard, yboard + 1);
        PointMovePlate(xboard, yboard - 1);
        PointMovePlate(xboard + 1, yboard);
        PointMovePlate(xboard - 1, yboard);
        PointMovePlate(xboard + 1, yboard+1);
        PointMovePlate(xboard + 1, yboard-1);
        PointMovePlate(xboard - 1, yboard+1);
        PointMovePlate(xboard - 1, yboard-1);

    }
    public void PointMovePlate(int x,int y)
    {
        GameManager sc = controller[0].GetComponent<GameManager>();
        if (sc.PositionOnBoard(x, y))
        {
            GameObject cp = sc.getPosition(x, y);
            if (cp == null) 
            { 
                MovePlateSpawn(x, y);
            }
            else if (cp.GetComponent<ChessMan>().player != player)
            {
                MovePlateAttackSpawn(x, y);
            }
        }
    }
    public void PawnMovePlate(int x, int y)
    {
        GameManager sc = controller[0].GetComponent<GameManager>();
        if (sc.PositionOnBoard(x, y))
        {
            if (sc.getPosition(x, y)==null)
            {
                MovePlateSpawn(x, y);
            }
            if(sc.getPosition(x+1,y)&& sc.getPosition(x+1,y)!=null&&sc.getPosition(x+1,y).GetComponent<ChessMan>().player!=player)
            {
                MovePlateAttackSpawn(x + 1, y);
            }
            if (sc.getPosition(x - 1, y) && sc.getPosition(x - 1, y) != null && sc.getPosition(x - 1, y).GetComponent<ChessMan>().player != player)
            {
                MovePlateAttackSpawn(x - 1, y);
            }
        }
    }
    public void MovePlateSpawn(int matrixX,int matrixY)
    {
        float x  = matrixX;
        float y = matrixY;
        x *= 0.66f;
        y *= 0.66f;
        x += -2.3f;
        y += -2.3f;
        GameObject mp = Instantiate(movePlate,new Vector3(x,y,0),Quaternion.identity);
        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX,matrixY);
    }
    public void MovePlateAttackSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;
        x *= 0.66f;
        y *= 0.66f;
        x += -2.3f;
        y += -2.3f;
        GameObject mp = Instantiate(movePlate, new Vector3(x, y, 0), Quaternion.identity);
        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = true;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }

}
