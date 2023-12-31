﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary.RepositoryInterfaces
{
    public interface IProductRepository
    {
        (ProductModel, string) Insert(ProductModel product);
        string Update(ProductModel product);
        (IEnumerable<ProductModel>, string) GetAll();
        (ProductModel, string) GetByID(int id);
        (IEnumerable<ProductModel>, string) GetByVendorID(int id);
    }
}
