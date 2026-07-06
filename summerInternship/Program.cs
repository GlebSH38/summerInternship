using Dapper;
using Npgsql;
using KingMigrations.PostgreSql;
using KingMigrations.MigrationParsers;
using KingMigrations.MigrationSources;
using summerIntership;
using System.Data;

NpgsqlConnection connection = null;

while (connection == null || connection.State != ConnectionState.Open)
{
    Console.WriteLine("Введите параметры подключение к базе данных");
    string connectionString =
            AnswerString("Host") +
            AnswerString("Port") +
            AnswerString("Database") +
            AnswerString("Username") +
            AnswerString("Password");

    try
    {
        connection = new NpgsqlConnection(connectionString);
        connection.Open();
        Console.WriteLine("Подключение прошло успешно");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка подключения: {ex.Message}");
        Console.WriteLine("Попробуйте снова");
    }
}

string answer = null;

while (!(answer == "1" || answer == "2"))
{
    Console.WriteLine("Работать с существующими таблицами и данными или загрузить тестовые (1-существующие;2-тестовые):");
    answer = Console.ReadLine();

    if (answer == "2")
    {
        try
        {
            Console.WriteLine("Применение миграций");

            var migrationApplier = new PostgreSqlMigrationApplier();
            var migrationSource = new DirectoryMigrationSource("Migrations");
            migrationSource.AddParser(".sql", new SemicolonDelimitedMigrationParser());

            await migrationApplier.ApplyMigrationsAsync(connection, migrationSource);

            Console.WriteLine("Миграции успешно применены!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при применении миграций: {ex.Message}");
        }
    }
    else if (answer != "1" && answer != "2")
    {
        Console.WriteLine("Неверный ввод");
    }
}

string sql1 = @"
            SELECT 
                u.id,
                u.username,
                ARRAY_AGG(r.name) AS role_names
            FROM users u
            LEFT JOIN user_roles ur ON u.id = ur.user_id
            LEFT JOIN roles r ON ur.role_id = r.id
            GROUP BY u.id, u.username
            ORDER BY u.id";

string sql2 = @"
            SELECT 
                r.id,
                r.name,
                COUNT(ur.user_id) AS user_count
            FROM roles r
            LEFT JOIN user_roles ur ON r.id = ur.role_id
            GROUP BY r.id
            ORDER BY user_count DESC, r.name";

answer = null;

while (answer != "0")
{
    Console.WriteLine($"Какой запрос вывести (\n1-пользователи и все роли каждого;\n2-роли и количество пользователей с данной ролью;\n0-выйти из программы):");
    answer = Console.ReadLine();

    if (answer == "1")
    {
        var users = connection.Query<UserWithRoles>(sql1);

        Console.WriteLine("Пользователи и их роли:");
        Console.WriteLine(new string('-', 60));

        foreach (var user in users)
        {
            var roles = string.Join(", ", user.role_names);

            Console.WriteLine($"{user.username}");
            Console.WriteLine($"Роли: {roles}");
            Console.WriteLine();
        }
    }
    else if (answer == "2")
    {
        var roleStats = connection.Query<RoleStatistic>(sql2);

        Console.WriteLine("Статистика по ролям:");
        Console.WriteLine(new string('-', 50));
        Console.WriteLine($"{"Роль",-20} {"Пользователей",-15}");
        Console.WriteLine(new string('-', 50));

        foreach (var role in roleStats)
        {
            Console.WriteLine($"{role.name,-20} {role.user_count,-15}");
        }
    }
    else if (answer == "0")
    {
        Console.WriteLine("Конец программы");
    }
    else
    {
        Console.WriteLine("Неверный ввод");
    }
}

connection?.Close();
connection?.Dispose();

string AnswerString(string parameter)
{
    Console.WriteLine($"введите {parameter}:");
    string answer = Console.ReadLine();
    return $"{parameter}={answer};";
}