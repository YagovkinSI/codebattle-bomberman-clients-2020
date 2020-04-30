using Bomberman.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public class BFS
    {
        List<Point> checkedPoint = new List<Point>();
        Queue<Queue<Point>> queuePaths = new Queue<Queue<Point>>();

        public BFS()
        {

        }

        public Queue<Point> Execute(Board board, Point startPoint)
        {
            checkedPoint.Clear();

            var path = new Queue<Point>();
            path.Enqueue(startPoint);

            queuePaths = new Queue<Queue<Point>>();
            queuePaths.Enqueue(path);

            checkedPoint.Add(startPoint);

            while (true)
            {
                var curPath = queuePaths.Dequeue();
                var position = curPath.ElementAt(curPath.Count - 1);
                foreach(var direction in Helper.AllDirections)
                {
                    var newPosition = Helper.GetNewPosition(direction, position);
                    if (checkedPoint.Contains(newPosition))
                        continue;
                    if (board.GetAt(newPosition) == Element.Space)
                    {
                        var newPath = new Queue<Point>();
                        foreach (var point in curPath)                        
                            newPath.Enqueue(point);
                        newPath.Enqueue(newPosition);
                        checkedPoint.Add(newPosition);
                        queuePaths.Enqueue(newPath);
                    }
                    if (board.GetAt(newPosition) == Element.OTHER_BOMBERMAN ||
                        board.GetAt(newPosition) == Element.OTHER_BOMB_BOMBERMAN ||
                        board.GetAt(newPosition) == Element.MEAT_CHOPPER)
                    {
                        curPath.Enqueue(newPosition);
                        return curPath;
                    }
                }
                if (queuePaths.Count == 0)
                    return curPath;
            }
        }
    }
}
