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
        public TMSRepository(IOptions<DBSettings> dbSettings)
        {
            db = new Database(dbSettings.Value.TMS, DatabaseType.MySQL, MySqlClientFactory.Instance);
        }
    }
}
