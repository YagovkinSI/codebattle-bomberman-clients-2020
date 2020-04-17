using System;
using WebSocket4Net;

namespace CodeBattleNetLibrary
{
	public class GameClientBomberman
	{
		private readonly WebSocket _socket;
		private event Action OnUpdate;

		public BombermanBlocks[,] Map { get; private set; }
		public int MapSize { get; private set; }
		public int PlayerX { get; private set; }
		public int PlayerY { get; private set; }

		public GameClientBomberman(string server, string userEmail, string code)
		{
			MapSize = 0;

			_socket =
				new WebSocket(
					$"ws://{server}/codenjoy-contest/ws?user={userEmail}&code={code}");
            _socket.Opened += new EventHandler(onOpened);
            _socket.Error += new EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>(onError);
            _socket.Closed += new EventHandler(onClose);
            _socket.MessageReceived += (s, e) => { ParseField(e.Message); };
           
		}

		public void Run(Action handler)
		{
			OnUpdate += handler;
			_socket.Open();
		}

		public void Up(BombAction bombAction = BombAction.None)
		{
			send(
				$"{(bombAction == BombAction.BeforeTurn ? "ACT," : "")}UP{(bombAction == BombAction.AfterTurn ? ",ACT" : "")}"
				);
		}

		public void Down(BombAction bombAction = BombAction.None)
		{
            send(
				$"{(bombAction == BombAction.BeforeTurn ? "ACT," : "")}DOWN{(bombAction == BombAction.AfterTurn ? ",ACT" : "")}"
				);
		}

		public void Right(BombAction bombAction = BombAction.None)
		{
            send(
				$"{(bombAction == BombAction.BeforeTurn ? "ACT," : "")}RIGHT{(bombAction == BombAction.AfterTurn ? ",ACT" : "")}"
				);
		}

		public void Left(BombAction bombAction = BombAction.None)
		{
            send(
				$"{(bombAction == BombAction.BeforeTurn ? "ACT," : "")}LEFT{(bombAction == BombAction.AfterTurn ? ",ACT" : "")}"
				);
		}

		public void Act()
		{
            send("ACT");
		}

		public void Blank()
		{
            send("");
		}

		private void ParseField(string rawField)
		{
			rawField = rawField.Substring(6);
			int size = (int) Math.Sqrt(rawField.Length);
			if (MapSize != size)
			{
				Map = new BombermanBlocks[size, size];
				MapSize = size;
			}

			int rawPosition = 0;
			for (int j = 0; j < size; j++)
			{
				for (int i = 0; i < size; i++)
				{
					Map[i, j] = CharToBlock(rawField[rawPosition]);

					if (IsPlayerCoords(Map[i, j]))
					{
						PlayerX = i;
						PlayerY = j;
					}

					rawPosition++;
				}
			}

			OnUpdate?.Invoke();
		}

        private void onOpened(object sender, EventArgs e)
        {
            Console.WriteLine("Connection established");
        }

        private void onError(object sender, EventArgs e)
        {
            Console.WriteLine("### error ###\n" + e.ToString());
        }

        private void onClose(object sender, EventArgs e)
        {
            Console.WriteLine("### disconnected ###");
        }

        private void send(string msg)
        {
            Console.WriteLine($"Sending: {msg}");
            _socket.Send(msg);
        }

		protected bool IsPlayerCoords(BombermanBlocks block) => block == BombermanBlocks.Bomberman ||
		                                                        block == BombermanBlocks.BombBomberman ||
		                                                        block == BombermanBlocks.DeadBomberman;

		protected BombermanBlocks CharToBlock(char c) =>
			Enum.IsDefined(typeof (BombermanBlocks), (int) c)
				? (BombermanBlocks) c
				: BombermanBlocks.Unknown;
	}
}
