package ru.codebattle.client;


import lombok.SneakyThrows;
import ru.codebattle.client.domain.Action;
import ru.codebattle.client.domain.BombermanBlock;

import java.util.Collections;
import java.util.HashSet;
import java.util.Random;
import java.util.Set;

public class CodeBattleJava {

    private static final String SERVER_ADDRESS = "localhost:8080";
    private static final String PLAYER_NAME = "e@mail.org";
    private static final String AUTH_CODE = "00000000000000000000";

    private static final Set<BombermanBlock> BLOCK_TYPES = Collections.unmodifiableSet(new HashSet<BombermanBlock>() {{
        add(BombermanBlock.WALL);
        add(BombermanBlock.DESTROY_WALL);
        add(BombermanBlock.MEAT_CHOPPER);
        add(BombermanBlock.BOMB_TIMER_1);
        add(BombermanBlock.BOMB_TIMER_2);
        add(BombermanBlock.BOMB_TIMER_3);
        add(BombermanBlock.BOMB_TIMER_4);
        add(BombermanBlock.BOMB_TIMER_5);
        add(BombermanBlock.OTHER_BOMBERMAN);
        add(BombermanBlock.OTHER_BOMB_BOMBERMAN);
    }});


    @SneakyThrows
    public static void main(String[] args) {
        CodeBattleJavaLibrary client = new CodeBattleJavaLibrary(SERVER_ADDRESS, PLAYER_NAME, AUTH_CODE);
        Random random = new Random();
        client.run(map -> {
            boolean done = false;
            switch (random.nextInt(5)) {
                case 0:
                    if (!isBlock(map[client.getPlayerX()][client.getPlayerY() - 1])) {
                        client.up(Action.BEFORE_TURN);
                        done = true;
                    }
                    break;
                case 1:
                    if (!isBlock(map[client.getPlayerX() + 1][client.getPlayerY()])) {
                        client.right(Action.BEFORE_TURN);
                        done = true;
                    }
                    break;
                case 2:
                    if (!isBlock(map[client.getPlayerX()][client.getPlayerY() + 1])) {
                        client.down(Action.BEFORE_TURN);
                        done = true;
                    }
                    break;
                case 3:
                    if (!isBlock(map[client.getPlayerX() -1 ][client.getPlayerY()])) {
                        client.left(Action.BEFORE_TURN);
                        done = true;
                    }
                    break;
                case 4:
                    client.act();
                    done = true;
                    break;
            }
            if (!done) {
                client.blank();
            }
        });
    }

    private static boolean isBlock(BombermanBlock e) {
        return BLOCK_TYPES.contains(e);
    }
}
