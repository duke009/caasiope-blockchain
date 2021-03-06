﻿using System.Collections.Generic;
using System.Linq;
using Caasiope.Explorer.Services;
using Caasiope.Node;
using Helios.Common.Concepts.Services;
using Helios.Common.Configurations;

//The blockchain explorer of Caasiope blockchain
//Authors:
//  Guillaume Bonnot
//    Github : guillaumebonnot
//  Ilia Palekhov
//    Github : duke009

namespace Caasiope.Explorer
{
	public class BlockchainExplorer
	{
		public readonly IExplorerConnectionService ExplorerConnectionService;
		public readonly IExplorerDatabaseService ExplorerDatabaseService;
		public readonly IExplorerDataTransformationService ExplorerDataTransformationService;
		public readonly IOrderBookService OrderBookService;

        public BlockchainExplorer(ServiceManager services, IExplorerServiceFactory factory = null)
		{
			if (factory == null)
				factory = new RealExplorerServiceFactory();
			
			ExplorerConnectionService = services.Add(factory.CreateExplorerConnectionService());
		    ExplorerDatabaseService = services.Add(factory.CreateExplorerDatabaseService());
		    ExplorerDataTransformationService = services.Add(factory.CreateExplorerDataTransformationService());
		    OrderBookService = services.Add(factory.CreateOrderBookService());
		}
	}

    public class RealExplorerServiceFactory : IExplorerServiceFactory
	{
		public IExplorerConnectionService CreateExplorerConnectionService()
		{
			var server = new WebSocketServer(NodeConfiguration.GetPath("explorer_server.txt"));
		    var connection = new ExplorerConnectionService(server);
		    var dispatcher = new Dispatcher(connection.Logger);
            connection.SetDispatcher(dispatcher);
		    return connection;
		}

	    public IExplorerDatabaseService CreateExplorerDatabaseService()
	    {
            return new ExplorerDatabaseService();
	    }

	    public IExplorerDataTransformationService CreateExplorerDataTransformationService()
	    {
	        return new ExplorerDataTransformationService();
	    }

	    public IOrderBookService CreateOrderBookService()
	    {
	        var raw = new UrlConfiguration(NodeConfiguration.GetPath("symbols.txt"));
	        var symbols = raw.Lines.Select(_ =>
	        {
	            var splited = _.Split('|');
                return new Symbol(splited[0].Trim(), splited[1].Trim());
	        }).ToList();

	        return new OrderBookService(symbols);
	    }
	}

	public interface IExplorerServiceFactory
	{
		IExplorerConnectionService CreateExplorerConnectionService();
	    IExplorerDatabaseService CreateExplorerDatabaseService();
	    IExplorerDataTransformationService CreateExplorerDataTransformationService();
	    IOrderBookService CreateOrderBookService();
	}
}
