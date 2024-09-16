﻿using Microsoft.Extensions.Logging;
using OT.Assessment.Database.Interface;
using OT.Assessment.Manager.UseCases.Games.Interface;
using OT.Assessment.Manager.UseCases.Wagers.Implementation;
using OT.Assessment.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Manager.UseCases.Games.Implementation
{
    public class GameManager : IGameManager
    {
        private readonly ILogger<GameManager> _logger;
        private readonly IGames _games;
        public GameManager(ILogger<GameManager> logger, IGames games)
        {
            _logger = logger;
            _games = games;
        }
        public async Task<Guid?> AddProviderAsync(AddProviderRequest addProviderRequest)
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now} - {nameof(GameManager)} - {nameof(AddProviderAsync)} - attempting to add a provider {addProviderRequest.ProviderName}.");
                return await _games.AddProviderAsync(addProviderRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now} - General Exception: {nameof(GameManager)} - {nameof(AddProviderAsync)} - {ex.Message}");
                throw new Exception("An unexpected error occurred. Please try again later.");
            }
        }

        public async Task<Guid?> AddGameAsync(AddGameRequest addGameRequest)
        {
            try 
            {
                _logger.LogInformation($"{DateTime.Now} - {nameof(GameManager)} - {nameof(AddGameAsync)} - attempting to add a game {addGameRequest.GameName}.");
                return await _games.AddGameAsync(addGameRequest);

            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now} - General Exception: {nameof(GameManager)} - {nameof(AddGameAsync)} - {ex.Message}");
                throw new Exception("An unexpected error occurred. Please try again later.");
            }
        }
    }
}
