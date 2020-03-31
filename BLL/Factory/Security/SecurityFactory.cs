using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.Models;
using DAL.db;
using BLL.Factory;
using BLL.Common;
using BLL.Interfaces.Security;
using DAL.Helper;

namespace BLL.Factory.Security
{

    public class UserFactory : GenericFactory<IVMS_DBEntities, SEC_UserInformation>
    {

    }

    public class UserPasswordFactory : GenericFactory<IVMS_DBEntities, SEC_Password>
    {

    }

    public class LoginStatusFactory : GenericFactory<IVMS_DBEntities, SEC_LoginStatus>
    {

    }

    public class QuestionFactory : GenericFactory<IVMS_DBEntities, SEC_SecurityQuestion>
    {

    }

    public class UserGroupFactory : GenericFactory<IVMS_DBEntities, SEC_UserGroup>
    {

    }

    public class UserActionMappingFactory : GenericFactory<IVMS_DBEntities, SEC_UserActionMapping>
    {

    }

    public class UiPageFactory : GenericFactory<IVMS_DBEntities, SEC_UIPage>
    {

    }


    public class ModuleFactory : GenericFactory<IVMS_DBEntities, SEC_UIModule>
    {

    }


    public class SecurityFactorys : ISecurityFactory
    {
        private IVMS_DBEntities context;
        private IGenericFactory<SEC_UserInformation> _userFactory;
        private IGenericFactory<SEC_Password> _userPasswordFactory;
        private IGenericFactory<SEC_UIPage> _uiPageFactory;
        private IGenericFactory<SEC_UIModule> _moduleFactory;
        private IGenericFactory<SEC_UserActionMapping> _userActionMappingFactory;
        private IGenericFactory<SEC_LoginStatus> _loginStatusFactory;
        private IGenericFactory<SEC_UserGroup> _userGroupFactory;
        private string tableName = "User Group";
        private string tablePageName = "Page";
        public SecurityFactorys()
        {
            context = new IVMS_DBEntities();
        }

