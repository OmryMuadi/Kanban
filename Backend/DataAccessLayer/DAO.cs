using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public abstract class DAO
    {
        protected readonly string connectionString;
        protected readonly string tableName;
        public DAO(string tableName)
        {
            //string path = @"C:\Users\omrym\source\repos\2022-2023-2023-2024-18\kanban.db";
            //string path = @"C:\Users\adamr\source\repos\BGU-SE-Intro\2022-2023-2023-2024-18\kanban.db";
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this.connectionString = $"Data Source={path}; Version=3;";
            this.tableName = tableName;
        }
        protected abstract DTO ConvertReaderToObject(SQLiteDataReader reader);
    }
}
