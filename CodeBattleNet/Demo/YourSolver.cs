/*-
 * #%L
 * Codenjoy - it's a dojo-like platform from developers to developers.
 * %%
 * Copyright (C) 2018 Codenjoy
 * %%
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public
 * License along with this program.  If not, see
 * <http://www.gnu.org/licenses/gpl-3.0.html>.
 * #L%
 */
using Bomberman.Api;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo
{
    /// <summary>
    /// This is BombermanAI client demo.
    /// </summary>
    internal class YourSolver : AbstractSolver
    {
        private Point myPosition;

        private List<Point> myBombs = new List<Point>();

        private Board board;

        private bool setBombBeforeMove;
        private bool setBombAfterMove;
        private Point? trySetBombPoint = null;

        private BlastAnaliser blastAnaliser = new BlastAnaliser();
        private BFS bfs = new BFS();

        private Dictionary<Direction, int> potentalMoves = new Dictionary<Direction, int>(5);

        private Random random = new Random(); 

        public YourSolver(string server)
			: base(server)
		{

		}

		/// <summary>
		/// Calls each move to make decision what to do (next move)
		/// </summary>
		protected override string Get(Board board)
		{
            //��������� ������
            Reset(board);

            //��������� ���� ��������� ����������� �� ��������� � �������� ������
            CheckPotentialMoves();
            var max = potentalMoves.Values.Max();
            Console.WriteLine("MaxPotential: " + max);
            var directionMoves = potentalMoves
                .Where(d => d.Value == max)
                .ToList();
            var directionMove = directionMoves[random.Next(directionMoves.Count)].Key;
            Console.WriteLine("Next position:" + board.GetAt(Helper.GetNewPosition(directionMove, myPosition)));

            //������, ����� �� ������� ����� ����� ������������
            setBombBeforeMove = IsSetBomb(myPosition, directionMove, true);

            //������, ����� �� ������� ����� ����� �����������
            if (setBombBeforeMove)
                setBombAfterMove = false;
            setBombAfterMove = IsSetBomb(myPosition, directionMove, false);

            //��������� �����
            var action = GetAction(directionMove);

            //��������� ������
            trySetBombPoint = setBombBeforeMove
                ? myPosition
                : setBombAfterMove
                    ? Helper.GetNewPosition(directionMove, myPosition)
                    : (Point?) null;

            //���������� �����
            return action;
		}

        private void Reset(Board board)
        {
            this.board = board;
            setBombBeforeMove = true;
            setBombAfterMove = true;

            myPosition = board.GetBomberman();

            potentalMoves = new Dictionary<Direction, int>(5) 
            {
                { Direction.Up, 0 },
                { Direction.Right, 0 },
                { Direction.Down, 0 },
                { Direction.Left, 0 },
                { Direction.Stop, 0 }
            };

            if (myBombs.Any(p => !Helper.IsAnyBomb(board, p)))
                myBombs.Clear();

            if (trySetBombPoint.HasValue && Helper.IsAnyBomb(board, trySetBombPoint.Value))
                myBombs.Add(trySetBombPoint.Value);

            blastAnaliser.Execute(board, myBombs); 
        }

        private void CheckPotentialMoves()
        {
            for (var i = 0; i < potentalMoves.Count; i++)
            {
                var key = potentalMoves.Keys.ToArray()[i];
                potentalMoves[key] = CalcPotentialMove(key);
            }

            //������� ����������� ������� �� ��������� ����
            var path = bfs.Execute(board, myPosition);
            if (path.Count > 1)
            {
                var firstStepPosition = path.ElementAt(1);
                var direction = firstStepPosition.X == myPosition.X
                    ? firstStepPosition.Y > myPosition.Y
                        ? Direction.Up
                        : Direction.Down
                    : firstStepPosition.X > myPosition.X
                        ? Direction.Right
                        : Direction.Left;
                potentalMoves[direction] += 35;
            }
            
        }

        private int CalcPotentialMove(Direction direction)
        {
            var potentialScore = 0;
            var position = Helper.GetNewPosition(direction, myPosition);
            
            //��������� �� ����� �� �� ����
            if (position.IsOutOf(board.BoardSize))
                potentialScore = -10000;

            //��������� �� ������� �� �� ��� ����
            var delta = CheckThisPosition(position);
            potentialScore += delta;

            //��������� �� �������� �� ��� ������ � ��������� ���� (������� ������� ����)
            delta = CheckBoom(position);
            potentialScore += delta;

            //��������� �� ������ �� ��� ������ ������ ������ ��� ��������
            delta = CheckAlienMoves(position);
            potentialScore += delta;            

            return potentialScore;
        }

        private int CheckThisPosition(Point position)
        {
            var element = board.GetAt(position);
            switch (element) 
            {                
                case Element.WALL:
                case Element.BOMB_TIMER_1:
                case Element.BOMB_TIMER_2:
                case Element.BOMB_TIMER_3:
                case Element.BOMB_TIMER_4:
                case Element.BOMB_TIMER_5:
                case Element.DESTROYABLE_WALL:
                case Element.OTHER_BOMB_BOMBERMAN:
                    return -10000; //������ ������������ ��� 100% ������

                case Element.MEAT_CHOPPER:
                    return -800; //����� ������� �����������

                case Element.OTHER_BOMBERMAN:
                    return -200; //���� ����������� ��� ����������� �� ������

                case Element.Space:
                    return 0; //��������� �������

                case Element.BOMB_BOMBERMAN:
                case Element.BOMBERMAN:
                    return -5; //������ �� ����� �� ���������

                default:
                case Element.BOOM:
                case Element.DeadMeatChopper:
                case Element.DEAD_BOMBERMAN:
                case Element.DestroyedWall:
                case Element.OTHER_DEAD_BOMBERMAN:
                    return -20; //���� ����������� ��� ����������� �� ������
            }
        }

        private int CheckBoom(Point position)
        {
            if (blastAnaliser.BoomNextTickAll.Contains(position) || blastAnaliser.BoomNextTickMy.Contains(position))
                return -10000;
            if (blastAnaliser.BoomSecondTickAll.Contains(position) || blastAnaliser.BoomSecondTickMy.Contains(position))
                return -150;
            if (blastAnaliser.BoomThirdTickAll.Contains(position) || blastAnaliser.BoomThirdTickMy.Contains(position))
                return -50;
            if (blastAnaliser.BoomFourthTickAll.Contains(position) || blastAnaliser.BoomFourthTickMy.Contains(position))
                return -20;
            if (blastAnaliser.BoomFifthTickAll.Contains(position) || blastAnaliser.BoomFifthTickMy.Contains(position))
                return -5;
            return 0;
        }

        private int CheckAlienMoves(Point position)
        {
            var delta = 0;
            foreach (var direction in Helper.AllDirections)
            {
                var newPosition = Helper.GetNewPosition(direction, position);
                if (newPosition.IsOutOf(board.BoardSize))
                    continue;
                var element = board.GetAt(position);
                switch(element)
                {
                    case Element.MEAT_CHOPPER:
                        delta -= 700; //����� ������
                        break;
                    case Element.OTHER_BOMBERMAN:
                    case Element.OTHER_BOMB_BOMBERMAN:
                        delta -= 100; //����������� ����� �� ���������
                        break;
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
                    case Element.OTHER_DEAD_BOMBERMAN:
                    case Element.Space:
                    case Element.WALL:
                        continue; //� ��� ������ �� �����
                }
            }
            return delta;
        }

        private string GetAction(Direction direction)
        {
            return setBombBeforeMove 
                ? $"ACT,{ direction.ToString().ToUpper() }"
                : setBombAfterMove
                    ? $"{ direction.ToString().ToUpper()},ACT"
                    : direction.ToString().ToUpper();
        }

        private bool IsSetBomb(Point currPosition, Direction directionMove, bool isBeforeMove)
        {
            var nextPosition = Helper.GetNewPosition(directionMove, myPosition);
            return isBeforeMove
                ? IsSetBombBefore(currPosition, nextPosition)
                : IsSetBombAfter(currPosition, nextPosition);
        }

        private bool IsSetBombBefore(Point currPosition, Point nextPosition)
        {
            //�� ������, ���� ����� �� ����� �� ����� �����
            if (currPosition == nextPosition && board.GetAt(currPosition) == Element.BOMB_BOMBERMAN)
                return false;

            //�� ������ ���� ��� ������� �������� � ��������� ���� ��� ����� ���
            if (blastAnaliser.BoomNextTickAll.Contains(currPosition) || blastAnaliser.BoomNextTickMy.Contains(currPosition) ||
                blastAnaliser.BoomSecondTickAll.Contains(currPosition) || blastAnaliser.BoomSecondTickMy.Contains(currPosition))
                return false;

            //������ ���� ��� ������� ������� �������� ����� ������
            if (blastAnaliser.BoomThirdTickMy.Contains(currPosition) || blastAnaliser.BoomFourthTickMy.Contains(currPosition) ||
                blastAnaliser.BoomFifthTickMy.Contains(currPosition))
                return true;

            //�� ������ ���� ��� ������� �������� ����� ������
            if (blastAnaliser.BoomThirdTickAll.Contains(currPosition) || blastAnaliser.BoomFourthTickAll.Contains(currPosition) ||
                blastAnaliser.BoomFifthTickAll.Contains(currPosition))
                return true;

            //������
            return true;
        }

        private bool IsSetBombAfter(Point currPosition, Point nextPosition)
        {
            //�� ������ ���� ��� ������� �������� � ��������� ���� ��� ����� ���
            if (blastAnaliser.BoomNextTickAll.Contains(nextPosition) || blastAnaliser.BoomNextTickMy.Contains(nextPosition) ||
                blastAnaliser.BoomSecondTickAll.Contains(nextPosition) || blastAnaliser.BoomSecondTickMy.Contains(nextPosition))
                return false;

            //������ ���� ��� ������� ������� �������� ����� ������
            if (blastAnaliser.BoomThirdTickMy.Contains(nextPosition) || blastAnaliser.BoomFourthTickMy.Contains(nextPosition) ||
                blastAnaliser.BoomFifthTickMy.Contains(nextPosition))
                return true;

            //�� ������ ���� ��� ������� �������� ����� ������
            if (blastAnaliser.BoomThirdTickAll.Contains(nextPosition) || blastAnaliser.BoomFourthTickAll.Contains(nextPosition) ||
                blastAnaliser.BoomFifthTickAll.Contains(nextPosition))
                return true;

            //������
            return true;
        }
    }
}

