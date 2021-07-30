﻿using AccountingNote.DBSource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AccountingNote.Auth
{
    public class AuthManager
    {
        /// <summary>
        /// check is logined
        /// </summary>
        /// <returns></returns>
        public static bool IsLogined()
        {            
            if (System.Web.HttpContext.Current.Session["UserLoginInfo"] == null)
                return false;
            else
                return true;            
        }
        /// <summary>
        /// 取得已登入的使用者資訊(沒有登入就回傳)
        /// </summary>
        /// <returns></returns>
        public static UserInfoModel GetCurrentUser()
        {
            string account = HttpContext.Current.Session["UserLoginInfo"] as string;

            if (account == null)
                return null;

            DataRow dr = UserInfoManager.GetUserInfoByAccount(account);

            if (dr == null)
            {
                HttpContext.Current.Session["UserLoginInfo"] = null;
                return null;
            }

            UserInfoModel model = new UserInfoModel();
            model.ID = dr["ID"].ToString();
            model.Account = dr["Account"].ToString();
            model.Name = dr["Name"].ToString();
            model.Email = dr["Email"].ToString();

            return model;
        }

        /// <summary>
        /// 登出
        /// </summary>
        public static void Logout()
        {
            HttpContext.Current.Session["UserLoginInfo"] = null;
        }

        /// <summary>
        /// 嘗試登入
        /// </summary>
        /// <param name="account"></param>
        /// <param name="ped"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static bool TryLogin(string account, string pwd, out string errorMsg)
        {
            // check empty
            if (string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(pwd))
            {
                errorMsg = "Account / PWD is required.";
                return false;
            }

            // read db and check
            var dr = UserInfoManager.GetUserInfoByAccount(account);

            // check null
            if (dr == null)
            {
                errorMsg = $"Account:{account} doesn't exists.";
                return false;
            }

            // check account / PWD
            // true為不區分大小寫，false為要區分大寫
            if (string.Compare(dr["Account"].ToString(), account, true) == 0 &&
               string.Compare(dr["PWD"].ToString(), pwd, false) == 0)
            {
                HttpContext.Current.Session["UserLoginInfo"] = dr["Account"].ToString();
              
                errorMsg = string.Empty;
                return true;
            }
            else
            {
                errorMsg = "Login fail. Please check Account / PWD.";
                return false;
            }
        }
    }
}
