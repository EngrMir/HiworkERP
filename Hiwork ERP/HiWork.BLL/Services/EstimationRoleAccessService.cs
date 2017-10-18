

using AutoMapper;
using HiWork.BLL.Models;
using HiWork.Utils;
using HiWork.DAL.Database;
using HiWork.DAL.Repositories;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HiWork.BLL.Services
{
    public partial interface IEstimationRoleAccessService
    {
        EstimationRoleAccessModel GetEstimationRoleAccess(EstimationRoleAccessModel model);
    }

    public class EstimationRoleAccessService : IEstimationRoleAccessService
    {
        private CentralDBEntities _dbContext;
        public EstimationRoleAccessService()
        {
            _dbContext = new CentralDBEntities();
        }

        public EstimationRoleAccessModel GetEstimationRoleAccess(EstimationRoleAccessModel model)
        {
            var item = (from e in _dbContext.EstimationUserAccesses
                        where 
                            (1 == 1 
                            && e.UserID == model.UserID 
                            && e.EstimationTypeID == model.EstimationTypeID 
                            && e.EstimationStatusID == model.EstimationStatusID)
                        select new EstimationRoleAccessModel
                        {
                            UserID = e.UserID,
                            EstimationStatusID = e.EstimationStatusID,
                            EstimationTypeID = e.EstimationTypeID,
                            Options = e.Options
                        }).FirstOrDefault();
            return item;
        }
    }
}
