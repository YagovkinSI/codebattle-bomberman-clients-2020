#pragma once

#include "BoardElement.h"
#include "BoardPoint.h"
#include <list>

class GameBoard
{
public:
	GameBoard(BoardElement** map, int map_size);
	bool hasElementAt(BoardPoint point, BoardElement element);
	BoardElement getElementAt(BoardPoint point);
	std::list<BoardPoint> findAllElements(BoardElement element);
	std::list<BoardPoint> getWalls();
	std::list<BoardPoint> getDestroyableWalls();
	std::list<BoardPoint> getMeatChoppers();
	std::list<BoardPoint> getBomberman();
	std::list<BoardPoint> getDeadBomberman();
	std::list<BoardPoint> getOtherBombermans();
	std::list<BoardPoint> getOtherDeadBombermans();
	std::list<BoardPoint> getOtherBombermanBombs();
	std::list<BoardPoint> getBombs();
	std::list<BoardPoint> getBlasts();
	bool isBarrierAt(BoardPoint point);
	bool isMyBombermanDead();
	std::list<BoardPoint> getBarriers();
	void printBoard();
	~GameBoard();

private:
	BoardElement** map;
	int map_size;
};