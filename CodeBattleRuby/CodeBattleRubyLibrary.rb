require 'websocket'
require 'websocket-eventmachine-client'

BombermanBlocks =
{
  Unknown: '\0',

  Bomberman: '☺',
  BombBomberman: '☻',
  DeadBomberman: 'Ѡ',

  OtherBomberman: '♥',
  OtherBombBomberman: '♠',
  OtherDeadBomberman: '♣',

  BombTimer5: '5',
  BombTimer4: '4',
  BombTimer3: '3',
  BombTimer2: '2',
  BombTimer1: '1',
  Boom: '҉',

  Wall: '☼',
  WallDestroyable: '#',
  DestroyedWall: 'H',

  MeatChopper: '&',
  DeadMeatChopper: 'x',

  Space: ' '
}

module BombAction
  None = 0,
  BeforeTurn = 1,
  AfterTurn = 2
end

class GameClient
  attr_reader :mapSize
  attr_reader :map
  attr_reader :playerX
  attr_reader :playerY

  def initialize(server, userEmail, code)
    @path = "ws://" + server + "/codenjoy-contest/ws?user=" + userEmail + "&code=" + code
  end

  def run(on_turn)
    EM.run do
      @ws = WebSocket::EventMachine::Client.connect(:uri => @path)
      
      @ws.onopen do
        puts "Connection established"
      end
    
      @ws.onmessage do |message, type|
        message.force_encoding("UTF-8")
        message = message[6, message.length - 6]

        @mapSize = Math.sqrt(message.length).to_i
        @map = Array.new(@mapSize) {Array.new(@mapSize, :Unknown)}
        messageCur = 0
        for j in 0..(@mapSize - 1)
          for i in 0..(@mapSize - 1)
            BombermanBlocks.each do |key, value|
              if message[messageCur] == value
                @map[j][i] = key
                if key == :Bomberman || key == :BombBomberman || key == :DeadBomberman
                  @playerX = i
                  @playerY = j
                end
                break
              end
            end
            messageCur = messageCur + 1
          end
        end
        
        on_turn.call(self)
      end
    
      @ws.onclose do |code, reason|
        puts "Disconnected with status code: #{code}"
      end
    end
  end

  def send(msg = "")
    puts "Sending: #{msg}"
    @ws.send msg
  end
  
  def up(action = BombAction::None)
    send "#{action == BombAction::BeforeTurn ? "ACT," : ""}UP#{action == BombAction::AfterTurn ? ",ACT" : ""}"
  end
  
  def down(action = BombAction::None)
    send "#{action == BombAction::BeforeTurn ? "ACT," : ""}DOWN#{action == BombAction::AfterTurn ? ",ACT" : ""}"
  end
  
  def right(action = BombAction::None)
    send "#{action == BombAction::BeforeTurn ? "ACT," : ""}RIGHT#{action == BombAction::AfterTurn ? ",ACT" : ""}"
  end
  
  def left(action = BombAction::None)
    send "#{action == BombAction::BeforeTurn ? "ACT," : ""}LEFT#{action == BombAction::AfterTurn ? ",ACT" : ""}"
  end
  
  def act
    send "ACT"
  end
  
  def blank
    send ""
  end

  private :send
end
