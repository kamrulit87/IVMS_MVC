using System.Collections.Generic;
using System.Web.Mvc;
using Anko.Models;
using BLL.Common;
using BLL.Factory;
using DAL.db;
using BLL.Models;

namespace BLL.Interfaces.Security
{
    public interface ISecurityFactory
    {

        LogInStatus CheckLogIn(LogOnModel entity);
        dynamic PagePermissedList(int userGroupId);
        PagePermissionVM GetCrudPermission(int userGroupId, string pageName);
        dynamic GetPageList();
        Result SaveUserGroupWithPagePermission(SEC_UserGroup userGroup, List<MenuItemVM> menuVm);
        Result SaveUserGroupWithPageMapping(SEC_UserGroup userGroup);
        Result DeleteUserGroup(int id);
        dynamic GetEditPageList(int userGroupId);
        Result EditUserGroupPagePermission(SEC_UserGroup userGroup, List<MenuItemVM> userMappingVm = null);
        Result UiPageSave(SEC_UIPage page);
        Result DeleteUiPage(int id);
        List<SEC_UIPage> SearchUiPages(int? pageID);
        List<SEC_UIModule> SearchUiModule(int? moduleID);
    }

    public interface IChangePasswordFactory
    {
        Result SelfPasswordChange(ChangePasswordModel changePassword);
        Result PasswordChangeByAdminSave(ChangePasswordModel changePassword);
        List<SEC_UserInformation> LoadAllUserName();
        List<SEC_UserInformation> LoadAllFullName();
    }

}
