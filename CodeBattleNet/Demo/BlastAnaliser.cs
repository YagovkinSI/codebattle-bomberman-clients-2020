using Bomberman.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public class BlastAnaliser
    {
        private Board board;
        private List<Point> myBombs;

        public List<Point> BoomNextTickMy = new List<Point>();
        public List<Point> BoomNextTickAll = new List<Point>();

        public List<Point> BoomSecondTickMy = new List<Point>();
        public List<Point> BoomSecondTickAll = new List<Point>();

        public List<Point> BoomThirdTickMy = new List<Point>();
        public List<Point> BoomThirdTickAll = new List<Point>();

        public List<Point> BoomFourthTickMy = new List<Point>();
        public List<Point> BoomFourthTickAll = new List<Point>();

        public List<Point> BoomFifthTickMy = new List<Point>();
        public List<Point> BoomFifthTickAll = new List<Point>();

        public BlastAnaliser()
        {
            
        }

        public void Execute(Board board, List<Point> myBombs)
        {
            this.board = board;
            this.myBombs = myBombs;

            BoomNextTickMy.Clear();
            BoomNextTickAll.Clear();
            BoomSecondTickMy.Clear();
            BoomSecondTickAll.Clear();
            BoomThirdTickMy.Clear();
            BoomThirdTickAll.Clear();
            BoomFourthTickMy.Clear();
            BoomFourthTickAll.Clear();
            BoomFifthTickMy.Clear();
            BoomFifthTickAll.Clear();

            //проверяем свои бомбы которые взорвуться на следующем ходу
            var bombs = board
                .Get(Element.BOMB_TIMER_1)
                .Where(myBombs.Contains);
            foreach (var bombPoint in bombs)
                FillBlast(bombPoint, ref BoomNextTickMy);

            //проверяем чужие бомбы которые взорвуться на следующем ходу
            bombs = board
                .Get(Element.BOMB_TIMER_1)
                .Where(p => !myBombs.Contains(p));
            foreach (var bombPoint in bombs)
                FillBlast(bombPoint, ref BoomNextTickAll);


            //проверяем свои бомбы которые взорвуться через ход
            bombs = board
                .Get(Element.BOMB_TIMER_2)
                .Where(myBombs.Contains);
            foreach (var bombPoint in bombs)
                FillBlast(bombPoint, ref BoomSecondTickMy);


            //проверяем чужие бомбы которые взорвуться через ход
            bombs = board
                .Get(Element.BOMB_TIMER_2)
                .Where(p => !myBombs.Contains(p));
            foreach (var bombPoint in bombs)
                FillBlast(bombPoint, ref BoomSecondTickAll);

            //проверяем свои бомбы которые взорвуться через ход
            bombs = board
                .Get(Element.BOMB_TIMER_3)
                .Where(myBombs.Contains);
            foreach (var bombPoint in bombs)
                FillBlast(bombPoint, ref BoomThirdTickMy);


            //проверяем чужие бомбы которые взорвуться через ход
            bombs = board
                .Get(Element.BOMB_TIMER_3)
                .Where(p => !myBombs.Contains(p));
            foreach (var bombPoint in bombs)
                FillBlast(bombPoint, ref BoomThirdTickAll);

            //проверяем свои бомбы которые взорвуться через ход
            bombs = board
                .Get(Element.BOMB_TIMER_4)
                .Where(myBombs.Contains);
            foreach (var bombPoint in bombs)
                FillBlast(bombPoint, ref BoomFourthTickMy);


            //проверяем чужие бомбы которые взорвуться через ход
            bombs = board
                .Get(Element.BOMB_TIMER_4)
                .Where(p => !myBombs.Contains(p));
            foreach (var bombPoint in bombs)
                FillBlast(bombPoint, ref BoomFourthTickAll);

            //проверяем свои бомбы которые взорвуться через ход
            bombs = board
                .Get(Element.BOMB_TIMER_5)
                .Where(myBombs.Contains);
            foreach (var bombPoint in bombs)
                FillBlast(bombPoint, ref BoomFifthTickMy);


            //проверяем чужие бомбы которые взорвуться через ход
            bombs = board
                .Get(Element.BOMB_TIMER_5)
                .Where(p => !myBombs.Contains(p));
            foreach (var bombPoint in bombs)
                FillBlast(bombPoint, ref BoomFifthTickAll);
        }

        private bool IsAnyListContains(Point point)
        {
            return BoomNextTickMy.Contains(point) ||
                BoomNextTickAll.Contains(point) ||
                BoomSecondTickMy.Contains(point) ||
                BoomSecondTickAll.Contains(point) ||
                BoomThirdTickMy.Contains(point) ||
                BoomThirdTickAll.Contains(point) ||
                BoomFourthTickMy.Contains(point) ||
                BoomFourthTickAll.Contains(point) ||
                BoomFifthTickMy.Contains(point) ||
                BoomFifthTickAll.Contains(point);
        }

        private void FillBlast (Point bombPoint, ref List<Point> list)
        {
            if (!IsAnyListContains(bombPoint))
                list.Add(bombPoint);
            else return;
            foreach (var direction in Helper.AllDirections)
            {                
                var currPoint = bombPoint;
                var range = 1;
                while (range < 4)
                {
                    var blastPoint = Helper.GetNewPosition(direction, currPoint);
                    if (blastPoint.IsOutOf(board.BoardSize))
                        break;
                    var element = board.GetAt(blastPoint);
                    if (Helper.BombElements.Contains(element))
                    {
                        if (IsAnyListContains(blastPoint))
                            break;
                        FillBlast(blastPoint, ref list);
                    }
                    if (Helper.WallElements.Contains(element))
                    {
                        if (!IsAnyListContains(blastPoint))
                            list.Add(blastPoint);
                        break;
                    }
                    if (!IsAnyListContains(blastPoint))
                        list.Add(blastPoint);
                    currPoint = blastPoint;
                    range++;
                }
            }
        }
    }
}
