//namespace Dungeon;

//public class DungeonTask {
//    public static MoveDirection[] FindShortestPath(Map map) {
//        return new MoveDirection[0];
//    }
//}


using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Dungeon {
    public class DungeonTask {
        public static MoveDirection[] FindShortestPath(Map dungeonMap) { // кратчайший путь из начальной позиции игрока до выхода из подземелья
            var playerTargetPaths = BfsTask.FindPaths(dungeonMap, dungeonMap.InitialPosition, dungeonMap.Chests.Append(dungeonMap.Exit).ToArray()); // пути от игрока до всех целей
            var directPlayerToExitPath = playerTargetPaths.Where(path => path.Value == dungeonMap.Exit); // путь от игрока до выхода без посещения сундуков

            if (!directPlayerToExitPath.Any()) // не содержит элементов
                return Array.Empty<MoveDirection>(); // пустой массив

            if (directPlayerToExitPath.Any(path => dungeonMap.Chests.Contains(path.Value))) // путь содержит один из сундуков
                return ConvertPathToMoveDirections(directPlayerToExitPath.Last()); // результата выполнения метода

            var pathsFromExitToChests = BfsTask.FindPaths(dungeonMap, dungeonMap.Exit, dungeonMap.Chests); // находим пути от выхода до всех сундуков
            var combinedPaths = playerTargetPaths.Join(pathsFromExitToChests, startPath => startPath.Value, endPath => endPath.Value, (startPath, endPath) => Tuple.Create(startPath, endPath)); // объединяем пути
            var shortestCombinedPath = combinedPaths.OrderBy(path => path.Item1.Length + path.Item2.Length).FirstOrDefault(); // находим кратчайший общий путь

            return shortestCombinedPath == null // массив содержит направления движения по пути
                ? ConvertPathToMoveDirections(directPlayerToExitPath.Last())
                : ConvertPathToMoveDirections(CombinePaths(shortestCombinedPath));
        }

        private static SinglyLinkedList<Point> CombinePaths(Tuple<SinglyLinkedList<Point>, SinglyLinkedList<Point>> pathsTuple) { // соединяет два пути в один, создавая новый экземпляр класса
            var combinedPath = new SinglyLinkedList<Point>(pathsTuple.Item2.Previous.Value, pathsTuple.Item1); // новый экземпляр класса со значением второго и первого пути

            for (var nextPoint = pathsTuple.Item2.Previous.Previous; nextPoint != null; nextPoint = nextPoint.Previous) {
                combinedPath = new SinglyLinkedList<Point>(nextPoint.Value, combinedPath);
            }

            return combinedPath;
        }

        private static MoveDirection DetermineMoveDirection(int dx, int dy) { // определяем направление движения игрока
            return dx != 0 // по разнице между координатами текущего и предыдущего узлов
                ? (dx == 1 ? MoveDirection.Left : MoveDirection.Right)
                : (dy == 1 ? MoveDirection.Up : MoveDirection.Down);
        }

        private static MoveDirection[] ConvertPathToMoveDirections(SinglyLinkedList<Point> path) { // преобразовываем путь в набор направлений движения
            var moveDirections = new List<MoveDirection>(); // список направлений

            var currentNode = path;
            var previousNode = currentNode.Previous;

            while (previousNode != null) {
                var dx = previousNode.Value.X - currentNode.Value.X; // разница координат X предыдущего и текущего узлов
                var dy = previousNode.Value.Y - currentNode.Value.Y; // разница координат Y предыдущего и текущего узлов

                moveDirections.Add(DetermineMoveDirection(dx, dy)); // заполнение списка направлений из разницы координат между текущим и предыдущим узлами пути

                currentNode = previousNode;
                previousNode = previousNode.Previous;
            }

            moveDirections.Reverse(); // изменяет порядок элементов в moveDirections на обратный
            return moveDirections.ToArray(); // список преобразуется в массив и возвращается
        }
    }
}
