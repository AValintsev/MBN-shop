using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Nop.Data;
using Nop.Core;
using System.Data.Entity.Infrastructure;
using Nop.Plugin.Misc.SMS.Domain;

namespace Nop.Plugin.Misc.SMS.Data
{
    public class SMSObjectContext : DbContext, IDbContext
    {
        public bool ProxyCreationEnabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool AutoDetectChangesEnabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public SMSObjectContext(string nameOrConnectionString) : base(nameOrConnectionString) { }

        #region Implementation of IDbContext

        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new SMSMap());
            modelBuilder.Configurations.Add(new SMSMessageMap());
            modelBuilder.Configurations.Add(new SMSProviderMap());

            base.OnModelCreating(modelBuilder);
        }

        public string CreateDatabaseInstallationScript()
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateDatabaseScript();
        }

        public void Install()
        {
            //It's required to set initializer to null (for SQL Server Compact).
            //otherwise, you'll get something like "The model backing the 'your context name' context has changed since the database was created. Consider using Code First Migrations to update the database"
            Database.SetInitializer<SMSObjectContext>(null);

            Database.ExecuteSqlCommand(CreateDatabaseInstallationScript());
            SaveChanges();
        }

        internal void Set(Domain.SMS sMS)
        {
            throw new NotImplementedException();
        }

        public void Uninstall()
        {
            var dbScriptforSMS = "DROP TABLE SMS";
            var dbScriptforMessage = "DROP TABLE SMSMessage";
            var dbScriptforProvider = "DROP TABLE SMSProvider";

            Database.ExecuteSqlCommand(dbScriptforSMS);
            Database.ExecuteSqlCommand(dbScriptforMessage);
            Database.ExecuteSqlCommand(dbScriptforProvider);

            //drop the table
            //var tableName = this.GetTableName<Domain.SMS>();
            //var tableName1 = this.GetTableName<SMSProvider>();
            //var tableName2 = this.GetTableName<SMSMessage>();

            //this.DropPluginTable(tableName);
            //this.DropPluginTable(tableName1);
            //this.DropPluginTable(tableName2);





            SaveChanges();
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : BaseEntity, new()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            throw new System.NotImplementedException();
        }

        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            throw new System.NotImplementedException();
        }

        public void Detach(object entity)
        {
            throw new NotImplementedException();
        }
    }
}
   