using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogicLibrary.Sales
{
    public class UpdateSalesOrderManager : IUpdateSalesOrderManager
    {
        private readonly IValidateExistingSalesOrderHeader _validateExistingSalesOrderHeader;
        private readonly IGetSalesOrderDetailsByIDManager _getSalesOrderDetailsByIDManager;
        private readonly ISalesOrderHeaderRepository _salesOrderHeaderRepository;
        private readonly ISalesOrderDetailRepository _salesOrderDetailRepository;

        private SalesOrderHeaderModel SalesOrder;
        private IEnumerable<SalesOrderDetailModel> _details;

        public UpdateSalesOrderManager(IValidateExistingSalesOrderHeader validateExistingSalesOrderHeader,
                                       ISalesOrderHeaderRepository salesOrderHeaderRepository,
                                       IGetSalesOrderDetailsByIDManager getSalesOrderDetailsByIDManager,
                                       ISalesOrderDetailRepository salesOrderDetailRepository)
        {
            _validateExistingSalesOrderHeader = validateExistingSalesOrderHeader;
            _getSalesOrderDetailsByIDManager = getSalesOrderDetailsByIDManager;
            _salesOrderHeaderRepository = salesOrderHeaderRepository;
            _salesOrderDetailRepository = salesOrderDetailRepository;
        }

        public void Update(SalesOrderHeaderModel salesOrderHeader)
        {
            SalesOrder = salesOrderHeader;
            _details = _getSalesOrderDetailsByIDManager.GetByID(salesOrderHeader.SalesOrderID);

            Tuple<bool, bool> validated = _validateExistingSalesOrderHeader.Validate(SalesOrder).ToTuple();
            //Check if new lines added
            if (validated.Item1 == true)
            {
                InsertNewDetails();
            }

            if (validated.Item2 == true)
            {
                UpdateExistingDetails();
            }
            UpdateHeader();
        }

        private void UpdateExistingDetails()
        {
            //update if the lines in _details
            foreach (SalesOrderDetailModel orderLine in SalesOrder.SalesOrderDetails)
            {
                //Check if existing
                if (_details.FirstOrDefault(x => x.ProductID == orderLine.ProductID) != default)
                {
                    _salesOrderDetailRepository.Update(orderLine);
                }
            }
        }

        private void InsertNewDetails()
        {
            //Insert the lines if not in _details
            foreach (SalesOrderDetailModel orderLine in SalesOrder.SalesOrderDetails)
            {
                //Check if existing
                if (_details.FirstOrDefault(x => x.ProductID == orderLine.ProductID) == default)
                {
                    _salesOrderDetailRepository.Insert(orderLine);
                }
            }
        }

        private void UpdateHeader()
        {
            //Update the header;
            _salesOrderHeaderRepository.Update(SalesOrder);
        }
    }
}
