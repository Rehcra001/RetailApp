using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogicLibrary.VAT
{
    public class VATManager : IVATManager
    {
        private readonly IVATRepository _vatRepository;

        public VATManager(IVATRepository vatRepository)
        {
            _vatRepository = vatRepository;
        }

        /// <summary>
        /// Get the VAT
        /// </summary>
        /// <returns>
        /// Returns a VATModel
        /// </returns>
        /// <exception cref="Exception">
        /// Throw an exception if unable to retrieve the vat
        /// </exception>
        public VatModel Get()
        {
            Tuple<VatModel, string> vat = _vatRepository.Get().ToTuple();
            //check for errors
            if (vat.Item2 == null)
            {
                //No error
                return vat.Item1;
            }
            else
            {
                //Error raised
                throw new Exception(vat.Item2);
            }
        }
    }
}
