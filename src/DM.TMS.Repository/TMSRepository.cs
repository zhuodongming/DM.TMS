using DM.TMS.Domain;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using NPoco;
using System;
using System.Collections.Generic;
using System.Text;

namespace DM.TMS.Repository
{
    public class TMSRepository<T> : BaseRepository<T>
    {
        public TMSRepository()
        {
            db = new Database(DBSettings.TMS, DatabaseType.SqlServer2012, MySqlClientFactory.Instance);
        }
    }
}
