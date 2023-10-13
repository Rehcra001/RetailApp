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

        public void Insert(IEnumerable<ReceiptingLineModel> receiptLines)
        {
            List<ReceiptModel> receipts = new List<ReceiptModel>();

            //Validate the receipting line
            foreach (ReceiptingLineModel receiptingLine in receiptLines)
            {
                receiptingLine.Validate();
                if (!string.IsNullOrWhiteSpace(receiptingLine.ValidationMessage))
                {
                    //Validation Error
                    throw new Exception(receiptingLine.ValidationMessage);
                }

                //Validate the receipt
                ReceiptModel receipt = new ReceiptModel
                {
                    PurchaseOrderID = receiptingLine.PurchaseOrderID,
                    ProductID = receiptingLine.ProductID,
                    QuantityReceipted = receiptingLine.QtyToReceipt,
                    UnitCost = receiptingLine.UnitCost
                };

                receipt.Validate();
                if (!string.IsNullOrWhiteSpace(receipt.ValidationMessage))
                {
                    //Validation Error
                    throw new Exception(receipt.ValidationMessage);
                }
                receipts.Add(receipt);
            }



            foreach (ReceiptModel receipt in receipts)
            {
                //Insert the receipt
                Tuple<ReceiptModel, string> insertedReceipt = new ReceiptRepository(_connectionString).Insert(receipt).ToTuple();
                //Check for errors
                if (insertedReceipt.Item2 != null)
                {
                    //Error raised inserting into database
                    throw new Exception(insertedReceipt.Item2);
                }
            }
            
        }

        public void Reverse(int id)
        {
            string errorMessage = new ReceiptRepository(_connectionString).ReverseByID(id);
            if (errorMessage != null)
            {
                throw new Exception(errorMessage);
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
