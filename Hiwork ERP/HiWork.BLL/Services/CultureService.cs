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
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Services
{
    public partial interface ICultureService : IBaseService<CultureModel, Master_Culture>
    {
        List<CultureModel> SaveCulture(CultureModel aCultureModel);
        List<CultureModel> GetAllCultureList(CultureModel aCultureModel);
        CultureModel EditCulture(CultureModel aCultureModel);
        List<CultureModel> DeleteCulture(CultureModel aCultureModel);
    }
    public class CultureService : BaseService<CultureModel, Master_Culture>, ICultureService
    {
        private readonly ICultureRepository _cultureRepository;
        public CultureService(ICultureRepository cultureRepository)
            : base(cultureRepository)
        {
            _cultureRepository = cultureRepository;
        }

        public List<CultureModel> SaveCulture(CultureModel aCultureModel)
        {
            List<CultureModel> roles = null;            
            Utility.SetDynamicPropertyValue(aCultureModel, aCultureModel.CurrentCulture);

            var culture = Mapper.Map<CultureModel, Master_Culture>(aCultureModel);
            if (culture.ID !=null)
            {
                culture.UpdatedBy = aCultureModel.CurrentUserID;
                culture.UpdatedDate = DateTime.Now;
                _cultureRepository.UpdateCulture(culture);
            }                
            else
            {
                culture.CreatedBy = aCultureModel.CurrentUserID;
                culture.CreatedDate = DateTime.Now;
                _cultureRepository.InsertCulture(culture);
            }
            roles = GetAllCultureList(aCultureModel);           
            return roles;            
        }
        public CultureModel EditCulture(CultureModel aCultureModel)
        {
            var culture = Mapper.Map<CultureModel, Master_Culture>(aCultureModel);
            Master_Culture aCulture= _cultureRepository.UpdateCulture(culture);
            CultureModel cultureModel = Mapper.Map<Master_Culture, CultureModel>(aCulture);
            return cultureModel;
        }
        
        public List<CultureModel> GetAllCultureList(CultureModel model)
        {
            CultureModel culturemodel = new CultureModel();
            List<CultureModel> cultureList = new List<CultureModel>();

            var DbCulList = _cultureRepository.GetAllCultureList();

            if (DbCulList != null)
            {
               
                DbCulList.ForEach(a =>
                {
                    culturemodel = Mapper.Map<Master_Culture,CultureModel>(a);
                    culturemodel.country = Mapper.Map<Master_Country, CountryModel>(a.Master_Country);
                    if (culturemodel.country != null)
                        culturemodel.CountryName = Utility.GetPropertyValue(culturemodel.country, "Name", model.CurrentCulture) == null ? string.Empty :
                                                          Utility.GetPropertyValue(culturemodel.country, "Name", model.CurrentCulture).ToString();

                    cultureList.Add(culturemodel);
                });
             }
            

            return cultureList;
        }        
        public List<CultureModel> DeleteCulture(CultureModel aCultureModel)
        {
            _cultureRepository.DeletCulture(aCultureModel.ID);
            List<CultureModel> cultures = GetAllCultureList(aCultureModel);
            return cultures;
        }


        //public List<CultureViewModel> GetCultureViewModel(CultureModel aCultureModel)
        //{
        //    ICountryRepository Country_repo = new CountryRepository(new UnitOfWork());
           
        //    List<CultureViewModel> culList = new List<CultureViewModel>();

        //    var CulList = GetAllCultureList(aCultureModel);
        //    var CountryList = Country_repo.GetAllCountryList();

        //    return culList;
        //}
    }
}
