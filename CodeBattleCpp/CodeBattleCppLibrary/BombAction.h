#pragma once

#include <cstdint>

enum class BombAction : uint8_t
{
	None = 0,
	BeforeTurn = 1,
	AfterTurn = 2
};
