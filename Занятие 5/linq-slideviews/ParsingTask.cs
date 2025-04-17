using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace linq_slideviews
{
    public class ParsingTask
    {
        /// <summary>
        /// Парсит записи о слайдах из строк и возвращает словарь, где ключ - ID слайда, значение - объект SlideRecord.
        /// </summary>
        /// <param name="lines">Строки, содержащие записи о слайдах.</param>
        /// <returns>Словарь с записями о слайдах.</returns>
        public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
        {
            return lines
                .Skip(1) // Пропускает заголовок, предполагая, что первая строка не содержит данных о слайдах
                .Where(line => !string.IsNullOrWhiteSpace(line)) // Фильтруем пустые строки
                .Select(line =>
                {
                    var parts = line.Split(';');
                    if (parts.Length != 3) return null; // Если строка содержит не три части, игнорирует её

                    int slideId;
                    if (!int.TryParse(parts[0], out slideId)) return null; // Попытка распарсить ID слайда

                    SlideType slideType;
                    if (!Enum.TryParse(parts[2], true, out slideType)) return null; // Попытка распарсить тип слайда

                    return new SlideRecord(slideId, slideType, parts[1]); // Создание объекта SlideRecord
                })
                .Where(record => record != null) // Удаляет записи, которые не удалось распарсить
                .ToDictionary(record => record.SlideId); // Преобразует в словарь по ID слайда
        }

        /// <summary>
        /// Парсит записи о посещениях слайдов из строк и возвращает список объектов VisitRecord.
        /// </summary>
        /// <param name="lines">Строки, содержащие записи о посещениях слайдов.</param>
        /// <param name="slides">Словарь слайдов для проверки наличия слайда по его ID.</param>
        /// <returns>Список записей о посещениях слайдов.</returns>
        public static IEnumerable<VisitRecord> ParseVisitRecords(
            IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
        {
            return lines
                .Skip(1) // Пропускает заголовок, предполагая, что первая строка не содержит данных о посещениях
                .Where(line => !string.IsNullOrWhiteSpace(line)) // Фильтрует пустые строки
                .Select(line =>
                {
                    var parts = line.Split(';');
                    if (parts.Length != 4) throw new FormatException($"Некорректная строка: {line}"); // Проверяет, что строка содержит четыре части

                    int userId;
                    if (!int.TryParse(parts[0], out userId)) throw new FormatException($"Некорректная строка: {line}"); // Пытается распарсить ID пользователя

                    int slideId;
                    if (!int.TryParse(parts[1], out slideId)) throw new FormatException($"Некорректная строка: {line}"); // Пытается распарсить ID слайда

                    DateTime time;
                    if (!DateTime.TryParse(parts[2] + " " + parts[3], out time)) throw new FormatException($"Некорректная строка: {line}"); // Пытается распарсить дату и время

                    if (!slides.ContainsKey(slideId)) throw new FormatException($"Слайд с ID {slideId} не найден."); // Проверяет, что слайд существует в словаре

                    var slideType = slides[slideId].SlideType; // Получает тип слайда из словаря

                    return new VisitRecord(userId, slideId, time, slideType); // Создаёт объект VisitRecord
                })
                .ToList(); // Преобразует результат в список для немедленной загрузки
        }
    }
}
