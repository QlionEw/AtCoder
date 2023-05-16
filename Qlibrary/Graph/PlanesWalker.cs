using System;
using System.Collections.Generic;

namespace Qlibrary
{
    public class PlanesWalker
    {
        public char Way { get; set; } = '.';
        public char Wall { get; set; } = '#';
        private readonly string[] plane;
        private readonly List<(int X, int Y)> moveDirectionList = new List<(int,int)>();
        private readonly int h;
        private readonly int w;
        
        public PlanesWalker(string[] plane, int h, int w)
        {
            this.plane = plane;
            this.h = h;
            this.w = w;
        }

        public void CommandCrossWalk()
        {
            TellMoveDirection((-1, 0), (1, 0), (0, -1), (0, 1));
        }

        public void TellMoveDirection(params (int,int)[] moveDirection)
        {
            foreach (var direction in moveDirection)
            {
                moveDirectionList.Add(direction);
            }
        }

        public int[,] Distances { get; set; }
        private bool[,] isVisited; 
        public int _01Bfs((int Y, int X) start, (int Y, int X) end, Func<(int Y, int X), (int Y, int X), int> func)
        {
            Deque<(int Y, int X)> deque = new Deque<(int, int)>();
            isVisited = new bool[h, w];
            Distances = new int[h, w];
            Distances.Init(Common.InfinityInt);
            Distances[start.Y, start.X] = 0;
            deque.PushBack((start.Y, start.X));

            while (deque.Length != 0)
            {
                var current = deque.PopFront();
                if (current.X == end.X && current.Y == end.Y)
                {
                    return Distances[current.Y, current.X];
                }
                if (isVisited[current.Y, current.X]) { continue; }
                isVisited[current.Y, current.X] = true;
                foreach ((int y, int x) in moveDirectionList)
                {
                    int nextX = current.X + x;
                    int nextY = current.Y + y;
                    if ( nextX < 0 || w <= nextX ) { continue; }
                    if ( nextY < 0 || h <= nextY ) { continue; }
                    if ( plane[nextY][nextX] == Wall ) { continue; }

                    var next = (nextY, nextX);
                    int _01 = func(current, next);
                    int dd = Distances[current.Y, current.X] + _01; 
                    if (dd < Distances[nextY, nextX])
                    {
                        Distances[nextY, nextX] = dd;
                        if (_01 == 0)
                        {
                            deque.PushFront(next);
                        }
                        else
                        {
                            deque.PushBack(next);
                        }
                    }
                }
            }

            return -1;
        }
        
        public int Dijkstra((int Y, int X) start, (int Y, int X) end, Func<(int Y, int X), (int Y, int X), int> func)
        {
            PriorityQueue<(int totalCost, int Y, int X)> pq = new PriorityQueue<(int, int, int)>(h * w);
            isVisited = new bool[h, w];
            Distances = new int[h, w];
            Distances.Init(Common.InfinityInt);
            Distances[start.Y, start.X] = 0;
            pq.Enqueue((0, start.Y, start.X));

            while (pq.Count != 0)
            {
                var current = pq.Dequeue();
                if (current.X == end.X && current.Y == end.Y)
                {
                    return Distances[current.Y, current.X];
                }
                if (isVisited[current.Y, current.X]) { continue; }
                isVisited[current.Y, current.X] = true;
                foreach ((int y, int x) in moveDirectionList)
                {
                    int nextX = current.X + x;
                    int nextY = current.Y + y;
                    if ( nextX < 0 || w <= nextX ) { continue; }
                    if ( nextY < 0 || h <= nextY ) { continue; }
                    if ( plane[nextY][nextX] == Wall ) { continue; }

                    var next = (nextY, nextX);
                    int cost = func((current.Y, current.X), next);
                    int dd = Distances[current.Y, current.X] + cost; 
                    if (dd < Distances[nextY, nextX])
                    {
                        Distances[nextY, nextX] = dd;
                        pq.Enqueue((dd, nextY, nextX));
                    }
                }
            }

            return -1;
        }

        private int[,,] dirDistances; 
        private bool[,,] isDirVisited; 
        public int Direction01Bfs((int Y, int X) start, (int Y, int X) end, 
            Func<(int Y, int X, int Dir), (int Y, int X, int Dir), int> func)
        {
            var d = moveDirectionList.Count;
            Deque<(int Y, int X, int Dir)> deque = new Deque<(int, int, int)>();
            isDirVisited = new bool[h, w, d];
            dirDistances = new int[h, w, d];
            dirDistances.Init(Common.InfinityInt);
            for (int i = 0; i < d; i++)
            {
                dirDistances[start.Y, start.X, i] = 0;
                deque.PushBack((start.Y, start.X, i));
            }

            while (deque.Length != 0)
            {
                var current = deque.PopFront();
                if (current.X == end.X && current.Y == end.Y)
                {
                    return dirDistances[current.Y, current.X, current.Dir];
                }
                if (isDirVisited[current.Y, current.X, current.Dir]) { continue; }
                isDirVisited[current.Y, current.X, current.Dir] = true;
                for (int i = 0; i < d; i++)
                {
                    var (y, x) = moveDirectionList[i];
                    int nextX = current.X + x;
                    int nextY = current.Y + y;
                    if ( nextX < 0 || w <= nextX ) { continue; }
                    if ( nextY < 0 || h <= nextY ) { continue; }
                    if ( plane[nextY][nextX] == Wall ) { continue; }

                    var next = (nextY, nextX, i);
                    int _01 = func(current, next);
                    int dd = dirDistances[current.Y, current.X, current.Dir] + _01; 
                    if (dd < dirDistances[nextY, nextX, i])
                    {
                        dirDistances[nextY, nextX, i] = dd;
                        if (_01 == 0)
                        {
                            deque.PushFront(next);
                        }
                        else
                        {
                            deque.PushBack(next);
                        }
                    }
                }
            }

            return -1;
        }
    }
}