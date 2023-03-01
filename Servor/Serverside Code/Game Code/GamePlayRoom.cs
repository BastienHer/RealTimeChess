using System;
using System.Collections.Generic;
using PlayerIO.GameLibrary;

namespace MushroomsUnity3DExample
{
    [RoomType("UnityRealTimeChessGameplay")]
    public class GamePlayRoom : Game<Player>
    {
        //private int last_toad_id = 0;
        private List<Toad> Toads = new List<Toad>();

        // This method is called when an instance of your the game is created
        public override void GameStarted()
        {
            // anything you write to the Console will show up in the 
            // output window of the development server
            Console.WriteLine("Game is started: " + RoomId);
        }

        public override void GameClosed()
        {
            Console.WriteLine("RoomId: " + RoomId);
        }

        // This method is called whenever a player joins the game
        public override void UserJoined(Player player)
        {
            Console.WriteLine("Joueur connecté");
            foreach (Player pl in Players)
            {
                if (pl.ConnectUserId != player.ConnectUserId)
                {
                    pl.Send("PlayerJoined", player.ConnectUserId, 0, 0);
                    player.Send("PlayerJoined", pl.ConnectUserId, pl.posx, pl.posz);
                }
            }
        }

        // This method is called when a player leaves the game
        public override void UserLeft(Player player)
        {
            Broadcast("PlayerLeft", player.ConnectUserId);
        }

        // This method is called when a player sends a message into the server code
        public override void GotMessage(Player player, Message message)
        {
            switch (message.Type)
            {
                case "Ready":
                    foreach (Player pl in Players)
                    {
                        if (pl.ConnectUserId != player.ConnectUserId)
                        {
                            pl.Send("OpponentReady");
                        }
                    }
                    break;
                case "StartGame":
                    Broadcast("StartGame");
                    break;
            }
        }
    }
}