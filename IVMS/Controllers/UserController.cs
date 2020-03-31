using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Common;
using BLL.Factory;
using BLL.Factory.Security;
using BLL.Interfaces;
using BLL.Interfaces.Security;
using BLL.Models;
using DAL.db;
using DAL.Helper;
using IVMS.Models;
using BLL.Factory.Setup;
using BLL.Interfaces.Setup;

namespace ICBS.Controllers
{
    public class UserController : Controller
    {
        // GET: User

        private IGenericFactory<SEC_UserInformation> _userFactory;
        private IGenericFactory<SEC_SecurityQuestion> _questionFactory;
        private IGenericFactory<SEC_Password> _passwordFactory;
        private IGenericFactory<SEC_UserGroup> _userGroupFactory;
        private IGenericFactory<Employee> _employeeFactory;
        private Result result;
        string tableName = "User";
        public ActionResult User()
        {
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
            if (userGroupId != 0)
            {
                ISecurityFactory securityLogInFactory = new SecurityFactorys();
                PagePermissionVM tblUserActionMapping = securityLogInFactory.GetCrudPermission(userGroupId, "User");
                if (tblUserActionMapping.Select == true)
                {
                    ViewBag.CallingForm = "Security";
                    ViewBag.CallingForm1 = "User";
                    ViewBag.CallingViewPage = "#";
                    return View();
                }
            }
            Session["logInSession"] = null;
            return Redirect("/#!/");
        }
        public ActionResult EditUser()
        {
            DefaultLoad();
            return View();
        }
        

        
        public ActionResult LoadAllUser()
        {
            try
            {
                Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
                int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
                if (userGroupId != 0)
                {
                    _userFactory = new UserFactory();
                    var users = _userFactory.GetAll().Select(x => new
                    {
                        x.ID,
                        x.UserFullName,
                        x.Address,
                        x.PhoneNo,
                        //Group = x.SEC_UserGroup.FirstOrDefault().Name,
                        Group = x.SEC_UserGroup.Name,
                        User = x.UserName,
                        x.IsActive,
                        x.UserGroupID,
                        x.Email,
                        x.UserName,
                        x.EmployeeID,
                        x.CompanyID,
                        x.BranchID,
                        x.Employee.EmpName,
                        x.Employee.DepartmentID
                    }).ToList();
                    return Json(users.OrderBy(x => x.UserFullName));
                }
                return Json(new { success = false, message = "LogOut" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(new { success = false, message = exception.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ActiveDeActiveUser(int id, bool status)
        {
            try
            {
                Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
                int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
                if (userGroupId != 0)
                {
                    ISecurityFactory _securityLogInFactory = new SecurityFactorys();
                    PagePermissionVM tblUserActionMapping = _securityLogInFactory.GetCrudPermission(userGroupId, "User");
                    if (tblUserActionMapping.Edit)
                    {
                        _userGroupFactory = new UserGroupFactory();
                        _userFactory = new UserFactory();
                        int userId = Convert.ToInt32(dictionary[3].Id);
                        SEC_UserInformation user = _userFactory.FindBy(x => x.ID == userId).FirstOrDefault();
                        SEC_UserGroup userGroup = _userGroupFactory.FindBy(x => x.ID == user.UserGroupID).FirstOrDefault();
                        if (userGroup != null && userGroup.IsAdmin)
                        {
                            _userFactory = new UserFactory();
                            SEC_UserInformation tblUserInformation = _userFactory.FindBy(x => x.ID == id).FirstOrDefault();
                            if (tblUserInformation != null)
                            {
                                tblUserInformation.IsActive = status;
                                _userFactory.Edit(tblUserInformation);
                            }
                            _userFactory.Save();
                            if (status)
                            {
                                return Json(new { success = true, message = "Sucessifuly activeted the User" }, JsonRequestBehavior.AllowGet);
                            }
                            return Json(new { success = true, message = "Sucessifuly de-activeted the User" }, JsonRequestBehavior.AllowGet);
                        }
                        return Json(new { success = false, message = "You are not Admin User" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { success = false, message = "You has no permission for edit" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false, message = "LogOut" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAllUserGroups()
        {
            try
            {
                _userGroupFactory = new UserGroupFactory();
                var userGroups = _userGroupFactory.GetAll().Select(x => new
                {
                    x.ID,
                    x.Name
                }).ToList();
                return Json(userGroups.OrderBy(x => x.Name) , JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(new { success = false, message = exception.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetDeptWiseEmployee(int deptID)
        {
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            IVMS_DBEntities db = new IVMS_DBEntities();
            try
            {
                if (deptID > 0)
                {
                    var employee = from b in db.Employees
                        where b.Status == 1 && b.DepartmentID == deptID
                        select new
                        {
                            b.EmployeeID,
                            b.EmpName
                        };
                    return Json(employee, JsonRequestBehavior.AllowGet);
                }
                
            }
            catch (Exception exception)
            {
                return Json(new { success = false, message = exception.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateUserSave(UserModel user)
        {
            JsonResult jsonResult = new JsonResult();
            try
            {
                Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
                int userId = Convert.ToInt32(dictionary[3].Id == "" ? 0 : Convert.ToInt32(dictionary[3].Id));
                if (userId != 0)
                {
                    _userFactory = new UserFactory();
                    _employeeFactory =new EmployeeFactory();

                    SEC_UserInformation isDuplicate = _userFactory.FindBy(x => x.UserName.ToLower().Trim() == user.UserName.ToLower().Trim()).FirstOrDefault();
                    if (isDuplicate == null)
                    {
                        var emp = _employeeFactory.FindBy(x => x.EmpCode == user.UserName).FirstOrDefault();
                        if (emp != null)
                        {
                            user.EmployeeID = emp.EmployeeID;
                            user.CompanyID = emp.CompanyID;
                            user.BranchID = emp.BranchID;
                            user.SecurityQuestion = "0";
                            user.SecurityQueAns = "BD";
                        }
                        else
                        {
                            return Json(new { success = false, message = "Your enter employee code is not registered in the employee registration section" }, JsonRequestBehavior.AllowGet);
                        }

                        jsonResult = CreateUser(user, userId);
                        return Json(jsonResult);
                    }
                    return Json(new { success = false, message = "Your entered user name are duplicated please chose another name" }, JsonRequestBehavior.AllowGet);
                }
                Session["logInSession"] = null;
                return Json(new { success = false, message = "LogOut" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(new { success = false, message = exception.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private JsonResult CreateUser(UserModel user, int userId)
        {           

            result = new Result();
            _questionFactory = new QuestionFactory();
            _passwordFactory = new UserPasswordFactory();

            var question = new SEC_SecurityQuestion();
            question.ID = Guid.NewGuid();
            question.SecurityQuestion = user.SecurityQuestion;
            question.SecutiryAnswer = user.SecurityQueAns;
            question.CreatedBy = userId;
            question.CreatedDate = DateTime.Now;           

            _questionFactory.Add(question);
            result = _questionFactory.Save();

            var password = new SEC_Password();
            if (result.isSucess)
            {
                
                var encription = new Encription();
                password.ID = Guid.NewGuid();
                password.NewPassword = encription.Encrypt(user.Password);
                password.OldPassword = "";
                password.IsSelfChanged = false;
                password.CreatedBy = userId;
                password.CreatedDate = DateTime.Now;
                _passwordFactory.Add(password);
                result = _passwordFactory.Save();
            }
            var userInformation = new SEC_UserInformation();
            //userInformation.ID = Guid.NewGuid();
            if (result.isSucess)
            {
                userInformation.EmployeeID = user.EmployeeID;
                userInformation.CompanyID = user.CompanyID;
                userInformation.BranchID = user.BranchID;
                userInformation.UserFullName = user.UserFullName;
                userInformation.UserName = user.UserName.ToLower().Trim();
                userInformation.Address = user.Address;
                userInformation.Email = user.EMail;
                userInformation.PhoneNo = user.PhoneNo;
                userInformation.SecurityQuestionID = question.ID;
                userInformation.PasswordID = password.ID;
                userInformation.IsEMailVerified = false;
                userInformation.IsPhoneNoVerified = false;
                userInformation.IsActive = true;
                userInformation.CreatedBy = userId;
                userInformation.CreatedDate = DateTime.Now;
                userInformation.UserGroupID = user.UserGroupID;
                _userFactory.Add(userInformation);
                result = _userFactory.Save();
            }

            if (result.isSucess)
            {
                result.message = result.SaveSuccessfull(tableName);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            
        }

        [HttpPost]
        public JsonResult UpdateUserForm(UserModel user)
        {
            try
            {
                result = new Result();
                Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
                int userId = Convert.ToInt32(dictionary[3].Id);
                if (userId != 0)
                {
                    
                    _userFactory = new UserFactory();                    
                    var aUserInformation = _userFactory.FindBy(x => x.UserName == user.UserName.ToLower().Trim()).FirstOrDefault();
                    if (aUserInformation != null)
                    {
                        aUserInformation.UpdatedDate = DateTime.Now;
                        aUserInformation.UpdatedBy = userId;
                        aUserInformation.UserGroupID = user.UserGroupID;
                        _userFactory.Edit(aUserInformation);
                        result = _userFactory.Save();   
                    }
                    return Json(new { success = false, message = "Your entared user name does not exist!!!" }, JsonRequestBehavior.AllowGet);
                }
                Session["logInSession"] = null;
                return Json(new { success = false, message = "LogOut" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(new { success = false, message = exception.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult LoadUserGroups()
        {
            try
            {
                Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
                int companyId = Convert.ToInt32(dictionary[1].Id == "" ? 0 : Convert.ToInt32(dictionary[1].Id));
                if (companyId != 0)
                {
                    _userGroupFactory = new UserGroupFactory();
                    var userGroups = _userGroupFactory.GetAll().Select(x => new
                    {
                        id = x.Name,
                        Group = x.Name
                    }).ToList();
                    return Json(new { success = true, data = userGroups.OrderBy(x => x.Group) }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false, message = "LogOut" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(new { success = false, message = exception.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UserRoleChange(int id, string userRole)
        {
            try
            {
                Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
                int companyId = Convert.ToInt32(dictionary[1].Id == "" ? 0 : Convert.ToInt32(dictionary[1].Id));
                if (companyId != 0)
                {
                    _userGroupFactory = new UserGroupFactory();
                    _userFactory = new UserFactory();
                    int userId = Convert.ToInt32(dictionary[3].Id);
                    SEC_UserInformation user = _userFactory.FindBy(x => x.ID == userId).FirstOrDefault();
                    SEC_UserGroup userGroup = _userGroupFactory.FindBy(x => x.ID == user.UserGroupID).FirstOrDefault();
                    if (userGroup != null && userGroup.IsAdmin)
                    {
                        SEC_UserGroup role = _userGroupFactory.FindBy(x => x.Name == userRole).FirstOrDefault();
                        _userFactory = new UserFactory();
                        SEC_UserInformation tblUserInformation = _userFactory.FindBy(x => x.ID == id).FirstOrDefault();
                        if (tblUserInformation != null)
                        {
                            tblUserInformation.UserGroupID = role.ID;
                            _userFactory.Edit(tblUserInformation);
                        }
                        _userFactory.Save();
                        return Json(new { success = true, message = "Sucessifuly changed the user role" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { success = false, message = "You are not Admin User" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false, message = "LogOut" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Delete(int id)
        {
            try
            {
                Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
                int companyId = Convert.ToInt32(dictionary[1].Id == "" ? 0 : Convert.ToInt32(dictionary[1].Id));
                if (companyId != 0)
                {
                    int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
                    ISecurityFactory _securityLogInFactory = new SecurityFactorys();
                    PagePermissionVM tblUserActionMapping = _securityLogInFactory.GetCrudPermission(userGroupId, "User");
                    if (tblUserActionMapping.Delete)
                    {
                        _userFactory = new UserFactory();
                        _userFactory.Delete(x => x.ID == id);
                        _userFactory.Save();
                        return Json(new { success = true, message = "Deleted Successfuly" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { success = false, message = "You has no delete permission" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false, message = "LogOut" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Another page use this User data" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult UserCreate()
        {
            
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
            if (userGroupId != 0)
            {
                ISecurityFactory securityLogInFactory = new SecurityFactorys();
                PagePermissionVM tblUserActionMapping = securityLogInFactory.GetCrudPermission(userGroupId, "User");
                if (tblUserActionMapping.Create)
                {
                    DefaultLoad();
                    return View();
                }
            }
            Session["logInSession"] = null;
            return Redirect("/#!/");
        }
        public void DefaultLoad()
        {
            ViewBag.CallingForm = "Security";
            ViewBag.CallingForm1 = "User";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "#!/User";
        }
    }
}