using DM.TMS.Domain;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using NPoco;
using System;
using System.Collections.Generic;
using System.Text;

namespace DM.TMS.Repository
{
    public abstract class TMSRepository<T> : BaseRepository<T>
    {
        public TMSRepository()
        {
            db = new Database(DBSettings.TMS, DatabaseType.MySQL, MySqlClientFactory.Instance);
        }
    }
}
