from codebattleclient.CodeBattleClient import GameClient
import random
import logging

from codebattleclient.internals.TurnAction import TurnAction
from codebattleclient.internals.Board import Board

logging.basicConfig(format='%(asctime)s %(levelname)s:%(message)s',
                    level=logging.INFO)


def turn(gcb: Board):
    # your code is here
    return random.choice(list(TurnAction))


def main():
    gcb = GameClient(
        "http://epruizhsa0001t2:8080/codenjoy-contest/board/player/ygzwtnd2yae0jn0y8viy?code=1104845394797194217&gameName=bomberman")
    gcb.run(turn)

if __name__ == '__main__':
    main()