        //public SecurityFactorys(ICBSEntities context)
        //{
        //    this.context = context;
        //}
        public LogInStatus CheckLogIn(LogOnModel entity)
        {

            LogInStatus _LogInStatus = new LogInStatus();
            Dictionary<string, string> list = new Dictionary<string, string>();
            Encription encription = new Encription();
            try
            {
                _loginStatusFactory = new LoginStatusFactory();
                _userFactory = new UserFactory();

                //var data = _userFactory.GetAll().ToList();

                //TBLA_USER_INFORMATION tblUserInformation = _userFactory.FindBy(x => x.UserName == entity.UserName && x.IsActive == true && x.TBLB_COMPANY.Code.ToLower() == entity.Company.ToLower()).FirstOrDefault();
                SEC_UserInformation tblUserInformation = _userFactory.FindBy(x => x.UserName == entity.UserName && x.CompanyID == entity.CompanyID && x.BranchID == entity.BranchID && x.IsActive == true).FirstOrDefault();
                if (tblUserInformation != null)
                {
                    SEC_LoginStatus logInStatus = _loginStatusFactory.FindBy(x => x.UserID == tblUserInformation.ID).FirstOrDefault();
                    if (logInStatus != null)
                    {
                        if (logInStatus.ForcedLogOutStatus == true)
                        {
                            _LogInStatus.IsAllowed = false;
                            _LogInStatus.Message = "The Page is Under maintenance";
                        }
                        else
                        {
                            _userPasswordFactory = new UserPasswordFactory();
                            SEC_Password tblPassword = _userPasswordFactory.FindBy(x => x.ID == tblUserInformation.PasswordID).FirstOrDefault();
                            if (tblPassword != null && encription.Decrypt(tblPassword.NewPassword).Trim() == (entity.Password.Trim()))
                            {
                                {
                                    list.Add("UserId", tblUserInformation.ID.ToString());
                                    list.Add("UserName", tblUserInformation.UserName);
                                    list.Add("Name", tblUserInformation.UserFullName);
                                    list.Add("UserEmployee", tblUserInformation.EmployeeID.ToString());
                                    list.Add("UserCompany", tblUserInformation.CompanyID.ToString());
                                    list.Add("UserBranch", tblUserInformation.BranchID.ToString());

                                    _LogInStatus.IsAllowed = true;
                                    _LogInStatus.Status = list;
                                    _LogInStatus.Message = "Login Successfully";
                                }
                            }
                            else
                            {
                                _LogInStatus.IsAllowed = false;
                                _LogInStatus.Message = "Password or User Name does not match";
                            }
                        }
                    }
                    else
                    {
                        _userPasswordFactory = new UserPasswordFactory();
                        SEC_Password tblPassword = _userPasswordFactory.FindBy(x => x.ID == tblUserInformation.PasswordID).FirstOrDefault();
                        if (tblPassword != null && encription.Decrypt(tblPassword.NewPassword).Trim() == (entity.Password.Trim()))
                        {
                            {
                                list.Add("UserId", tblUserInformation.ID.ToString());
                                list.Add("UserName", tblUserInformation.UserName);
                                list.Add("Name", tblUserInformation.UserFullName);
                                list.Add("UserEmployee", tblUserInformation.EmployeeID.ToString());
                                list.Add("UserCompany", tblUserInformation.CompanyID.ToString());
                                list.Add("UserBranch", tblUserInformation.BranchID.ToString());

                                _LogInStatus.IsAllowed = true;
                                _LogInStatus.Status = list;
                                _LogInStatus.Message = "Login Successfully";
                            }
                        }
                        else
                        {
                            _LogInStatus.IsAllowed = false;
                            _LogInStatus.Message = "Password or User Name not matching";
                        }
                    }
                }
                else
                {
                    _LogInStatus.IsAllowed = false;
                    _LogInStatus.Message = "User are not exist";
                }

                return _LogInStatus;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public dynamic PagePermissedList(int userGroupId)
        {
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int userId = Convert.ToInt32(dictionary[3].Id);
            try
            {
                var menu = (from map in context.SEC_UserActionMapping.Where(x => (x.IsCreate == true || x.IsDelete == true || x.IsEdit == true || x.IsSelect == true))
                            join uip in context.SEC_UIPage on map.UIPageID equals uip.ID
                            join mod in context.SEC_UIModule on uip.ModuleID equals mod.ID
                            where map.UserGroupID == userGroupId
                            select new
                            {
                                ID = uip.ID,
                                ModuleID = map.UIModuleID,
                                UIPage = uip.UIPageName,
                                ModuleName = mod.UIName,
                                IsSelect = map.IsSelect,
                                IsCreate = map.IsCreate,
                                IsEdit = map.IsEdit,
                                IsDelete = map.IsDelete
                            });
                _moduleFactory = new ModuleFactory();
                List<SEC_UIModule> moldule = _moduleFactory.GetAll().ToList();
                List<PagePermissionVM> pagePermissionList = new List<PagePermissionVM>();
                foreach (var item in menu)
                {
                    var pagePermission = new PagePermissionVM();
                    pagePermission.ModuleID = item.ModuleID;
                    pagePermission.UIPageID = item.ID;
                    pagePermission.Module = item.ModuleName;
                    pagePermission.Page = item.UIPage;
                    pagePermissionList.Add(pagePermission);

                    if (moldule.Where(x => x.ID == item.ModuleID && (x.ParentModulIDHierarchy != null)).Count() > 0)
                    {
                        var getParent = moldule.Where(x => x.ID == item.ModuleID).FirstOrDefault();
                        if (getParent != null)
                        {
                            string hierarchi = getParent.ParentModulIDHierarchy;
                            string[] words = hierarchi.Split(',');
                            foreach (var module in words)
                            {
                                if (pagePermissionList.Where(x => x.Page == getParent.UIName).Count() < 1)
                                {
                                    pagePermission = new PagePermissionVM();
                                    pagePermission.ModuleID = item.ModuleID;
                                    pagePermission.UIPageID = item.ModuleID;
                                    pagePermission.Module = item.ModuleName;
                                    pagePermission.Page = item.ModuleName;
                                    pagePermissionList.Add(pagePermission);
                                }
                            }
                        }
                    }
                }
                return pagePermissionList.Distinct();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PagePermissionVM GetCrudPermission(int userGroupID, string pageName)
        {
            try
            {
                Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
                int userId = Convert.ToInt16(dictionary[3].Id);

                bool forcedLogInStatus = context.SEC_LoginStatus.Where(x => x.UserID == userId && x.ForcedLogOutStatus).FirstOrDefault() == null ? false : true;

                PagePermissionVM accountmapping = (from map in context.SEC_UserActionMapping.Where(x => (x.UserGroupID == userGroupID) && (x.IsCreate || x.IsDelete || x.IsEdit || x.IsSelect))
                                                   join uip in context.SEC_UIPage.Where(x => x.UIPageName.Trim().ToLower() == pageName.Trim().ToLower()) on map.UIPageID equals uip.ID
                                                   where map.UserGroupID == userGroupID
                                                   select new PagePermissionVM()
                                                   {
                                                       ID = 3,
                                                       UserGroupId = map.UserGroupID,
                                                       Select = map.IsSelect,
                                                       Create = map.IsCreate,
                                                       Edit = map.IsEdit,
                                                       Delete = map.IsDelete,
                                                       ForcedLogOut = forcedLogInStatus
                                                   }).FirstOrDefault();
                return accountmapping;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public dynamic GetPageList()
        {
            _userActionMappingFactory = new UserActionMappingFactory();
            _uiPageFactory = new UiPageFactory();
            _moduleFactory = new ModuleFactory();
            var module = _moduleFactory.GetAll().ToList();
            var page = _uiPageFactory.GetAll();

            List<MenuItemVM> pageUrlvmList = new List<MenuItemVM>();
            foreach (var item in page)
            {
                MenuItemVM pageUrlvm = new MenuItemVM();
                pageUrlvm.PageId = item.ID;
                pageUrlvm.ModuleId = item.ModuleID.GetValueOrDefault();
                pageUrlvm.ModuleName = item.Name;
                pageUrlvm.Select = false;
                pageUrlvm.Edit = false;
                pageUrlvm.Create = false;
                pageUrlvm.Delete = false;
                pageUrlvm.ShowHide = true;
                pageUrlvmList.Add(pageUrlvm);
            }
            var menu = (from item in module
                        select new
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Select = true,
                            Edit = true,
                            Create = true,
                            Delete = true,
                            ShowHide = false,
                            children = pageUrlvmList.Where(x => x.ModuleId == item.ID).ToList()
                        }).ToList();

            return menu;
        }

        public Result SaveUserGroupWithPagePermission(SEC_UserGroup userGroup, List<MenuItemVM> menuVm)
        {
            Result result = new Result();
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    _userGroupFactory = new UserGroupFactory();
                    _userGroupFactory.Add(userGroup);
                    bool isDuplicate = _userGroupFactory.FindBy(x => x.Name.ToLower().Trim() == userGroup.Name.ToLower().Trim()).Any(x => x.Name.ToLower() == userGroup.Name.ToLower());
                    if (!isDuplicate)
                    {
                        _userGroupFactory.Save();
                        _uiPageFactory = new UiPageFactory();
                        _userActionMappingFactory = new UserActionMappingFactory();
                        List<SEC_UIPage> uiPageList = _uiPageFactory.GetAll().ToList();
                        foreach (var item in uiPageList)
                        {
                            SEC_UserActionMapping userMappings = new SEC_UserActionMapping();
                            userMappings.UserGroupID = Convert.ToInt32(userGroup.ID);
                            userMappings.UIModuleID = Convert.ToInt32(item.ModuleID);
                            userMappings.UIPageID = item.ID;
                            userMappings.IsSelect = false;
                            userMappings.IsCreate = false;
                            userMappings.IsEdit = false;
                            userMappings.IsDelete = false;
                            userMappings.CreatedDate = DateTime.Now;
                            userMappings.CreatedBy = Convert.ToInt32(userGroup.CreatedBy);
                            _userActionMappingFactory.Add(userMappings);
                        }
                        _userActionMappingFactory.Save();

                        if (menuVm != null)
                        {
                            int userGroupId = Convert.ToInt32(userGroup.ID);
                            List<SEC_UserActionMapping> userMappingList = _userActionMappingFactory.FindBy(x => x.UserGroupID == userGroupId).ToList();

                            List<MenuItemVM> userMappingVmList = menuVm.Distinct().ToList();

                            foreach (var item in userMappingVmList)
                            {
                                SEC_UserActionMapping userMapping = new SEC_UserActionMapping();
                                userMapping = userMappingList.FirstOrDefault(x => x.UIPageID == item.PageId);
                                if (userMapping != null)
                                {
                                    userMapping.IsSelect = item.Select != null ? (bool)item.Select : userMapping.IsSelect;
                                    userMapping.IsCreate = item.Create != null ? (bool)item.Create : userMapping.IsCreate;
                                    userMapping.IsEdit = item.Edit != null ? (bool)item.Edit : userMapping.IsEdit;
                                    userMapping.IsDelete = item.Delete != null ? (bool)item.Delete : userMapping.IsDelete;
                                    _userActionMappingFactory.Edit(userMapping);
                                }
                            }
                            _userActionMappingFactory.Save();
                        }
                        result.message = "Saved Successfuly";
                        result.isSucess = true;
                        dbContextTransaction.Commit();
                        return result;
                    }
                    result.message = "Your entared code is duplicate";
                    result.isSucess = false;
                    dbContextTransaction.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    result.message = "Error occured";
                    result.isSucess = false;
                    dbContextTransaction.Rollback();
                }
            }
            return result;
        }
        public Result SaveUserGroupWithPageMapping(SEC_UserGroup userGroup)
        {
            Result result = new Result();
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    _userGroupFactory = new UserGroupFactory();
                    _userGroupFactory.Add(userGroup);
                    bool isDuplicate = _userGroupFactory.FindBy(x => x.Name.ToLower().Trim() == userGroup.Name.ToLower().Trim()).Any(x => x.Name.ToLower() == userGroup.Name.ToLower());
                    if (!isDuplicate)
                    {
                        result = _userGroupFactory.Save();
                        _uiPageFactory = new UiPageFactory();
                        _userActionMappingFactory = new UserActionMappingFactory();
                        if (result.isSucess)
                        {
                            List<SEC_UIPage> uiPageList = _uiPageFactory.GetAll().ToList();
                            foreach (var item in uiPageList)
                            {
                                SEC_UserActionMapping userMappings = new SEC_UserActionMapping();
                                userMappings.UserGroupID = Convert.ToInt32(userGroup.ID);
                                userMappings.UIModuleID = Convert.ToInt32(item.ModuleID);
                                userMappings.UIPageID = item.ID;
                                userMappings.IsSelect = false;
                                userMappings.IsCreate = false;
                                userMappings.IsEdit = false;
                                userMappings.IsDelete = false;
                                userMappings.CreatedDate = DateTime.Now;
                                userMappings.CreatedBy = Convert.ToInt32(userGroup.CreatedBy);
                                _userActionMappingFactory.Add(userMappings);
                            }
                            result = _userActionMappingFactory.Save();
                        }

                        if (result.isSucess)
                        {
                            result.message = result.SaveSuccessfull(tableName);
                            dbContextTransaction.Commit();
                            return result;
                        }
                        else
                        {
                            result.isSucess = false;
                            dbContextTransaction.Rollback();
                            return result;
                        }

                    }
                    result.message = "Your entared code is duplicate";
                    result.isSucess = false;
                    dbContextTransaction.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    result.message = "Error occured";
                    result.isSucess = false;
                    dbContextTransaction.Rollback();
                }
            }
            return result;
        }


        public Result DeleteUserGroup(int id)
        {
            Result result = new Result();

            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    _userGroupFactory = new UserGroupFactory();
                    _userFactory = new UserFactory();
                    int countUser = _userFactory.FindBy(x => x.UserGroupID == id).Count();
                    if (countUser == 0)
                    {
                        result.isSucess = true;
                        _userActionMappingFactory = new UserActionMappingFactory();
                        int countUserAction = _userActionMappingFactory.FindBy(x => x.UserGroupID == id).Count();
                        if (countUserAction > 0)
                        {
                            _userActionMappingFactory.Delete(x => x.UserGroupID == id);
                            result = _userActionMappingFactory.Save();
                        }

                        if (result.isSucess)
                        {
                            _userGroupFactory.Delete(x => x.ID == id);
                            result = _userGroupFactory.Save();
                        }

                        if (result.isSucess)
                        {
                            result.message = result.DeleteSuccessfull(tableName);
                            dbContextTransaction.Commit();
                            return result;

                        }


                    }
                }
                catch (Exception ex)
                {
                    result.message = ex.Message;
                    result.isSucess = false;
                    dbContextTransaction.Rollback();
                }
            }

