﻿using MatchMaker.Shared.MatchDays;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatchMaker.UI.Services.MatchDays
{
    public interface IMatchDaysService
    {
        Task<MatchDayCompactDTO> CreateNewMatchDay(List<int> participantIds, DateTime when);

        Task<MatchDTO> GetNextMatch(int matchDayId);
    }
}