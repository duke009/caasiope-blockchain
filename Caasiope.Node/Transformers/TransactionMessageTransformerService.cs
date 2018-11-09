using System.Collections.Generic;
using Caasiope.Database.Repositories;
using Caasiope.Database.Repositories.Entities;
using Caasiope.Protocol.Types;

namespace Caasiope.Node.Transformers
{
    internal class TransactionMessageTransformerService : DataTransformerService<TransactionMessageEntity, TransactionMessageRepository>
    {
        protected override IEnumerable<TransactionMessageEntity> Transform(DataTransformationContext context)
        {
            var signedLedgerState = context.SignedLedgerState;
            var list = new List<TransactionMessageEntity>();
            var transactions = signedLedgerState.Ledger.Ledger.Block.Transactions;
            foreach (var transaction in transactions)
            {
                var hash = transaction.Hash;
                if (transaction.Transaction.Message != null && !transaction.Transaction.Message.Equals(TransactionMessage.Empty)) // TODO we don't really want to save empty messages, right?
                    list.Add(new TransactionMessageEntity(hash, transaction.Transaction.Message));
            }
            return list;
        }
    }
}