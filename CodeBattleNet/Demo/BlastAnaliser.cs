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

        public Tuple<Element, bool, List<Point>>[] BlastAnaliserDict;

        public BlastAnaliser()
        {
            BlastAnaliserDict = new[]
            {
                Tuple.Create(Element.BOMB_TIMER_1, true, BoomNextTickMy),
                Tuple.Create(Element.BOMB_TIMER_1, false, BoomNextTickAll),
                Tuple.Create(Element.BOMB_TIMER_2, true, BoomSecondTickMy),
                Tuple.Create(Element.BOMB_TIMER_2, false, BoomSecondTickAll),
                Tuple.Create(Element.BOMB_TIMER_3, true, BoomThirdTickMy),
                Tuple.Create(Element.BOMB_TIMER_3, false, BoomThirdTickAll),
                Tuple.Create(Element.BOMB_TIMER_4, true, BoomFourthTickMy),
                Tuple.Create(Element.BOMB_TIMER_4, false, BoomFourthTickAll),
                Tuple.Create(Element.BOMB_TIMER_5, true, BoomFifthTickMy),
                Tuple.Create(Element.BOMB_TIMER_5, false, BoomFifthTickAll)
            };

        }

        public void Execute(Board board, List<Point> myBombs)
        {
            this.board = board;

            foreach (var tuple in BlastAnaliserDict)
                tuple.Item3.Clear();

            foreach (var tuple in BlastAnaliserDict)
            {
                var bombs = board
                .Get(tuple.Item1)
                .Where(p => myBombs.Contains(p) == tuple.Item2);
                foreach (var bombPoint in bombs)
                    FillBlast(bombPoint, tuple);
            }
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

        private void FillBlast (Point bombPoint, Tuple<Element, bool, List<Point>> tuple)
        {
            if (!IsAnyListContains(bombPoint))
                tuple.Item3.Add(bombPoint);
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
                        FillBlast(blastPoint, tuple);
                    }
                    if (Helper.WallElements.Contains(element))
                    {
                        if (!IsAnyListContains(blastPoint))
                            tuple.Item3.Add(blastPoint);
                        break;
                    }
                    if (!IsAnyListContains(blastPoint))
                        tuple.Item3.Add(blastPoint);
                    currPoint = blastPoint;
                    range++;
                }
            }
        }
    }
}
