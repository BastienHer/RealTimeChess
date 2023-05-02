using System;
using System.Collections.Generic;
using PlayerIO.GameLibrary;

namespace MushroomsUnity3DExample {
	public class Player : BasePlayer {
		public float posx = 0;
		public float posz = 0;
		public int toadspicked = 0;
	}

	public class Toad {
		public int id = 0;
		public float posx = 0;
		public float posz = 0;
	}

	[RoomType("UnityRealTimeChess")]
	public class GameCode : Game<Player> {
		//private int last_toad_id = 0;
		private List<Toad> Toads = new List<Toad>(); 

		// This method is called when an instance of your the game is created
		public override void GameStarted() {
			Console.WriteLine("Game is started: " + RoomId);

		}

		public override void GameClosed() {
			Console.WriteLine("RoomId: " + RoomId);
		}

		// This method is called whenever a player joins the game
		public override void UserJoined(Player player) {
			Console.WriteLine("Joueur connecté");
			foreach(Player pl in Players) {
				if(pl.ConnectUserId != player.ConnectUserId) {
					pl.Send("PlayerJoined", player.ConnectUserId, 0, 0);
					player.Send("PlayerJoined", pl.ConnectUserId, pl.posx, pl.posz);
				}
			}
            if (this.PlayerCount>1)
            {
				Console.WriteLine("Plus de 2 joueurs");
				Random rdn = new Random();
				int randomRoom = rdn.Next(1, int.MaxValue);
				Broadcast("ChangeRoom",randomRoom);


				List<Player> newPlayers = new List<Player>(Players);
				foreach (Player pl in newPlayers)
				{
					Console.WriteLine("deconnect" + pl.Id);
					pl.Disconnect();
				}
			}
			Console.WriteLine(this.PlayerCount);

		}

		// This method is called when a player leaves the game
		public override void UserLeft(Player player) {
			Broadcast("PlayerLeft", player.ConnectUserId);
		}

		// This method is called when a player sends a message into the server code
		public override void GotMessage(Player player, Message message) {
			switch(message.Type) {
				
			}
		}
	}
}