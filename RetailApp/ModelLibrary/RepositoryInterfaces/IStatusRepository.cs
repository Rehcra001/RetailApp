﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary.RepositoryInterfaces
{
    public interface IStatusRepository
    {
        (IEnumerable<StatusModel>, string) GetAll();
        (StatusModel, string) GetByID(int id);
    }
}
