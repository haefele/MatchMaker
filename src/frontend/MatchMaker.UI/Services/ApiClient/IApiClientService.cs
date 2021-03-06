﻿using MatchMaker.Shared.MatchDays;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MatchMaker.Shared.Accounts;

namespace MatchMaker.UI.Services.ApiClient
{
    public interface IApiClientService
    {
        Task Register(string email, string password);

        Task Login(string email, string password);

        Task<MatchDay> CreateNewMatchDay(List<int> participantIds, DateTime when);

        Task<Match> GetNextMatch(int matchDayId);

        Task<Match> SaveMatch(int matchDayId, Match match);

        Task<IList<Account>> SearchAccount(string searchText);
    }
}