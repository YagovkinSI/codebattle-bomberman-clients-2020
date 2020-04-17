#pragma once

#include <string>
#include <thread>
#include "easywsclient\easywsclient.hpp"
#ifdef _WIN32
#pragma comment( lib, "ws2_32" )
#include <WinSock2.h>
#endif
#include <assert.h>
#include <stdio.h>
#include <iostream>
#include <string>
#include <memory>

#include "BombermanBlocks.h"
#include "BombAction.h"

class GameClientBomberman
{
	BombermanBlocks **map;
	uint32_t map_size, player_x, player_y;

	easywsclient::WebSocket *web_socket;
	std::string path;

	bool is_running;
	std::thread *work_thread;
	void update_func(std::function<void()> _message_handler);

public:
	GameClientBomberman(std::string _server, std::string _userEmail, std::string _userPassword = "");
	~GameClientBomberman();

	void Run(std::function<void()> _message_handler);
	void Up(BombAction _action = BombAction::None)
	{
		send(std::string(_action == BombAction::BeforeTurn ? "ACT," : "") + "UP" + std::string(_action == BombAction::AfterTurn ? ",ACT" : ""));
	}
	void Down(BombAction _action = BombAction::None)
	{
		send(std::string(_action == BombAction::BeforeTurn ? "ACT," : "") + "DOWN" + std::string(_action == BombAction::AfterTurn ? ",ACT" : ""));
	}
	void Right(BombAction _action = BombAction::None)
	{
		send(std::string(_action == BombAction::BeforeTurn ? "ACT," : "") + "RIGHT" + std::string(_action == BombAction::AfterTurn ? ",ACT" : ""));
	}
	void Left(BombAction _action = BombAction::None)
	{
		send(std::string(_action == BombAction::BeforeTurn ? "ACT," : "") + "LEFT" + std::string(_action == BombAction::AfterTurn ? ",ACT" : ""));
	}
	void Act() { send("ACT"); }
	void Blank() { send(""); }

	BombermanBlocks **get_map() { return map; }
	uint32_t get_map_size() { return map_size; }
	uint32_t get_player_x() { return player_x; }
	uint32_t get_player_y() { return player_y; }

private:
	void send(std::string msg)
	{
		std::cout << "Sending: " << msg << std::endl;
		web_socket->send(msg);
	}
};
