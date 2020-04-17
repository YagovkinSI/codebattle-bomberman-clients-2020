#include <iostream>
#include <random>

#include "GameClientBomberman.h"

bool is_block(BombermanBlocks _block)
{
	return
		_block == BombermanBlocks::Wall ||
		_block == BombermanBlocks::WallDestroyable ||
		_block == BombermanBlocks::MeatChopper ||
		_block == BombermanBlocks::BombTimer1 ||
		_block == BombermanBlocks::BombTimer2 ||
		_block == BombermanBlocks::BombTimer3 ||
		_block == BombermanBlocks::BombTimer4 ||
		_block == BombermanBlocks::BombTimer5 ||
		_block == BombermanBlocks::OtherBomberman ||
		_block == BombermanBlocks::OtherBombBomberman;
}

void main()
{
	srand(time(0));
	GameClientBomberman *gcb = new GameClientBomberman("localhost:8080", "e@mail.org", "00000000000000000000");
	gcb->Run([&]()
	{
		BombermanBlocks **map = gcb->get_map();
		bool done = false;

		switch (rand() % 5)
		{
		case 0: if (is_block(map[gcb->get_player_y() - 1][gcb->get_player_x()]) == false) { gcb->Up(BombAction::BeforeTurn);    done = true; } break;
		case 1: if (is_block(map[gcb->get_player_y()][gcb->get_player_x() + 1]) == false) { gcb->Right(BombAction::BeforeTurn); done = true; } break;
		case 2: if (is_block(map[gcb->get_player_y() + 1][gcb->get_player_x()]) == false) { gcb->Down(BombAction::BeforeTurn);  done = true; } break;
		case 3: if (is_block(map[gcb->get_player_y()][gcb->get_player_x() - 1]) == false) { gcb->Left(BombAction::BeforeTurn);  done = true; } break;
		case 4: gcb->Act(); done = true; break;
		}
		if (done == false)
			gcb->Blank();
	});

	getchar();
}
