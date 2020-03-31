﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAL.Helper
{
    public class CheckSessionData
    {
        public CheckSessionData(string id, string name)
        {
            Id = id;
            Name = name;
        }
        public string Id { get; set; }
        public string Name { get; set; }
        //Orginal Code GetSessionValues
        public static Dictionary<int, CheckSessionData> GetSessionValues()
        {            
            Dictionary<int, CheckSessionData> sessionData = new Dictionary<int, CheckSessionData>();
            if (HttpContext.Current.Session["LoginEmployee"] == null)
            {
                CheckSessionData aCheckSessionData = new CheckSessionData("", "EmployeeID");
                sessionData.Add(1, aCheckSessionData);
            }
            else
            {
                CheckSessionData aCheckSessionData = new CheckSessionData(Convert.ToString(HttpContext.Current.Session["LoginEmployee"].ToString()), "EmployeeID");
                sessionData.Add(1, aCheckSessionData);
            }
            if (HttpContext.Current.Session["LoginCompanyID"] == null)
            {
                CheckSessionData aCheckSessionData = new CheckSessionData("", "CompanyID");
                sessionData.Add(9, aCheckSessionData);
            }
            else
            {
                CheckSessionData aCheckSessionData = new CheckSessionData(Convert.ToString(HttpContext.Current.Session["LoginCompanyID"].ToString()), "CompanyID");
                sessionData.Add(9, aCheckSessionData);
            }
            if (HttpContext.Current.Session["LoginBranchID"] == null)
            {
                CheckSessionData aCheckSessionData = new CheckSessionData("", "BranchID");
                sessionData.Add(10, aCheckSessionData);
            }
            else
            {
                CheckSessionData aCheckSessionData = new CheckSessionData(Convert.ToString(HttpContext.Current.Session["LoginBranchID"].ToString()), "BranchID");
                sessionData.Add(10, aCheckSessionData);
            }
            if (HttpContext.Current.Session["LoginUserID"] == null)
            {
                CheckSessionData aCheckSessionData = new CheckSessionData("", "LoginUserID");
                sessionData.Add(3, aCheckSessionData);
            }
            else
            {
                CheckSessionData aCheckSessionData = new CheckSessionData(Convert.ToString(System.Web.HttpContext.Current.Session["LoginUserID"].ToString()), "LoginUserID");
                sessionData.Add(3, aCheckSessionData);
            }
            if (HttpContext.Current.Session["LoginUserName"] == null)
            {
                CheckSessionData aCheckSessionData = new CheckSessionData("", "UserName");
                sessionData.Add(4, aCheckSessionData);
            }
            else
            {
                CheckSessionData aCheckSessionData = new CheckSessionData(Convert.ToString(HttpContext.Current.Session["LoginUserName"].ToString()), HttpContext.Current.Session["LoginUserName"].ToString());
                sessionData.Add(4, aCheckSessionData);
            }
            if (HttpContext.Current.Session["LoginUserFullName"] == null)
            {
                CheckSessionData aCheckSessionData = new CheckSessionData("", "LoginUserFullName");
                sessionData.Add(5, aCheckSessionData);
            }
            else
            {
                CheckSessionData aCheckSessionData = new CheckSessionData(Convert.ToString(HttpContext.Current.Session["LoginUserFullName"].ToString()), HttpContext.Current.Session["LoginUserFullName"].ToString());
                sessionData.Add(5, aCheckSessionData);
            }
            if (HttpContext.Current.Session["UserGroupID"] == null)
            {
                CheckSessionData aCheckSessionData = new CheckSessionData("", "UserGroupID");
                sessionData.Add(6, aCheckSessionData);
            }
            else
            {
                CheckSessionData aCheckSessionData = new CheckSessionData(Convert.ToString(HttpContext.Current.Session["UserGroupID"].ToString()), HttpContext.Current.Session["UserGroupID"].ToString());
                sessionData.Add(6, aCheckSessionData);
            }
            if (HttpContext.Current.Session["IPAddress"] == null)
            {
                CheckSessionData aCheckSessionData = new CheckSessionData("", "IPAddress");
                sessionData.Add(7, aCheckSessionData);
            }
            else
            {
                CheckSessionData aCheckSessionData = new CheckSessionData(Convert.ToString(HttpContext.Current.Session["IPAddress"].ToString()), HttpContext.Current.Session["IPAddress"].ToString());
                sessionData.Add(7, aCheckSessionData);
            }
            if (HttpContext.Current.Session["PCName"] == null)
            {
                CheckSessionData aCheckSessionData = new CheckSessionData("", "PCName");
                sessionData.Add(8, aCheckSessionData);
            }
            else
            {
                CheckSessionData aCheckSessionData = new CheckSessionData(Convert.ToString(HttpContext.Current.Session["PCName"].ToString()), HttpContext.Current.Session["PCName"].ToString());
                sessionData.Add(8, aCheckSessionData);
            }

            return sessionData;
        }
    }
}
