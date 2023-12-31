﻿using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;

namespace BussinessLogicLibrary.Sales
{
    public class GetSalesOrderHeaderByIDManager : IGetSalesOrderHeaderByIDManager
    {
        private readonly ISalesOrderHeaderRepository _salesOrderHeaderRepository;

        public GetSalesOrderHeaderByIDManager(ISalesOrderHeaderRepository salesOrderHeaderRepository)
        {
            _salesOrderHeaderRepository = salesOrderHeaderRepository;
        }

        public SalesOrderHeaderModel GetByID(long id)
        {
            Tuple<SalesOrderHeaderModel, string> salesOrder = _salesOrderHeaderRepository.GetBySalesOrderID(id).ToTuple();
            //Check for errors
            if (salesOrder.Item2 == null)
            {
                //No error
                return salesOrder.Item1;
            }
            else
            {
                //error raised 
                throw new Exception(salesOrder.Item2);
            }
        }
    }
}
