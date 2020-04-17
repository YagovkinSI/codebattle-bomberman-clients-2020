require_relative 'CodeBattleRubyLibrary'

def isBlock?(block)
  [
    :Wall,
    :WallDestroyable,
    :MeatChopper,
    :BombTimer1,
    :BombTimer2,
    :BombTimer3,
    :BombTimer4,
    :BombTimer5,
    :OtherBomberman,
    :OtherBombBomberman
  ].include?(block)
end

def turn(gcb)
  done = false
  r = rand(5)
  case r
  when 0
    if isBlock?(gcb.map[gcb.playerY - 1][gcb.playerX]) == false
      gcb.up(BombAction::BeforeTurn)
      done = true
    end
  when 1
    if isBlock?(gcb.map[gcb.playerY - 1][gcb.playerX]) == false
      gcb.down(BombAction::BeforeTurn)
      done = true
    end
  when 2
    if isBlock?(gcb.map[gcb.playerY - 1][gcb.playerX]) == false
      gcb.right(BombAction::BeforeTurn)
      done = true
    end
  when 3
    if isBlock?(gcb.map[gcb.playerY - 1][gcb.playerX]) == false
      gcb.left(BombAction::BeforeTurn)
      done = true
    end
  when 4
    gcb.act
    done = true
  else
  end
  if done == false
    gcb.blank
  end
end

w = GameClient.new("localhost:8080", "e@mail.org", "00000000000000000000")
w.run(method(:turn))
