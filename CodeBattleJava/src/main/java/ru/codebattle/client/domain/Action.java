package ru.codebattle.client.domain;


import lombok.RequiredArgsConstructor;

@RequiredArgsConstructor
public enum Action {
    NONE("", ""),
    BEFORE_TURN("ACT,", ""),
    AFTER_TURN("", ",ACT");

    private final String preTurn;
    private final String postTurn;

    public String getPreTurn() {
        return preTurn;
    }

    public String getPostTurn() {
        return postTurn;
    }
}
