using System.Data.SQLite;

namespace Yaba.Data
{
    public static class DatabaseSchema
    {
        public static void EnsureCreated(SQLiteConnection connection)
        {
            Execute(connection, """
                CREATE TABLE IF NOT EXISTS whisky (
                    id TEXT PRIMARY KEY,
                    type TEXT NOT NULL,
                    name TEXT NOT NULL,
                    strength REAL NOT NULL,
                    size INTEGER NOT NULL,
                    created TEXT NOT NULL,
                    category TEXT NOT NULL,
                    distillery TEXT NOT NULL,
                    bottled INTEGER NOT NULL,
                    age INTEGER NOT NULL,
                    caskType TEXT NOT NULL,
                    bottlingSeries TEXT NOT NULL,
                    naturalColor INTEGER,
                    nonChillFiltered INTEGER
                );
                """);

            Execute(connection, """
                CREATE TABLE IF NOT EXISTS images (
                    id TEXT PRIMARY KEY,
                    spirit_type TEXT NOT NULL,
                    spirit_id TEXT NOT NULL,
                    user_id TEXT,
                    file_name TEXT NOT NULL,
                    sort_order INTEGER NOT NULL,
                    created TEXT NOT NULL
                );
                """);

            Execute(connection, """
                CREATE TABLE IF NOT EXISTS collection_items (
                    user_id TEXT NOT NULL,
                    spirit_type TEXT NOT NULL,
                    spirit_id TEXT NOT NULL,
                    added TEXT NOT NULL,
                    PRIMARY KEY (user_id, spirit_type, spirit_id)
                );
                """);
        }

        private static void Execute(SQLiteConnection connection, string sql)
        {
            using var command = connection.CreateCommand();
            command.CommandText = sql;
            command.ExecuteNonQuery();
        }
    }
}
