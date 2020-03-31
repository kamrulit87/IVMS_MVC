using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Anko.Models;
using BLL.Common;
using BLL.Interfaces;
using BLL.Interfaces.Security;
using BLL.Models;
using DAL.db;
using DAL.Helper;

namespace BLL.Factory.Security
{
    public class ChangePasswordFactorys : IChangePasswordFactory
    {
        private IGenericFactory<SEC_Password> _passwordFactory;
        private IGenericFactory<SEC_UserInformation> _userFactory;
        private IGenericFactory<SEC_UserGroup> _userGroup;
        private Result result;
        public Result SelfPasswordChange(ChangePasswordModel changePassword)
        {
            try
            {
                result = new Result();
                result.isSucess = false;
                Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
                int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));

                if (userGroupId != 0)
                {
                    int userId = Convert.ToInt32(dictionary[3].Id);
                    string userName = dictionary[4].Id;
                    _userFactory = new UserFactory();
                    bool status = _userFactory.GetAll().Any(x => x.UserName == userName && x.UserGroupID == userGroupId);
                    if (status == true)
                    {
                        SEC_UserInformation tblUserInformation;
                        tblUserInformation = _userFactory.FindBy(x => x.ID == userId).FirstOrDefault();
                        _passwordFactory = new UserPasswordFactory();
                        Encription encription = new Encription();
                        SEC_Password tblPassword = _passwordFactory.GetAll().FirstOrDefault(x => x.ID == tblUserInformation.PasswordID);
                        if (tblPassword != null)
                        {
                            tblPassword.OldPassword = tblPassword.NewPassword;
                            tblPassword.NewPassword = encription.Encrypt(changePassword.NewPassword.Trim());
                            tblPassword.IsSelfChanged = true;
                            tblPassword.UpdatedDate = DateTime.Now;
                            tblPassword.UpdatedBy = userId;
                            _passwordFactory.Edit(tblPassword);
                        }
                        result = _passwordFactory.Save();
                        if (result.isSucess)
                        {
                            result.message = "Changed Password Sucessfully";
                            return result;
                        }
                    }
                    result.message = "Password not Changed try again";
                    return result;
                }
                result.message = "LogOut";
            }
            catch (Exception exception)
            {
                result.isSucess = false;
                result.message = exception.Message;
            }
            return result;
        }

        public Result PasswordChangeByAdminSave(ChangePasswordModel changePassword)
        {
            try
            {
                result = new Result();
                result.isSucess = false;
                Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
                int userGroupID = Convert.ToInt32(dictionary[6].Id);
                int userId = Convert.ToInt32(dictionary[3].Id);
                string userName = dictionary[4].Id;
                if (userGroupID != 0)
                {
                    _userFactory = new UserFactory();
                    _passwordFactory = new UserPasswordFactory();
                    _userGroup = new UserGroupFactory();
                    Encription encription = new Encription();
                    SEC_Password tblPassword;
                    SEC_UserInformation tblUserInformation = new SEC_UserInformation();
                    SEC_UserGroup tblUserGroup = new SEC_UserGroup();

                    if (changePassword.FullName != "" || changePassword.UserName != "")
                    {
                        if (changePassword.UserName != null)
                        {
                            tblUserInformation = _userFactory.FindBy(x => x.UserName == changePassword.UserName).FirstOrDefault();
                        }

                        tblUserGroup = _userGroup.FindBy(x => x.ID == userGroupID).FirstOrDefault();
                        if (tblUserGroup != null)
                        {
                            if (tblUserGroup.IsAdmin == false)
                            {
                                result.message = "You are not a Admin";
                                return result;
                            }

                            tblPassword = _passwordFactory.GetAll().FirstOrDefault(x => x.ID == tblUserInformation.PasswordID);
                            if (tblPassword != null)
                            {
                                tblPassword.OldPassword = tblPassword.NewPassword;
                                tblPassword.NewPassword = encription.Encrypt(changePassword.NewPassword.Trim());
                                tblPassword.IsSelfChanged = false;
                                tblPassword.UpdatedDate = DateTime.Now;
                                tblPassword.UpdatedBy = userId;
                                _passwordFactory.Edit(tblPassword);
                            }
                            result = _passwordFactory.Save();
                            if (result.isSucess)
                            {
                                result.message = "Changed Password Sucessfully";
                                return result;
                            }
                        }
                        result.message = "User cant found";
                        return result;
                    }
                    result.message = "Password not Changed try again";
                    return result;
                }
                result.message = "Logout";
            }
            catch (Exception exception)
            {

                result.isSucess = false;
                result.message = exception.Message;
                return result;
            }
            return result;
        }

        public List<SEC_UserInformation> LoadAllFullName()
        {
            try
            {
                Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
                int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
                _userFactory = new UserFactory();
                var list = new List<SEC_UserInformation>();
                list = _userFactory.FindBy(x => x.IsActive == true && x.UserGroupID == userGroupId).ToList();
                return list;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public List<SEC_UserInformation> LoadAllUserName()
        {
            try
            {
                Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
                int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
                _userFactory = new UserFactory();
                var list = new List<SEC_UserInformation>();
                list = _userFactory.FindBy(x => x.IsActive == true && x.UserGroupID == userGroupId).ToList();
                return list;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
    
}
