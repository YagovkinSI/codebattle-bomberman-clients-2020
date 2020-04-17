package ru.codebattle.client;


import lombok.Getter;
import lombok.extern.slf4j.Slf4j;
import org.java_websocket.client.WebSocketClient;
import org.java_websocket.handshake.ServerHandshake;
import ru.codebattle.client.domain.Action;
import ru.codebattle.client.domain.BombermanBlock;

import java.net.URI;
import java.net.URISyntaxException;
import java.util.function.Consumer;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import static java.lang.String.format;
import static java.util.Optional.ofNullable;
import static ru.codebattle.client.domain.Action.NONE;

@Slf4j
public class CodeBattleJavaLibrary extends WebSocketClient {
    private static final Pattern DATA_PATTERN = Pattern.compile("^board=(.*)$");
    private Consumer<BombermanBlock[][]> handler;

    @Getter
    private int playerX;

    @Getter
    private int playerY;

    public CodeBattleJavaLibrary(String serverAddress, String user, String code) throws URISyntaxException {
        super(new URI(format("ws://%s/codenjoy-contest/ws?user=%s&code=%s", serverAddress, user, code)));
    }

    @Override
    public void onOpen(ServerHandshake handShakeData) {
        log.info("Connection established");
    }

    @Override
    public void onClose(int code, String reason, boolean remote) {
        log.warn("### disconnected ###");
    }

    @Override
    public void onError(Exception ex) {
        log.error("### error ###", ex);
    }

    @Override
    public void onMessage(String message) {   // "description": "No more Levels. You win!"
        ofNullable(message)
                .map(this::parseField)
                .map(msg -> {
                    handler.accept(msg);
                    return null;
                });
    }

    public void run(Consumer<BombermanBlock[][]> handler) {
        this.handler = handler;
        connect();
    }

    public final void up(Action action) {
        sendMsg("UP", action);
    }

    public final void down(Action action) {
        sendMsg("DOWN", action);
    }

    public final void left(Action action) {
        sendMsg("LEFT", action);
    }

    public final void right(Action action) {
        sendMsg("RIGHT", NONE);
    }

    public final void up() {
        sendMsg("UP", NONE);
    }

    public final void down() {
        sendMsg("DOWN", NONE);
    }

    public final void left() {
        sendMsg("LEFT", NONE);
    }

    public final void right() {
        sendMsg("RIGHT", NONE);
    }

    public final void act() {
        sendMsg("ACT", NONE);
    }

    public final void blank() {
        sendMsg("", NONE);
    }

    private BombermanBlock[][] parseField(String boardString) {
        Matcher matcher = DATA_PATTERN.matcher(boardString);
        if (!matcher.matches()) {
            throw new RuntimeException("Error parsing data: " + boardString);
        }
        boardString = matcher.group(1);
        String board = boardString.replaceAll("\n", "");
        int size = (int) Math.sqrt(board.length());
        BombermanBlock[][] field = new BombermanBlock[size][size];
        board = boardString.replaceAll("\n", "");

        char[] temp = board.toCharArray();
        for (int y = 0; y < size; y++) {
            int dy = y * size;
            for (int x = 0; x < size; x++) {
                field[x][y] = BombermanBlock.valueOf(temp[dy + x]);
                if (isPlayerCoords(field[x][y])) {
                    playerX = x;
                    playerY = y;
                }
            }
        }
        return field;
    }

    private void sendMsg(String direction, Action action) {
        action = action == null ? NONE : action;
        String msg = format("%s%s%s", action.getPreTurn(), direction, action.getPostTurn());
        log.info("Sending: {}", msg);
        send(msg);
    }

    private boolean isPlayerCoords(BombermanBlock e) {
        return e == BombermanBlock.BOMBERMAN
                || e == BombermanBlock.BOMB_BOMBERMAN
                || e == BombermanBlock.DEAD_BOMBERMAN;
    }

    @Override
    protected void finalize() throws Throwable {
        close();
    }
}
