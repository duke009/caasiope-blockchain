﻿using System;
using Caasiope.Database.Managers;
using Caasiope.Database.SQL;
using Helios.Common.Extensions;
using Helios.Common.Logs;

namespace Caasiope.Database
{
    public abstract class Transaction
    {
        private ILogger logger;

        public void Initialize(ILogger logger)
        {
            this.logger = logger;
        }

        protected abstract void Populate(RepositoryManager repositories, BlockchainEntities entities);

        public void Save(RepositoryManager repositories)
        {
            using (var entities = new BlockchainEntities())
            {
                using (new PerformanceLogger(logger, "PopulateEntitiesLogger"))
                {
                    // TODO this is slow
                    Populate(repositories, entities);
                }
                using (new PerformanceLogger(logger, "SaveDatabaseLogger"))
                {
                    entities.SaveChanges();
                }
            }
            OnSaveFinished();
        }

        protected abstract void OnSaveFinished();
    }
    public abstract class Transaction<T> : Transaction
    {
        protected readonly T Data;

        public Action<T> Callback { get; set; }

        protected Transaction(T data)
        {
            Data = data;
        }

        protected sealed override void OnSaveFinished()
        {
            Callback.Call(Data);
        }
    }
}
