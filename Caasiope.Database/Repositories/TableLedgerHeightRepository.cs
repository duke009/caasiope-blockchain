﻿using System.Data.Entity;
using Caasiope.Database.Repositories.Entities;
using Caasiope.Database.SQL;
using Caasiope.Database.SQL.Entities;

namespace Caasiope.Database.Repositories
{
    public class TableLedgerHeightRepository : Repository<TableLedgerHeight, tableledgerheight>
    {
        protected override tableledgerheight ToEntity(TableLedgerHeight item)
        {
            return new tableledgerheight
            {
                table_name = item.TableName,
                processed_ledger_height = item.Height
            };
        }

        protected override TableLedgerHeight ToItem(tableledgerheight entity)
        {
            return new TableLedgerHeight(entity.table_name, entity.processed_ledger_height);
        }

        protected override bool CheckIsNew(BlockchainEntities entities, TableLedgerHeight item)
        {
            return item.Height == 0;
        }

        protected override DbSet<tableledgerheight> GetDbSet(BlockchainEntities entities)
        {
            return entities.tableledgerheights;
        }
    }
}