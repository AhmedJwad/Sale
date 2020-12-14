﻿using Sale.Common.Helpers;
using Sale.prism.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Sale.prism.Helpers
{
   public class Languages
    {
        static Languages()
        {
            CultureInfo ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            Culture = ci.Name;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Culture { get; set; }
        public static string Accept => Resource.Accept;
        public static string ConnectionError => Resource.Checktheinternetconnection;
        public static string Error => Resource.Error;
        public static string SearchProduct => Resource.SearchProduct;
        public static string Name => Resource.Name;
        public static string Description => Resource.Description;
        public static string Price => Resource.Price;
        public static string Category => Resource.Category;
        public static string IsStarred => Resource.IsStarred;
        public static string AddToCart => Resource.AddToCart;
        public static string Product => Resource.Product;
        public static string Products => Resource.Products;
        public static string Login => Resource.Login;
        public static string ShowShoppingCar => Resource.ShowShoppingCar;
        public static string ShowPurchaseHistory => Resource.ShowPurchaseHistory;
        public static string ModifyUser => Resource.ModifyUser;
        public static string Email => Resource.Email;
        public static string EmailError => Resource.EmailError;
        public static string EmailPlaceHolder => Resource.EmailPlaceHolder;
        public static string Password => Resource.Password;
        public static string PasswordError => Resource.PasswordError;
        public static string PasswordPlaceHolder => Resource.PasswordPlaceHolder;
        public static string ForgotPassword => Resource.ForgotPassword;
        public static string LoginError => Resource.LoginError;
        public static string Logout => Resource.Logout;
        public static string Register => Resource.Register;
        public static string LoginFirstMessage => Resource.LoginFirstMessage;
        public static string Qualification => Resource.Qualification;
        public static string Qualifications => Resource.Qualifications;
        public static string QualificationNumber => Resource.QualificationNumber;
        public static string Details => Resource.Details;
        public static string RemarksPlaceHolder => Resource.RemarksPlaceHolder;
        public static string QualificationError => Resource.QualificationError;
        public static string NewQualification => Resource.NewQualification;
        public static string Save => Resource.Save;
    }
}
