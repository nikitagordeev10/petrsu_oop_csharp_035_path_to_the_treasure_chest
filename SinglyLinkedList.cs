// Класс SinglyLinkedList представляет связный список с однонаправленной связью

using System.Collections;
using System.Collections.Generic;

namespace Dungeon {
    public class SinglyLinkedList<T> : IEnumerable<T> {
        public readonly T Value; // Значение текущего узла
        public readonly SinglyLinkedList<T> Previous; // Предыдущий узел
        public readonly int Length; // Длина списка

        public SinglyLinkedList(T value, SinglyLinkedList<T> previous = null) {
            Value = value;
            Previous = previous;
            Length = previous?.Length + 1 ?? 1; // Увеличиваем длину списка на единицу при добавлении нового узла
        }

        public IEnumerator<T> GetEnumerator() {
            yield return Value; // Возвращаем значение текущего узла
            var pathItem = Previous; // Устанавливаем переменную для перебора узлов
            while (pathItem != null) // Пока имеются узлы
            {
                yield return pathItem.Value; // Возвращаем значение узла
                pathItem = pathItem.Previous; // Переходим к предыдущему узлу
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}