            return result;
        }

        public dynamic GetEditPageList(int userGroupId)
        {
            try
            {
                _userActionMappingFactory = new UserActionMappingFactory();
                _uiPageFactory = new UiPageFactory();
                _moduleFactory = new ModuleFactory();
                _userActionMappingFactory = new UserActionMappingFactory();

                var module = _moduleFactory.GetAll().ToList();

                var page = from pag in context.SEC_UIPage
                           join map in context.SEC_UserActionMapping
                           on pag.ID equals map.UIPageID
                           where map.UserGroupID == userGroupId
                           select new
                           {
                               PageId = pag.ID,
                               ModuleId = map.UIModuleID,
                               ModuleName = pag.Name,
                               Select = map.IsSelect,
                               Create = map.IsCreate,
                               Edit = map.IsEdit,
                               Delete = map.IsDelete,
                               ShowHide = true
                           };

                var menu = (from item in module
                            select new
                            {
                                ID = item.ID,
                                Name = item.Name,
                                Select = true,
                                Edit = true,
                                Create = true,
                                Delete = true,
                                ShowHide = false,
                                children = page.Where(x => x.ModuleId == item.ID).ToList()
                            }).ToList();

                return menu;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Result EditUserGroupPagePermission(SEC_UserGroup userGroup, List<MenuItemVM> userMappingVm = null)
        {
            Result _result = new Result();
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
                    int userId = Convert.ToInt32(dictionary[3].Id);
                    _userGroupFactory = new UserGroupFactory();
                    if (userGroup != null)
                    {
                        userGroup.UpdatedBy = userId;
                        userGroup.UpdatedDate = DateTime.Now;
                        _userGroupFactory.Edit(userGroup);
                        _result = _userGroupFactory.Save();
                    }

                    if (_result.isSucess)
                    {
                        if (userMappingVm != null)
                        {
                            _userActionMappingFactory = new UserActionMappingFactory();
                            int userGroupId = Convert.ToInt32(userGroup.ID);
                            List<SEC_UserActionMapping> userMappingList = _userActionMappingFactory.FindBy(x => x.UserGroupID == userGroupId).ToList();
                            foreach (var item in userMappingVm)
                            {
                                SEC_UserActionMapping userMapping = userMappingList.FirstOrDefault(x => x.UIPageID == item.PageId);
                                if (userMapping != null)
                                {
                                    userMapping.IsSelect = item.Select != null ? (bool)item.Select : userMapping.IsSelect;
                                    userMapping.IsCreate = item.Create != null ? (bool)item.Create : userMapping.IsCreate;
                                    userMapping.IsEdit = item.Edit != null ? (bool)item.Edit : userMapping.IsEdit;
                                    userMapping.IsDelete = item.Delete != null ? (bool)item.Delete : userMapping.IsDelete;
                                    userMapping.UpdatedDate = DateTime.Now;
                                    userMapping.UpdatedBy = userId;
                                    _userActionMappingFactory.Edit(userMapping);
                                }
                                _result = _userActionMappingFactory.Save();
                            }

                            if (_result.isSucess)
                            {
                                _result.message = _result.UpdateSuccessfull(tableName);
                                dbContextTransaction.Commit();
                            }
                            else
                            {
                                _result.isSucess = false;
                                dbContextTransaction.Rollback();
                            }

                        }
                    }

                    return _result;
                }
                catch (Exception ex)
                {
                    _result.isSucess = false;
                    dbContextTransaction.Rollback();
                    return _result;
                }
            }
        }

