package ru.codebattle.client;

import java.util.function.Function;

import ru.codebattle.client.api.GameBoard;
import ru.codebattle.client.api.TurnAction;
import ru.codebattle.client.api.CodeBattleBase;

import java.net.URISyntaxException;

public class CodeBattleClient extends CodeBattleBase {

    private Function<GameBoard, TurnAction> callback;

    public CodeBattleClient(String url) throws URISyntaxException {
        super(url);
    }

    public void run(Function<GameBoard, TurnAction> callback) {
        connect();
        this.callback = callback;
    }

    @Override
    protected String doMove(GameBoard gameBoard) {
        clearScreen();
        gameBoard.printBoard();
        TurnAction action = callback.apply(gameBoard);
        var command = action.toString();
        System.out.println(command);
        return command;
    }

    public void clearScreen() {
        System.out.print("\033[H\033[2J");
        System.out.flush();
    }

    public void initiateExit()
    {
        setShouldExit(true);
    }
}
