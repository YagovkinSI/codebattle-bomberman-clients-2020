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
#include <functional>

#include "BoardElement.h"
#include "TurnAction.h"
#include "GameBoard.h"

class GameClient
{
	BoardElement **map;
	GameBoard *board;
	uint32_t map_size, player_x, player_y;

	easywsclient::WebSocket *web_socket;
	std::string path;

	bool is_running;
	std::thread *work_thread;
	void update_func(std::function<void()> _message_handler);

public:
	GameClient(std::string _server);
	~GameClient();

	void Run(std::function<void()> _message_handler);

	void Up(TurnAction _action = TurnAction::None)
	{
		send(std::string(_action == TurnAction::BeforeTurn ? "ACT," : "") + "UP" + std::string(_action == TurnAction::AfterTurn ? ",ACT" : ""));
	}
	void Down(TurnAction _action = TurnAction::None)
	{
		send(std::string(_action == TurnAction::BeforeTurn ? "ACT," : "") + "DOWN" + std::string(_action == TurnAction::AfterTurn ? ",ACT" : ""));
	}
	void Right(TurnAction _action = TurnAction::None)
	{
		send(std::string(_action == TurnAction::BeforeTurn ? "ACT," : "") + "RIGHT" + std::string(_action == TurnAction::AfterTurn ? ",ACT" : ""));
	}
	void Left(TurnAction _action = TurnAction::None)
	{
		send(std::string(_action == TurnAction::BeforeTurn ? "ACT," : "") + "LEFT" + std::string(_action == TurnAction::AfterTurn ? ",ACT" : ""));
	}
	void Act() {
		send("ACT");
	}
	void Blank() { send(""); }


	GameBoard* get_GameBoard() { return board; }
private:
	void send(std::string msg)
	{
		std::cout << "Sending: " << msg << std::endl;
		web_socket->send(msg);
	}
};