        public Result UiPageSave(SEC_UIPage page)
        {
            Result _result = new Result();
            _uiPageFactory = new UiPageFactory();
            _userActionMappingFactory = new UserActionMappingFactory();
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int userId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
            int empId = Convert.ToInt32(dictionary[1].Id == "" ? 0 : Convert.ToInt32(dictionary[1].Id));
            try
            {
                if (page.ID > 0)
                {
                    _uiPageFactory.Edit(page);
                    _result = _uiPageFactory.Save();
                    if (_result.isSucess)
                    {
                        SEC_UserActionMapping userActionMapping = new SEC_UserActionMapping();
                        _userGroupFactory = new UserGroupFactory();
                        var groupID = _userGroupFactory.GetAll().ToList();
                        foreach (var details in groupID)
                        {
                            var uiMapping = _userActionMappingFactory.FindBy(x => x.UserGroupID == details.ID && x.UIPageID == page.ID).Count();
                            if (uiMapping == 0)
                            {
                                userActionMapping.UserGroupID = details.ID;
                                userActionMapping.UIPageID = page.ID;
                                userActionMapping.UIModuleID = Convert.ToInt32(page.ModuleID);
                                userActionMapping.IsCreate = false;
                                userActionMapping.IsDelete = false;
                                userActionMapping.IsEdit = false;
                                userActionMapping.IsSelect = false;
                                userActionMapping.CreatedBy = empId;
                                userActionMapping.CreatedDate = DateTime.Now;
                                _userActionMappingFactory.Add(userActionMapping);
                                _result = _userActionMappingFactory.Save();
                            }

                        }
                        
                        if (_result.isSucess)
                        {
                            _result.message = _result.UpdateSuccessfull(tablePageName);
                            return _result;
                        }
                        _result.message = _result.UpdateSuccessfull(tablePageName);
                    }
                }
                else
                {
                    int pageID = 1;
                    var prvPage = _uiPageFactory.GetLastRecord().OrderByDescending(x => x.ID).FirstOrDefault();

                    if (prvPage != null)
                    {
                        pageID = prvPage.ID + 1;
                    }
                    page.UrlID = 1;
                    page.ID = pageID;
                    _uiPageFactory.Add(page);
                    _result = _uiPageFactory.Save();
                    if (_result.isSucess)
                    {
                        var uiMapping = _userActionMappingFactory.FindBy(x => x.UserGroupID == userId && x.UIPageID == page.ID).Count();
                        if (uiMapping == 0)
                        {
                            SEC_UserActionMapping userActionMapping = new SEC_UserActionMapping();
                            _userGroupFactory = new UserGroupFactory();
                            var groupID = _userGroupFactory.GetAll().ToList();
                            foreach (var details in groupID)
                            {
                                userActionMapping.UserGroupID = details.ID;
                                userActionMapping.UIPageID = page.ID;
                                userActionMapping.UIModuleID = Convert.ToInt32(page.ModuleID);
                                userActionMapping.IsCreate = false;
                                userActionMapping.IsDelete = false;
                                userActionMapping.IsEdit = false;
                                userActionMapping.IsSelect = false;
                                userActionMapping.CreatedBy = empId;
                                userActionMapping.CreatedDate = DateTime.Now;
                                _userActionMappingFactory.Add(userActionMapping);
                                _result = _userActionMappingFactory.Save();
                            }
                            
                            if (_result.isSucess)
                            {
                                _result.message = _result.SaveSuccessfull(tablePageName);
                                _result.lastInsertedID = userActionMapping.ID;
                                return _result;
                            }
                        }
                        _result.message = _result.SaveSuccessfull(tablePageName);
                    }
                }
            }
            catch (Exception e)
            {
                _result.isSucess = false;
                _result.message = e.Message;
            }

            return _result;
        }
        public Result DeleteUiPage(int id)
        {
            Result result = new Result();

            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    result.isSucess = true;
                    _uiPageFactory = new UiPageFactory();
                    _userActionMappingFactory = new UserActionMappingFactory();
                    var userMappingPage = _userActionMappingFactory.FindBy(x => x.UIPageID == id).Count();
                    if (userMappingPage > 0)
                    {
                        _userActionMappingFactory.Delete(x => x.UIPageID == id);
                        result = _userActionMappingFactory.Save();
                    }

                    if (result.isSucess)
                    {
                        _uiPageFactory.Delete(x => x.ID == id);
                        result = _uiPageFactory.Save();
                        if (result.isSucess)
                        {
                            result.message = result.DeleteSuccessfull(tablePageName);
                            dbContextTransaction.Commit();
                            return result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.message = ex.Message;
                    result.isSucess = false;
                    dbContextTransaction.Rollback();
                }
            }

            return result;
        }
        public List<SEC_UIPage> SearchUiPages(int? pageID)
        {
            Result _result = new Result();
            _uiPageFactory = new UiPageFactory();
            try
            {
                var list = new List<SEC_UIPage>();
                if (pageID > 0)
                {
                    list = _uiPageFactory.FindBy(x => x.ID == pageID).ToList();
                }
                else
                {
                    list = _uiPageFactory.GetAll().ToList();
                }

                return list;

            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public List<SEC_UIModule> SearchUiModule(int? moduleID)
        {
            Result _result = new Result();
            _moduleFactory = new ModuleFactory();
            try
            {
                var list = new List<SEC_UIModule>();
                if (moduleID > 0)
                {
                    list = _moduleFactory.FindBy(x => x.ID == moduleID).ToList();
                }
                else
                {
                    list = _moduleFactory.GetAll().ToList();
                }

                return list;

            }
            catch (Exception e)
            {
                throw e;
            }

        }

    }
}
