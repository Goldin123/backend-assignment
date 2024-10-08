﻿using OT.Assessment.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Database.Interface
{
    public interface IAccounts
    {
        Task<Guid?> AddAccountAsync(AddAccountRequest addAccountRequest);
        Task<Guid?> AddCountryAsync(AddCountryRequest addCountryRequest);
        Task<IEnumerable<ApplicationPlayersResponse>> GetAllPlayersAsync();
        Task<IEnumerable<ApplicationCountriesResponse>> GetAllCountriesAsync();
    }
}
