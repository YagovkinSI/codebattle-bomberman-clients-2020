package ru.codebattle.client.api;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import lombok.Getter;

import static ru.codebattle.client.api.BoardElement.*;

public class GameBoard {

  public GameBoard(String boardString) {
    this.boardString = boardString.replace("\n", "");
  }

  @Getter
  private String boardString;

  public int size() {
    return (int) Math.sqrt(boardString.length());
  }

    public List<BoardPoint> getWalls() {
        return findAllElements(WALL);
    }
    public List<BoardPoint> getBomberman() {
        return findAllElements(BOMBERMAN, DEAD_BOMBERMAN, BOMB_BOMBERMAN);
    }
    public List<BoardPoint> getOtherBomberman() {
        return findAllElements(OTHER_BOMBERMAN, OTHER_DEAD_BOMBERMAN, OTHER_BOMB_BOMBERMAN);
    }
    public boolean amIDead() {
        return !findAllElements(DEAD_BOMBERMAN).isEmpty();
    }
    public List<BoardPoint> getDestroyableWalls() {
        return findAllElements(DESTROY_WALL);
    }
    public List<BoardPoint> getMeatchoppers() {
        return findAllElements(MEAT_CHOPPER);
    }
    public List<BoardPoint> getBlasts() {
        return findAllElements(BOOM);
    }
    public List<BoardPoint> getBombs() {
        return findAllElements(BOMB_BOMBERMAN, BOMB_TIMER_1, BOMB_TIMER_2, BOMB_TIMER_3,BOMB_TIMER_4,BOMB_TIMER_5);
    }
    public List<BoardPoint> getBarriers() {
        return findAllElements(
            WALL, DESTROY_WALL,
            MEAT_CHOPPER,
            OTHER_BOMBERMAN,
            OTHER_BOMB_BOMBERMAN, BOMB_TIMER_1, BOMB_TIMER_2, BOMB_TIMER_3, BOMB_TIMER_4, BOMB_TIMER_5 );
    }

    public boolean hasElementAt(BoardPoint point, BoardElement element) {
    if (point.isOutOfBoard(size())) {
      return false;
    }

    return getElementAt(point) == element;
  }

  public BoardElement getElementAt(BoardPoint point) {
    return BoardElement.valueOf(boardString.charAt(getShiftByPoint(point)));
  }

  public void printBoard() {
    for (int i = 0; i < size(); i++) {
      System.out.println(boardString.substring(i * size(), size() * (i + 1)));
    }
  }

  public BoardPoint findElement(BoardElement elementType) {
    for (int i = 0; i < size() * size(); i++) {
      BoardPoint pt = getPointByShift(i);
      if (hasElementAt(pt, elementType)) {
        return pt;
      }
    }
    return null;
  }

  public BoardPoint findFirstElement(BoardElement... elementType) {
      for (int i = 0; i < size() * size(); i++) {
          BoardPoint pt = getPointByShift(i);

          for (BoardElement elemType : elementType) {
              if (hasElementAt(pt, elemType)) {
                  return pt;
              }
          }
      }
      return null;
  }

  public List<BoardPoint> findAllElements(BoardElement... elementType) {
    List<BoardPoint> result = new ArrayList<>();

    for (int i = 0; i < size() * size(); i++) {
      BoardPoint pt = getPointByShift(i);

      for (BoardElement elemType : elementType) {
        if (hasElementAt(pt, elemType)) {
          result.add(pt);
        }
      }
    }

    return result;
  }

  public boolean hasElementAt(BoardPoint point, BoardElement... elements) {
    return Arrays.stream(elements).anyMatch(element -> hasElementAt(point, element));
  }

  private int getShiftByPoint(BoardPoint point) {
    return point.getY() * size() + point.getX();
  }

  private BoardPoint getPointByShift(int shift) {
    return new BoardPoint(shift % size(), shift / size());
  }
}
