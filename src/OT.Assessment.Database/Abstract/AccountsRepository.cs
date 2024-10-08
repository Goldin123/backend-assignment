﻿using Dapper;
using Microsoft.Extensions.Logging;
using OT.Assessment.Database.Helper;
using OT.Assessment.Database.Interface;
using OT.Assessment.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Database.Abstract
{
    public class AccountsRepository : IAccounts
    {
        private readonly ILogger<AccountsRepository> _logger;
        private readonly IDatabaseConnection _databaseConnection;
        public AccountsRepository(ILogger<AccountsRepository> logger, IDatabaseConnection databaseConnection)
        {
            _logger = logger;
            _databaseConnection = databaseConnection;
        }
        public async Task<Guid?> AddAccountAsync(AddAccountRequest addAccountRequest)
        {
            try
            {
                using (var connection = _databaseConnection.CreateConnection())
                {
                    _logger.LogInformation($"{DateTime.Now} - {nameof(AccountsRepository)} - {nameof(AddAccountAsync)} - attempting to add new account for {addAccountRequest.FirstName}.");
                    // Generate new AccountID (GUID)
                    var newAccountID = Guid.NewGuid();

                    object parameters = CreateAccountRequestParameter(addAccountRequest, newAccountID);

                    // Execute the stored procedure and retrieve the same AccountID back
                    var accountID = await connection.ExecuteScalarAsync<Guid?>(
                        "sp_AddAccount",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    _logger.LogInformation($"{DateTime.Now} - {nameof(GamesRepository)} - {nameof(AddAccountAsync)} - account added with ID {accountID}.");

                    // Return the new AccountID
                    return accountID;
                }

            }
            catch (SqlException ex)
            {
                _logger.LogError($"{DateTime.Now} - SQL Exception: {nameof(AccountsRepository)} - {nameof(AddAccountAsync)} - {ex.Message}");
                throw new Exception("An error occurred while adding the account. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now} - General Exception: {nameof(AccountsRepository)} - {nameof(AddAccountAsync)} - {ex.Message}");
                throw new Exception(Nofications.GeneralExceptionMessage);
            }
        }
        public async Task<Guid?> AddCountryAsync(AddCountryRequest addCountryRequest) 
        {
            try
            {
                using (var connection = _databaseConnection.CreateConnection())
                {
                    _logger.LogInformation($"{DateTime.Now} - {nameof(AccountsRepository)} - {nameof(AddCountryAsync)} - attempting to add new country called {addCountryRequest.CountryName}.");
                    // Generate new Country (GUID)
                    var newCountryID = Guid.NewGuid();

                    var parameters = new
                    {
                        CountryID = newCountryID,
                        CountryCode = addCountryRequest.CountryCode,
                        CountryName = addCountryRequest.CountryName,

                    };

                    // Execute the stored procedure and retrieve the same AccountID back
                    var accountID = await connection.ExecuteScalarAsync<Guid?>(
                        "sp_AddCountry",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    _logger.LogInformation($"{DateTime.Now} - {nameof(GamesRepository)} - {nameof(AddCountryAsync)} - country added with ID {accountID}.");

                    // Return the new AccountID
                    return accountID;
                }

            }
            catch (SqlException ex)
            {
                _logger.LogError($"{DateTime.Now} - SQL Exception: {nameof(AccountsRepository)} - {nameof(AddCountryAsync)} - {ex.Message}");
                throw new Exception("An error occurred while adding the country. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now} - General Exception: {nameof(AccountsRepository)} - {nameof(AddAccountAsync)} - {ex.Message}");
                throw new Exception(Nofications.GeneralExceptionMessage);
            }
        }
        static object CreateAccountRequestParameter(AddAccountRequest addAccountRequest, Guid newAccountID)
        {
            return new
            {
                AccountID = newAccountID,
                FirstName = addAccountRequest.FirstName,
                Surname = addAccountRequest.Surname,
                Email = addAccountRequest.Email,
                Gender = addAccountRequest.Gender,
                PhysicalAddress1 = addAccountRequest.PhysicalAddress1,
                PhysicalAddress2 = addAccountRequest.PhysicalAddress2,
                PhysicalAddress3 = addAccountRequest.PhysicalAddress3,
                PhysicalCode = addAccountRequest.PhysicalCode,
                PostalAddress1 = addAccountRequest.PostalAddress1,
                PostalAddress2 = addAccountRequest.PostalAddress2,
                PostalAddress3 = addAccountRequest.PostalAddress3,
                PostalCode = addAccountRequest.PostalCode
            };
        }
        public async Task<IEnumerable<ApplicationPlayersResponse>> GetAllPlayersAsync() 
        {
            try
            {
                using (var connection = _databaseConnection.CreateConnection())
                {
                    var players = await connection.QueryAsync<ApplicationPlayersResponse>("sp_GetAllPlayers", commandType: CommandType.StoredProcedure);
                    _logger.LogInformation($"{DateTime.Now}- {nameof(AccountsRepository)} - {nameof(GetAllPlayersAsync)} - {players.Count()} retrieved.");
                    return players;
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError($"{DateTime.Now} - SQL Exception: {nameof(AccountsRepository)} - {nameof(GetAllPlayersAsync)} - {ex.Message}");
                throw new Exception("An error occurred while retrieving the players. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now} - General Exception: {nameof(AccountsRepository)} - {nameof(GetAllPlayersAsync)} - {ex.Message}");
                throw new Exception(Nofications.GeneralExceptionMessage);
            }
        }

        public async Task<IEnumerable<ApplicationCountriesResponse>> GetAllCountriesAsync()
        {
            try
            {
                using (var connection = _databaseConnection.CreateConnection())
                {
                    var countries = await connection.QueryAsync<ApplicationCountriesResponse>("sp_GetAllCountries", commandType: CommandType.StoredProcedure);
                    _logger.LogInformation($"{DateTime.Now}- {nameof(AccountsRepository)} - {nameof(GetAllCountriesAsync)} - {countries.Count()} retrieved.");
                    return countries;
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError($"{DateTime.Now} - SQL Exception: {nameof(AccountsRepository)} - {nameof(GetAllCountriesAsync)} - {ex.Message}");
                throw new Exception("An error occurred while retrieving the countries. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now} - General Exception: {nameof(AccountsRepository)} - {nameof(GetAllCountriesAsync)} - {ex.Message}");
                throw new Exception(Nofications.GeneralExceptionMessage);
            }
        }
    }
}
