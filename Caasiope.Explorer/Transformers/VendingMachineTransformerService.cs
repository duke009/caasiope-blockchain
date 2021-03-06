using System.Collections.Generic;
using Caasiope.Explorer.Database.Repositories;
using Caasiope.Explorer.Database.Repositories.Entities;
using Caasiope.Protocol.Types;

namespace Caasiope.Explorer.Transformers
{
    internal class VendingMachineTransformerService : ExplorerDataTransformerService<VendingMachineEntity, VendingMachineRepository>
    {
        protected override IEnumerable<VendingMachineEntity> Transform(DataTransformationContext context)
        {
            var machines = context.SignedLedgerState.State.VendingMachines;
            var list = new List<VendingMachineEntity>();

            var declarations = context.GetDeclarations();
            foreach (var machine in machines)
            {
                list.Add(new VendingMachineEntity(GetDeclarationId(declarations.AddressDeclarations, machine.Address), new VendingMachineAccount(machine.Address, machine.Owner, machine.CurrencyIn, machine.CurrencyOut, machine.Rate)));
            }
            return list;
        }

        private long GetDeclarationId(Dictionary<Address, TransactionDeclarationEntity> declarations, Address address)
        {
            return declarations[address].DeclarationId;
        }
    }
}