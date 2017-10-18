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
    public partial interface ICategoryService : IBaseService<CategoryModel, Master_StaffCategory>
    {
        List<CategoryModel> SaveCategory(CategoryModel model);
        List<CategoryModel> GetAllCategoryList(CategoryModel model);
        List<CategoryModel> DeleteCategory(CategoryModel model);
    }

    public class CategoryService : BaseService<CategoryModel, Master_StaffCategory>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
            : base(categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }      

        public List<CategoryModel> SaveCategory(CategoryModel model)
        {
            List<CategoryModel> categorys = null;
            try
            {
                Utility.SetDynamicPropertyValue(model, model.CurrentCulture);

            var category = Mapper.Map<CategoryModel, Master_StaffCategory>(model);
            if (category.ID != Guid.Empty)
            {
                category.UpdatedBy = model.CurrentUserID;
                category.UpdatedDate = DateTime.Now;
                _categoryRepository.UpdateCategory(category);
            }
            else
            {
                    category.ID = Guid.NewGuid();
                    category.CreatedBy = model.CurrentUserID;
                category.CreatedDate = DateTime.Now;
                _categoryRepository.InsertCategory(category);
            }
            categorys = GetAllCategoryList(model);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Category", message);
                throw new Exception(ex.Message);
            }
            return categorys;
        }
        public List<CategoryModel> GetAllCategoryList(CategoryModel model)
        {
            List<CategoryModel> categoryList = new List<CategoryModel>();
            CategoryModel CategoryModel = new CategoryModel();
            try
            {
                List<Master_StaffCategory> categoryDataList = _categoryRepository.GetAllCategoryList();
            if (categoryDataList != null)
            {
                categoryDataList.ForEach(a =>
                {
                    CategoryModel = Mapper.Map<Master_StaffCategory, CategoryModel>(a);
                    CategoryModel.Name = Utility.GetPropertyValue(CategoryModel, "Name", model.CurrentCulture) == null ? string.Empty :
                        Utility.GetPropertyValue(CategoryModel, "Name", model.CurrentCulture).ToString();
                    CategoryModel.Description = Utility.GetPropertyValue(CategoryModel, "Description", model.CurrentCulture) == null ? string.Empty :
                        Utility.GetPropertyValue(CategoryModel, "Description", model.CurrentCulture).ToString();
                    CategoryModel.CurrentUserID = model.CurrentUserID;
                    CategoryModel.CurrentCulture = model.CurrentCulture;
                    categoryList.Add(CategoryModel);
                });
            }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Category", message);
                throw new Exception(ex.Message);
            }
            return categoryList;
        }
        public List<CategoryModel> DeleteCategory(CategoryModel model)
        {
            List<CategoryModel> categories = null;
            try
            {
                _categoryRepository.DeleteCategory(model.ID);
                categories = GetAllCategoryList(model);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Category", message);
                throw new Exception(ex.Message);
            }
            return categories;
        }
    }
}
