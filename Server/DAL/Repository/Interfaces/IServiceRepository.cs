﻿using BO.DTO.Requests;
using BO.DTO.Responses;
using BO.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public interface IServiceRepository : IGenericRepository<Service>
    {
        Task<PageResponse<Service>> GetAllAsync(PageRequest pageRequest);
        Task<Service> GetMetForServiceAsync(int id);
        Task<Service> GetServiceByDateAndService(DateTime date, bool midi);
        Task<Service> UpdateAllAsync(Service entity);
    }
}
