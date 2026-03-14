using System;

while (true)
{
    Console.Write("Введите первую строку (или 'exit' для выхода): ");
    string? s1 = Console.ReadLine();

    if (s1 == null) continue;                 // на всякий случай
    if (s1.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase))
        break;

    Console.Write("Введите вторую строку: ");
    string? s2 = Console.ReadLine();
    if (s2 == null) s2 = "";

    int dist = DamerauLevenshteinDistance(s1, s2);

    Console.WriteLine($"Расстояние Дамерау-Левенштейна между \"{s1}\" и \"{s2}\" = {dist}");
    Console.WriteLine(new string('-', 60));
}

/// <summary>
/// Расстояние Дамерау–Левенштейна (вариант Вагнера–Фишера с транспозицией соседних символов).
/// Разрешённые операции (стоимость 1): вставка, удаление, замена, транспозиция соседних.
/// </summary>
static int DamerauLevenshteinDistance(string a, string b)
{
    if (a == null || b == null) return -1;

    // Можно сделать сравнение нечувствительным к регистру:
    a = a.ToUpperInvariant();
    b = b.ToUpperInvariant();

    int m = a.Length;
    int n = b.Length;

    // Если одна строка пустая — расстояние равно длине другой (вставки/удаления).
    if (m == 0) return n;
    if (n == 0) return m;

    // matrix[i, j] = расстояние между префиксами a[0..i-1] и b[0..j-1]
    int[,] matrix = new int[m + 1, n + 1];

    // Инициализация: расстояния до пустой строки
    for (int i = 0; i <= m; i++) matrix[i, 0] = i;
    for (int j = 0; j <= n; j++) matrix[0, j] = j;

    for (int i = 1; i <= m; i++)
    {
        for (int j = 1; j <= n; j++)
        {
            int cost = (a[i - 1] == b[j - 1]) ? 0 : 1;

            int del = matrix[i - 1, j] + 1;        // удалить a[i-1]
            int ins = matrix[i, j - 1] + 1;        // вставить b[j-1]
            int sub = matrix[i - 1, j - 1] + cost; // заменить a[i-1] -> b[j-1] (или совпадение)

            int best = Math.Min(Math.Min(del, ins), sub);

            // Поправка Дамерау: транспозиция соседних символов
            // Если a[i-1] == b[j-2] и a[i-2] == b[j-1], то можно "поменять местами" за 1 шаг
            if (i > 1 && j > 1 &&
                a[i - 1] == b[j - 2] &&
                a[i - 2] == b[j - 1])
            {
                // В классическом "оптимальном выравнивании" транспозиция стоит 1.
                // Обычно добавляют +1. В твоём тексте формула дана как + m(...),
                // но логика "транспозиция = 1" — это +1.
                best = Math.Min(best, matrix[i - 2, j - 2] + 1);
            }

            matrix[i, j] = best;
        }
    }

    return matrix[m, n];
}