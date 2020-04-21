#pragma once

#include <cstdint>

enum class BoardElement : uint16_t
{
	NONE = L' ',
	BOMBERMAN = L'☺',
	BOMB_BOMBERMAN = L'☻',
	DEAD_BOMBERMAN = L'Ѡ',

	OTHER_BOMBERMAN = L'♥',
	OTHER_BOMB_BOMBERMAN = L'♠',
	OTHER_DEAD_BOMBERMAN = L'♣',

	BOMB_TIMER_5 = L'5',
	BOMB_TIMER_4 = L'4',
	BOMB_TIMER_3 = L'3',
	BOMB_TIMER_2 = L'2',
	BOMB_TIMER_1 = L'1',
	BOOM = L'҉',

	WALL = L'☼',
	WALL_DESTROYABLE = L'#',
	WALL_DESTROYED = L'H',

	MEATCHOPPER = L'&',
	DEAD_MEATCHOPPER = L'x'
	
};