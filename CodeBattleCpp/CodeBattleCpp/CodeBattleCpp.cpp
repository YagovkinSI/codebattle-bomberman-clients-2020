#include <iostream>
#include <random>

#include "GameClient.h"

void main()
{
	srand(time(0));

	//После регистрации на сайте вы попадаете на свою игроую доску. Вставьте URL из строки браузера для инициализации клиента. 
	//ВАЖНО: В строке должны быть обязательно ид игрока (в примере: 82bc5roztht315o8f5yc) и код (в примере: 3242242588940318227)
	GameClient *gcb = new GameClient("http://localhost:8080/codenjoy-contest/board/player/82bc5roztht315o8f5yc?code=3242242588940318227&gameName=snakebattle");
	gcb->Run([&]()
	{
			//Используем реализованную библиотеку, которая описывает игровую доску. Вы можете ее модифицировать по вашему смотрению.
			GameBoard* gb = gcb->get_GameBoard();
			
			//Пример использования списка бомб
			std::list<BoardPoint> bombs = gb->getBombs();
			for each (BoardPoint bomb in bombs)
			{
				bomb.print();
			}
			//

			bool done = false; //Вам обязательно надо слать команду на сервер, даже если вы ничего не сделали
			switch (rand() % 12) {
				case 0: gcb->Up(); done = true; break; //Одиночкая команда
				case 1: gcb->Down(); done = true; break;
				case 2: gcb->Right(); done = true; break;
				case 3: gcb->Left(); done = true; break;
				case 4: gcb->Up(TurnAction::AfterTurn); done = true; break; //Двойная команда
				case 5: gcb->Down(TurnAction::AfterTurn); done = true; break;
				case 6: gcb->Left(TurnAction::AfterTurn); done = true; break;
				case 7: gcb->Right(TurnAction::AfterTurn); done = true; break;
				case 8: gcb->Up(TurnAction::BeforeTurn); done = true; break;
				case 9: gcb->Down(TurnAction::BeforeTurn); done = true; break;
				case 10: gcb->Left(TurnAction::BeforeTurn); done = true; break;
				case 11: gcb->Right(TurnAction::BeforeTurn); done = true; break;
			}
			//вы не знаете что делать? Шлем пустую команду
			if (!done) {
				gcb->Blank();
			}
	});

	getchar();
}
