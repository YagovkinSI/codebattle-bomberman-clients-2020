#include "GameBoard.h"

GameBoard::GameBoard(BoardElement** map, int map_size)
{
	this->map = map;
	this->map_size = map_size;
}


bool GameBoard::hasElementAt(BoardPoint point, BoardElement element) {
	if (point.isOutOfBoard(map_size)) {
		return false;
	}
	return getElementAt(point) == element;
}
BoardElement GameBoard::getElementAt(BoardPoint point) {
	return map[point.getX()][point.getY()];
}
std::list<BoardPoint> GameBoard::findAllElements(BoardElement element) {
	std::list<BoardPoint> result;
	for (uint32_t j = 0; j < map_size; j++)
	{
		for (uint32_t i = 0; i < map_size; i++)
		{
			if (map[j][i] == element) {
				result.push_back(BoardPoint(j, i));
			}
		}
	}
	return result;
}
void GameBoard::printBoard() {

}

std::list<BoardPoint> GameBoard::getWalls() {
	return findAllElements(BoardElement::WALL);
}

bool GameBoard::isBarrierAt(BoardPoint point) {
	std::list<BoardPoint> result = getBarriers();
	BoardPoint resultPoint = result.front();
	return resultPoint == point;
}

bool GameBoard::isMyBombermanDead() {
	std::list<BoardPoint> isDead = findAllElements(BoardElement::DEAD_BOMBERMAN);
	if (!isDead.empty()) {
		return true;
	}
	return false;
}

std::list<BoardPoint> GameBoard::getDestroyableWalls() {
	return findAllElements(BoardElement::WALL_DESTROYABLE);
}

std::list<BoardPoint> GameBoard::getMeatChoppers() {
	return findAllElements(BoardElement::MEATCHOPPER);
}

std::list<BoardPoint> GameBoard::getBomberman() {
	std::list<BoardPoint> result = findAllElements(BoardElement::BOMBERMAN);
	result.splice(result.end(), findAllElements(BoardElement::DEAD_BOMBERMAN));
	result.splice(result.end(), findAllElements(BoardElement::BOMB_BOMBERMAN));
	return result;
}


std::list<BoardPoint> GameBoard::getDeadBomberman() {
	return findAllElements(BoardElement::DEAD_BOMBERMAN);
}

std::list<BoardPoint> GameBoard::getOtherBombermans() {
	std::list<BoardPoint> result = findAllElements(BoardElement::OTHER_BOMBERMAN);
	result.splice(result.end(), findAllElements(BoardElement::OTHER_DEAD_BOMBERMAN));
	result.splice(result.end(), findAllElements(BoardElement::OTHER_BOMB_BOMBERMAN));
	return result;
}

std::list<BoardPoint> GameBoard::getOtherDeadBombermans() {
	return findAllElements(BoardElement::OTHER_DEAD_BOMBERMAN);
}


std::list<BoardPoint> GameBoard::getOtherBombermanBombs() {
	return findAllElements(BoardElement::OTHER_BOMB_BOMBERMAN);
}

std::list<BoardPoint> GameBoard::getBombs() {
	std::list<BoardPoint> result = findAllElements(BoardElement::BOMB_TIMER_1);
	result.splice(result.end(), findAllElements(BoardElement::BOMB_BOMBERMAN));
	result.splice(result.end(), findAllElements(BoardElement::BOMB_TIMER_2));
	result.splice(result.end(), findAllElements(BoardElement::BOMB_TIMER_3));
	result.splice(result.end(), findAllElements(BoardElement::BOMB_TIMER_4));
	result.splice(result.end(), findAllElements(BoardElement::BOMB_TIMER_5));
	return result;
}

std::list<BoardPoint> GameBoard::getBlasts() {
	return findAllElements(BoardElement::BOOM);
}

std::list<BoardPoint> GameBoard::getBarriers() {
	std::list<BoardPoint> result = findAllElements(BoardElement::WALL);
	result.splice(result.end(), findAllElements(BoardElement::MEATCHOPPER));
	result.splice(result.end(), findAllElements(BoardElement::BOMB_TIMER_1));
	result.splice(result.end(), findAllElements(BoardElement::BOMB_TIMER_2));
	result.splice(result.end(), findAllElements(BoardElement::BOMB_TIMER_3));
	result.splice(result.end(), findAllElements(BoardElement::BOMB_TIMER_4));
	result.splice(result.end(), findAllElements(BoardElement::BOMB_TIMER_5));
	result.splice(result.end(), findAllElements(BoardElement::WALL_DESTROYABLE));
	result.splice(result.end(), findAllElements(BoardElement::OTHER_BOMBERMAN));
	result.splice(result.end(), findAllElements(BoardElement::OTHER_BOMB_BOMBERMAN));
	return result;
}

GameBoard::~GameBoard()
{
	delete map;
}

