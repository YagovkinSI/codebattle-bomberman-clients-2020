from bombermanclient.CodeBattlePythonLibrary import GameClient
from bombermanclient.CodeBattlePythonLibrary import BombermanBlocks
from bombermanclient.CodeBattlePythonLibrary import BombAction
import math
import random
import logging

logging.basicConfig(format='%(asctime)s %(levelname)s:%(message)s',
                    level=logging.INFO)


def isBlock(block):
    return block in [
        BombermanBlocks.Wall,
        BombermanBlocks.WallDestroyable,
        BombermanBlocks.MeatChopper,
        BombermanBlocks.BombTimer1,
        BombermanBlocks.BombTimer2,
        BombermanBlocks.BombTimer3,
        BombermanBlocks.BombTimer4,
        BombermanBlocks.BombTimer5,
        BombermanBlocks.OtherBomberman,
        BombermanBlocks.OtherBombBomberman]


def turn(gcb):
    done = False
    val = math.ceil(random.random() * 5 - 0.5)

    if val == 0:
        if isBlock(gcb.map[gcb.playerY - 1][gcb.playerX]) == False:
            gcb.up(BombAction.BeforeTurn)
            done = True
    if val == 1:
        if isBlock(gcb.map[gcb.playerY][gcb.playerX + 1]) == False:
            gcb.right(BombAction.BeforeTurn)
            done = True
    if val == 2:
        if isBlock(gcb.map[gcb.playerY + 1][gcb.playerX]) == False:
            gcb.down(BombAction.BeforeTurn)
            done = True
    if val == 3:
        if isBlock(gcb.map[gcb.playerY][gcb.playerX - 1]) == False:
            gcb.left(BombAction.BeforeTurn)
            done = True
    if val == 4:
        gcb.act()
        done = True
    if done == False:
        gcb.blank()


if __name__ == '__main__':
    random.seed()
    gcb = GameClient("localhost:8080", "e@mail.org", "00000000000000000000")
    gcb.run(turn)
