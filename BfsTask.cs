//using System.Collections.Generic;

//namespace Dungeon;

//public class BfsTask {
//    public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests) {
//        yield break;
//    }
//}

using System.Collections.Generic;
using System.Drawing;

namespace Dungeon {
    public class BfsTask {
        public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests) { // метод находит все пути от стартовой точки к сундукам
            HashSet<Point> chestsHashSet = new HashSet<Point>(chests); // хэшсет с координатами сундуков (для быстрого поиска)
            Queue<SinglyLinkedList<Point>> queue = new Queue<SinglyLinkedList<Point>>(); // очередь для хранения путей
            HashSet<Point> visited = new HashSet<Point>(); // хэшсет для хранения посещенных точек

            queue.Enqueue(new SinglyLinkedList<Point>(start)); // добавляем стартовую точку в очередь
            visited.Add(start); // помечаем как посещенную

            while (queue.Count != 0) { // пока очередь не пуста
                var currentNode = queue.Dequeue(); // извлекаем текущий путь из очереди
                var currentPoint = currentNode.Value; // получаем текущую точку из пути

                foreach (var neighbor in GetValidNeighbors(map, currentPoint)) { // получаем соседние точки, которые можно посетить
                    if (visited.Contains(neighbor)) continue; // соседняя точка уже посещена, пропускаем ее

                    visited.Add(neighbor); // // помечаем соседнюю точку как посещенную
                    queue.Enqueue(new SinglyLinkedList<Point>(neighbor, currentNode)); // добавляем путь в очередь

                    if (chestsHashSet.Contains(neighbor)) // в соседней точке находится сундук?
                        yield return new SinglyLinkedList<Point>(neighbor, currentNode); // возвращаем путь
                }
            }
        }

        private static IEnumerable<Point> GetValidNeighbors(Map map, Point point) { // метод получения соседних точек, которые можно посетить
            int[,] directions = new int[,] { { 0, -1 }, { -1, 0 }, { 0, 1 }, { 1, 0 } };

            for (int i = 0; i < directions.GetLength(0); i++) { // перебираем все направления
                // вычисляем координаты соседней точки
                var x = point.X + directions[i, 0];
                var y = point.Y + directions[i, 1];

                if (IsValidNeighbor(map, x, y)) { // соседняя точка допустима
                    yield return new Point(x, y); // возвращаем ее
                }
            }
        }

        private static bool IsValidNeighbor(Map map, int x, int y) { // метод проверяет, является ли соседняя точка допустимой
            return x >= 0 && x < map.Dungeon.GetLength(0) && y >= 0 && y < map.Dungeon.GetLength(1) &&
              map.Dungeon[x, y] == MapCell.Empty;
        }
    }
}