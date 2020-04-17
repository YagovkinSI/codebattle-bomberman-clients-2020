using System;
using CodeBattleNetLibrary;

namespace CodeBattleNet
{
	internal static class Program
	{
		private static void Main()
		{
			var r = new Random();
			var gcb = new GameClientBomberman("localhost:8080", "e@mail.org", "00000000000000000000");
			gcb.Run(() =>
			{
				var done = false;

				switch (r.Next(5))
				{
					case 0:
						if (IsBlock(gcb.Map[gcb.PlayerX, gcb.PlayerY - 1]) == false)
						{
							gcb.Up(BombAction.BeforeTurn);
							done = true;
						}
						break;
					case 1:
						if (IsBlock(gcb.Map[gcb.PlayerX + 1, gcb.PlayerY]) == false)
						{
							gcb.Right(BombAction.BeforeTurn);
							done = true;
						}
						break;
					case 2:
						if (IsBlock(gcb.Map[gcb.PlayerX, gcb.PlayerY + 1]) == false)
						{
							gcb.Down(BombAction.BeforeTurn);
							done = true;
						}
						break;
					case 3:
						if (IsBlock(gcb.Map[gcb.PlayerX - 1, gcb.PlayerY]) == false)
						{
							gcb.Left(BombAction.BeforeTurn);
							done = true;
						}
						break;
					case 4:
						gcb.Act();
						done = true;
						break;
				}
				if (done == false)
					gcb.Blank();
			});

			Console.ReadKey();
		}

		private static bool IsBlock(BombermanBlocks block) =>
			block == BombermanBlocks.Wall ||
			block == BombermanBlocks.WallDestroyable ||
			block == BombermanBlocks.MeatChopper ||
			block == BombermanBlocks.BombTimer1 ||
			block == BombermanBlocks.BombTimer2 ||
			block == BombermanBlocks.BombTimer3 ||
			block == BombermanBlocks.BombTimer4 ||
			block == BombermanBlocks.BombTimer5 ||
			block == BombermanBlocks.OtherBomberman ||
			block == BombermanBlocks.OtherBombBomberman;
	}
}
