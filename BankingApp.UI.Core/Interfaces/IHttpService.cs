﻿
using System.Threading.Tasks;

namespace BankingApp.UI.Core.Interfaces
{
    public interface IHttpService
    {
        Task<T> GetAsync<T>(string uri, bool authorized = true);
        Task<T> PostAsync<T>(string uri, object value, bool authorized = true);
    }
}