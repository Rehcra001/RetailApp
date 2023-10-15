using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;

namespace BussinessLogicLibrary.Receipts
{
    public class ReceiptManager : IReceiptManager
    {
        private readonly IReceiptsRepository _receiptsRepository;

        public ReceiptManager(IReceiptsRepository receiptsRepository)
        {
            _receiptsRepository = receiptsRepository;
        }

        public void Insert(IEnumerable<ReceiptingLineModel> receiptLines)
        {
            List<ReceiptModel> receipts = new List<ReceiptModel>();

            //Validate the receipting line
            foreach (ReceiptingLineModel receiptingLine in receiptLines)
            {
                if (!receiptingLine.Validate())
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

                if (!receipt.Validate())
                {
                    //Validation Error
                    throw new Exception(receipt.ValidationMessage);
                }
                receipts.Add(receipt);
            }



            foreach (ReceiptModel receipt in receipts)
            {
                //Insert the receipt
                Tuple<ReceiptModel, string> insertedReceipt = _receiptsRepository.Insert(receipt).ToTuple();
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
            string errorMessage = _receiptsRepository.ReverseByID(id);
            if (errorMessage != null)
            {
                throw new Exception(errorMessage);
            }
        }

        public IEnumerable<ReceiptModel> GetByPurchaseOrderID(long id)
        {
            Tuple<IEnumerable<ReceiptModel>, string> receipts = _receiptsRepository.GetByPurchaseOrderID(id).ToTuple();

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
