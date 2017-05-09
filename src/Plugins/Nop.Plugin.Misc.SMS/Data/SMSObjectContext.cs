using Nop.Core;
using Nop.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Nop.Plugin.Misc.SMS.Data
{
	public class SMSObjectContext : DbContext, IDbContext
    {
        public bool ProxyCreationEnabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool AutoDetectChangesEnabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public SMSObjectContext(string nameOrConnectionString) : base(nameOrConnectionString) { }
		
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new SMSMap());
            modelBuilder.Configurations.Add(new SMSMessageMap());

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

            Database.ExecuteSqlCommand(dbScriptforSMS);
            Database.ExecuteSqlCommand(dbScriptforMessage);
			
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
   