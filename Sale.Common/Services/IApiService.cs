﻿using Sale.Common.Request;
using Sale.Common.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Common.Services
{
   public interface IApiService
    {
        Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller);
        Task<Response> GetTokenAsync(string urlBase, string servicePrefix, string controller, TokenRequest request);
        Task<Response> PostQualificationAsync(string urlBase, string servicePrefix, string controller, QualificationRequest qualificationRequest, string token);
        Task<Response> RegisterUserAsync(string urlBase, string servicePrefix, string controller, UserRequest userRequest);
        Task<Response> RecoverPasswordAsync(string urlBase, string servicePrefix, string controller, emailrequest emailRequest);
        Task<Response> ModifyUserAsync(string urlBase, string servicePrefix, string controller, UserRequest userRequest, string token);
        Task<Response> ChangePasswordAsync(string urlBase, string servicePrefix, string controller, ChangePasswordRequest changePasswordRequest, string token);
        Task<Response> PostAsync<T>(string urlBase, string servicePrefix, string controller, T model, string token);
        Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller, string token);
        Task<Response> PutAsync<T>(string urlBase, string servicePrefix, string controller, T model, string token);
    }
}
