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
            case"BlackQueen":this.GetComponent<SpriteRenderer>().sprite = BlackQueen;break;
            case "BlackKing": this.GetComponent<SpriteRenderer>().sprite = BlackKing; break;
            case "BlackBishop": this.GetComponent<SpriteRenderer>().sprite = BlackBishop; break;
            case "BlackKnight": this.GetComponent<SpriteRenderer>().sprite = BlackKnight; break;
            case "BlackRook": this.GetComponent<SpriteRenderer>().sprite = BlackRook; break;
            case "BlackPawn": this.GetComponent<SpriteRenderer>().sprite = BlackPawn; break;

            case "WhiteQueen": this.GetComponent<SpriteRenderer>().sprite = WhiteQueen; break;
            case " WhiteKing": this.GetComponent<SpriteRenderer>().sprite = WhiteKing; break;
            case "WhiteBishop": this.GetComponent<SpriteRenderer>().sprite = WhiteBishop; break;
            case "WhiteKnight": this.GetComponent<SpriteRenderer>().sprite = WhiteKnight; break;
            case "WhiteRook": this.GetComponent<SpriteRenderer>().sprite = WhiteRook; break;
            case "WhitePawn": this.GetComponent<SpriteRenderer>().sprite = WhitePawn; break;
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
}
