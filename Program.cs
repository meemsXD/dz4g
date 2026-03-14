using System;

while (true)
{
    Console.Write("Введите первую строку (или 'exit' для выхода): ");
    string? s1 = Console.ReadLine();

    if (s1 == null) continue;         
    if (s1.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase))
        break;

    Console.Write("Введите вторую строку: ");
    string? s2 = Console.ReadLine();
    if (s2 == null) s2 = "";

    int dist = DamerauLevenshteinDistance(s1, s2);

    Console.WriteLine($"Расстояние Дамерау-Левенштейна между \"{s1}\" и \"{s2}\" = {dist}");
    Console.WriteLine(new string('-', 60));
}

static int DamerauLevenshteinDistance(string a, string b)
{
    if (a == null || b == null) return -1;
    a = a.ToUpperInvariant();
    b = b.ToUpperInvariant();

    int m = a.Length;
    int n = b.Length;

    if (m == 0) return n;
    if (n == 0) return m;

    int[,] matrix = new int[m + 1, n + 1];

    for (int i = 0; i <= m; i++) matrix[i, 0] = i;
    for (int j = 0; j <= n; j++) matrix[0, j] = j;

    for (int i = 1; i <= m; i++)
    {
        for (int j = 1; j <= n; j++)
        {
            int cost = (a[i - 1] == b[j - 1]) ? 0 : 1;

            int del = matrix[i - 1, j] + 1;        
            int ins = matrix[i, j - 1] + 1;        
            int sub = matrix[i - 1, j - 1] + cost;

            int best = Math.Min(Math.Min(del, ins), sub);

            if (i > 1 && j > 1 &&
                a[i - 1] == b[j - 2] &&
                a[i - 2] == b[j - 1])
            {
                best = Math.Min(best, matrix[i - 2, j - 2] + 1);
            }

            matrix[i, j] = best;
        }
    }

    return matrix[m, n];
}