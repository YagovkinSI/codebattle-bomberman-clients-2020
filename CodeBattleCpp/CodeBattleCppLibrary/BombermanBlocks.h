#pragma once

#include <cstdint>

enum class BombermanBlocks : uint16_t
{
	Unknown = 0,

	Bomberman = L'☺',
	BombBomberman = L'☻',
	DeadBomberman = L'Ѡ',

	OtherBomberman = L'♥',
	OtherBombBomberman = L'♠',
	OtherDeadBomberman = L'♣',

	BombTimer5 = L'5',
	BombTimer4 = L'4',
	BombTimer3 = L'3',
	BombTimer2 = L'2',
	BombTimer1 = L'1',
	Boom = L'҉',

	Wall = L'☼',
	WallDestroyable = L'#',
	DestroyedWall = L'H',

	MeatChopper = L'&',
	DeadMeatChopper = L'x',

	Space = L' '
};

