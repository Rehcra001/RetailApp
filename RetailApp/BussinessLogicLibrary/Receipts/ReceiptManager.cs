using DataAccessLibrary.ReceiptRepository;
using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogicLibrary.Receipts
{
    public class ReceiptManager
    {
        private readonly string _connectionString;

        public ReceiptManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ReceiptModel Insert(ReceiptingLineModel receiptLine)
        {
            //Validate the receipting line
            receiptLine.Validate();
            if (!string.IsNullOrWhiteSpace(receiptLine.ValidationMessage))
            {
                //Validation Error
                throw new Exception(receiptLine.ValidationMessage);
            }

            //Validate the receipt
            ReceiptModel receipt = new ReceiptModel
            {
                PurchaseOrderID = receiptLine.PurchaseOrderID,
                ProductID = receiptLine.ProductID,
                QuantityReceipted = receiptLine.QtyToReceipt,
                UnitCost = receiptLine.UnitCost
            };

            receipt.Validate();
            if (!string.IsNullOrWhiteSpace(receipt.ValidationMessage))
            {
                //Validation Error
                throw new Exception(receipt.ValidationMessage);
            }

            //Insert the receipt
            Tuple<ReceiptModel, string> insertedReceipt = new ReceiptRepository(_connectionString).Insert(receipt).ToTuple();
            //Check for errors
            if (insertedReceipt.Item2 == null)
            {
                //No errors
                return insertedReceipt.Item1;
            }
            else
            {
                //Error raised inserting into database
                throw new Exception(insertedReceipt.Item2);
            }
        }

        public IEnumerable<ReceiptModel> GetByPurchaseOrderID(long id)
        {
            Tuple<IEnumerable<ReceiptModel>, string> receipts = new ReceiptRepository(_connectionString).GetByPurchaseOrderID(id).ToTuple();

            //check for errors
            if (receipts.Item2 == null)
            {
                //No errors
                return receipts.Item1;
            }
            else
            {
                //error raised retrieving from database
                throw new Exception(receipts.Item2);
            }
        }
    }
}
