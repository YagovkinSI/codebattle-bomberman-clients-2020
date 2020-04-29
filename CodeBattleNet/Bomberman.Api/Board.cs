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
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bomberman.Api
{
	public class Board
	{
		#region Fields

		#region Private

		private readonly string _boardString;
		private readonly LengthToXY _lengthXy;
		private const int SIZE_OF_BLAST = 3;

		#endregion


		#endregion

		#region Methods

		#region Private

		private string BoardAsString()
		{
			string result = "";
			for (int i = 0; i < BoardSize; i++)
			{
				result += _boardString.Substring(i * BoardSize, BoardSize);
				result += "\n";
			}
			return result;
		}

		private string ListToString(List<Point> list)
		{
			return string.Join(",", list.ToArray());
		}

		private void IsBlastCanBeOnThisPoint(Point blastPoint, ISet<Point> result)
		{
			if (!blastPoint.IsOutOf(BoardSize) && !IsAt(blastPoint, Element.WALL))
				result.Add(blastPoint);
		}

		#endregion


		#region Public

		#region .ctor

		public Board(String boardString)
		{
			_boardString = boardString.Replace("\n", "");
			BoardSize = (int)Math.Sqrt(_boardString.Length);
			_lengthXy = new LengthToXY(BoardSize);
		}

		#endregion


		/// <summary>
		/// ���������� ������ ����� (��� ��� � ��� ����� - �������, �� ��� ���������� ����� ��� �������)
		/// </summary>
		public int BoardSize { get; }

		/// <summary>
		/// �������� ������� ����� ������ ������� ��� ����������� �� ��� ���������� ���������
		/// </summary>
		/// <returns>������� ������ �������, ���� �� ������������ �� ����, ���� ��� ���������� �����, ��� ��� ��� - �� ����� ��������� ��������</returns>
		public Point GetBomberman()
		{
			return Get(Element.BOMBERMAN)
				.Concat(Get(Element.BOMB_BOMBERMAN))
				.Concat(Get(Element.DEAD_BOMBERMAN))
				.FirstOrDefault();
		}

		/// <summary>
		/// �������� ������� ���� ��������� �������� ��� ����������� �� �� ���������� ���������
		/// </summary>
		/// <returns>������ � ��������� ��������� �������� ��� �������� ���� ����� ������� �����������</returns>
		public List<Point> GetOtherBombermans()
		{
			return Get(Element.OTHER_BOMBERMAN)
				.Concat(Get(Element.OTHER_BOMB_BOMBERMAN))
				.Concat(Get(Element.OTHER_DEAD_BOMBERMAN))
				.ToList();
		}

		/// <summary>
		/// �������� �� ���������������� ������ �������
		/// </summary>
		public bool IsMyBombermanDead => _boardString.Contains((char)Element.DEAD_BOMBERMAN);

		/// <summary>
		/// �������� �������, ������� ��������� �� ����������� �����������.
		/// ���� ���������� ��������� �� ��������� ���� - �� ����� ���������� ������������ �����.
		/// </summary>
		/// <param name="point">�������, �� ������� ���������� �������� �������</param>
		/// <returns>��� ��������</returns>
		public Element GetAt(Point point)
		{
			if (point.IsOutOf(BoardSize))
			{
				return Element.WALL;
			}
			return (Element)_boardString[_lengthXy.GetLength(point.X, point.Y)];
		}

		/// <summary>
		/// �������� �� ��, ��������� � ����������� ����� ������� ����������� ����
		/// </summary>
		/// <param name="point">�������, �� ������� ���������� ��������� ������� ��������</param>
		/// <param name="element">��� ��������</param>
		/// <returns></returns>
		public bool IsAt(Point point, Element element)
		{
			if (point.IsOutOf(BoardSize))
			{
				return false;
			}

			return GetAt(point) == element;
		}

		/// <summary>
		/// �������� �� ��, ��������� � ����������� ����� ���� �� ��������� ����������� ����
		/// </summary>
		/// <param name="point">�������, �� ������� ���������� ��������� ������� ������ �� ��������� ����������� ����</param>
		/// <param name="elements">��� ������� ���������</param>
		/// <returns></returns>
		public bool IsAt(Point point, Element[] elements)
		{
			var elementOnPoint = GetAt(point);

			return elements.Any(elem => elem.Equals(elementOnPoint));
		}

		/// <summary>
		/// �������� ���������� ���� ����� ���������� �� ����
		/// </summary>
		/// <returns></returns>
		public List<Point> GetMeatChoppers()
		{
			return Get(Element.MEAT_CHOPPER);
		}

		/// <summary>
		/// �������� ���������� ���� ��������� ������������ ����
		/// </summary>
		/// <param name="element">������� ��� ���������</param>
		/// <returns></returns>
		public List<Point> Get(Element element)
		{
			List<Point> result = new List<Point>();

			for (int i = 0; i < BoardSize * BoardSize; i++)
			{
				Point pt = _lengthXy.GetXY(i);

				if (IsAt(pt, element))
				{
					result.Add(pt);
				}
			}

			return result;
		}

		/// <summary>
		/// �������� ���������� ���� ������������ ����
		/// </summary>
		/// <returns></returns>
		public List<Point> GetWalls()
		{
			return Get(Element.WALL);
		}

		/// <summary>
		/// �������� ���������� ���� ����������� ����
		/// </summary>
		/// <returns></returns>
		public List<Point> GetDestroyableWalls()
		{
			return Get(Element.DESTROYABLE_WALL);
		}

		/// <summary>
		/// �������� ���������� ���� ����, ��� ����������� �� ����, ��� ����� ��� ����� �������� � ���� �� ��� ���� ������
		/// </summary>
		/// <returns></returns>
		public List<Point> GetBombs()
		{
			return Get(Element.BOMB_TIMER_1)
				.Concat(Get(Element.BOMB_TIMER_2))
				.Concat(Get(Element.BOMB_TIMER_3))
				.Concat(Get(Element.BOMB_TIMER_4))
				.Concat(Get(Element.BOMB_TIMER_5))
				.Concat(Get(Element.BOMB_BOMBERMAN))
				.Concat(Get(Element.OTHER_BOMB_BOMBERMAN))
				.ToList();
		}

		/// <summary>
		/// �������� ���������� ���� ������� �� ������� ����
		/// </summary>
		/// <returns></returns>
		public List<Point> GetBlasts()
		{
			return Get(Element.BOOM);
		}

		/// <summary>
		/// �������� ���������� ���� ������� �������, ��� ����������� �� ����, ����� �� ����������.
		/// ��� ���� �� ����������� ������������� ���������� ��������������� ������ �������������
		/// </summary>
		/// <returns></returns>
		public List<Point> GetFutureBlasts()
		{
			var bombs = GetBombs();
			var result = new HashSet<Point>();

			foreach (var bomb in bombs)
			{
				result.Add(bomb);

				for (var i = 1; i <= SIZE_OF_BLAST; i++)
				{
					IsBlastCanBeOnThisPoint(bomb.ShiftLeft(i), result);
					IsBlastCanBeOnThisPoint(bomb.ShiftRight(i), result);
					IsBlastCanBeOnThisPoint(bomb.ShiftTop(i), result);
					IsBlastCanBeOnThisPoint(bomb.ShiftBottom(i), result);
				}
			}

			return result.ToList();
		}

		/// <summary>
		/// �������� �� ��, � ��� �� ����� � ������ ��������� ������������ ����
		/// </summary>
		/// <param name="point">�����, ����� � ������� ���������� �������� ��������</param>
		/// <param name="element">��� �������� ��������</param>
		/// <returns></returns>
		public bool IsNear(Point point, Element element)
		{
			if (point.IsOutOf(BoardSize))
				return false;

			return IsAt(point.ShiftLeft(), element) ||
			       IsAt(point.ShiftRight(), element) ||
			       IsAt(point.ShiftTop(), element) ||
			       IsAt(point.ShiftBottom(), element);
		}

		/// <summary>
		/// �������� �� ��, � ��������� �� �� ����������� ����������� ������������ �����������
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		public bool IsBarrierAt(Point point)
		{
			return GetBarriers().Contains(point);
		}

		/// <summary>
		/// ��������� ���������� ����������� ����� � ����������� ������ ��������� ������������ ����.
		/// ����������� ������ ������ ��������������� ������, �����, ������ � �����, �� ���� ����������� � ��������������� �� ��������.
		/// </summary>
		/// <param name="point">�����, ����� � ������� ���������� ��������� ���������� ���������.</param>
		/// <param name="element">��� �������� ��������.</param>
		/// <returns></returns>
		public int CountNear(Point point, Element element)
		{
			if (point.IsOutOf(BoardSize))
				return 0;

			int count = 0;
			if (IsAt(point.ShiftLeft(), element)) count++;
			if (IsAt(point.ShiftRight(), element)) count++;
			if (IsAt(point.ShiftTop(), element)) count++;
			if (IsAt(point.ShiftBottom(), element)) count++;
			return count;
		}

		/// <summary>
		/// �������� ���������� ���� ������������ ����������� �� ����
		/// </summary>
		/// <returns></returns>
		public List<Point> GetBarriers()
		{
			return GetMeatChoppers()
				.Concat(GetWalls())
				.Concat(GetBombs())
				.Concat(GetDestroyableWalls())
				.Concat(GetOtherBombermans())
				.Distinct()
				.ToList();
		}

		/// <summary>
		/// �������� ����������� ������������� ������� ����� � � ���������
		/// </summary>
		public override string ToString()
		{
			return string.Format("{0}\n" +
								 "Bomberman at: {1}\n" +
								 "Other bombermans at: {2}\n" +
								 "Meat choppers at: {3}\n" +
								 "Destroyable walls at: {4}\n" +
								 "Bombs at: {5}\n" +
								 "Blasts: {6}\n" +
								 "Expected blasts at: {7}",
				BoardAsString(),
				GetBomberman(),
				ListToString(GetOtherBombermans()),
				ListToString(GetMeatChoppers()),
				ListToString(GetDestroyableWalls()),
				ListToString(GetBombs()),
				ListToString(GetBlasts()),
				ListToString(GetFutureBlasts()));
		}

		#endregion

		#endregion
	}
}
