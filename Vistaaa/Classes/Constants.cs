using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistaaa
{
    public static class Constants
    {
        public const string DatabaseFilename = "test.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            SQLite.SQLiteOpenFlags.ReadWrite |
            SQLite.SQLiteOpenFlags.Create |
            SQLite.SQLiteOpenFlags.SharedCache;
        public static string DatabasePath =>
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DatabaseFilename);
    }
}
