using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogicLibrary.Vendors
{
    public class VendorManager
    {
        public ObservableCollection<VendorModel> Vendors { get; set; } = new ObservableCollection<VendorModel>();
        private VendorModel undoEdit;
        public string State { get; private set; } // View, Add and Edit

        private int _bookMark; //Holds the index of the current record

        public VendorManager(ObservableCollection<VendorModel> vendors)
        {
            Vendors = vendors;
            _bookMark = 0;
            SetState("View");
        }

        private void SetState(string state)
        {
            State = state;
        }

        public void AddNewVendor()
        {
            SetState("Add");
            Vendors.Add(new VendorModel());
            _bookMark = Vendors.Count - 1;
        }

        public void EditVendor()
        {
            SetState("Edit");
        }

        public void DeleteVendor()
        {

        }

        public void Cancel()
        {
            if (State.Equals("Add"))
            {
                Vendors.RemoveAt(_bookMark);
                _bookMark = 0;//Move to first record
            }
            else if (State.Equals("Edit"))
            {

            }
            SetState("View");
        }

        public void SaveVendor()
        {
            if (State.Equals("Add"))
            {

            }
            else if (State.Equals("Edit"))
            {

            }
        }


    }
}
