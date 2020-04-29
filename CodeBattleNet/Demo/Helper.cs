using Bomberman.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public static class Helper
    {
        public static Direction[] AllDirections = new[] { Direction.Up, Direction.Right, Direction.Down, Direction.Left };
        public static Element[] BombElements = new[] { Element.BOMB_BOMBERMAN, Element.BOMB_TIMER_1, Element.BOMB_TIMER_2,
            Element.BOMB_TIMER_3, Element.BOMB_TIMER_4, Element.BOMB_TIMER_5, Element.OTHER_BOMB_BOMBERMAN};
        public static Element[] WallElements = new[] { Element.DestroyedWall, Element.WALL };

        public static Point GetNewPosition(Direction direction, Point point)
        {
            switch (direction)
            {
                case Direction.Up:
                    return new Point(point.X, point.Y + 1);
                case Direction.Right:
                    return new Point(point.X + 1, point.Y);
                case Direction.Down:
                    return new Point(point.X, point.Y - 1);
                case Direction.Left:
                    return new Point(point.X - 1, point.Y);
                default:
                    return point;
            }
        }

        public static bool IsAnyBomb(Board board, Point point)
        {
            return BombElements.Contains(board.GetAt(point));
        }

        public static bool IsWall(Board board, Point point)
        {
            return WallElements.Contains(board.GetAt(point));
        }
    }
}

/*
 switch (element) 
            {
                case Element.BOMBERMAN:
                case Element.BOMB_BOMBERMAN:
                case Element.BOMB_TIMER_1:
                case Element.BOMB_TIMER_2:
                case Element.BOMB_TIMER_3:
                case Element.BOMB_TIMER_4:
                case Element.BOMB_TIMER_5:
                case Element.BOOM:
                case Element.DeadMeatChopper:
                case Element.DEAD_BOMBERMAN:
                case Element.DESTROYABLE_WALL:
                case Element.DestroyedWall:
                case Element.MEAT_CHOPPER:
                case Element.OTHER_BOMBERMAN:
                case Element.OTHER_BOMB_BOMBERMAN:
                case Element.OTHER_DEAD_BOMBERMAN:
                case Element.Space:
                case Element.WALL:

            }
     */
