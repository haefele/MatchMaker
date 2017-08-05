﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MatchMaker.Api.Database;
using MatchMaker.Shared.MatchDays;
using Microsoft.AspNetCore.Mvc;

namespace MatchMaker.Api.Controllers
{
    [Route("MatchDays")]
    public class MatchDaysController : Controller
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public MatchDaysController(IDbConnectionFactory dbConnectionFactory)
        {
            this._dbConnectionFactory = dbConnectionFactory;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewMatchDay([FromBody]CreateMatchDayData data, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (data == null || data.ParticipantIds == null || data.ParticipantIds.Count == 0)
                return this.BadRequest();

            using (var connection = this._dbConnectionFactory.Create())
            using (var transaction = connection.BeginTransaction())
            {
                int matchDayId = await connection.CreateMatchDay(data.When, data.ParticipantIds, transaction, cancellationToken);
                var matchDayCompact = await connection.GetMatchDayCompact(matchDayId, transaction, cancellationToken);

                transaction.Commit();

                return this.Created(string.Empty, matchDayCompact);
            }
        }

        [HttpGet]
        [Route("{matchDayId:int}/NextMatch")]
        public async Task<IActionResult> GetNextMatch(int matchDayId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (matchDayId <= 0)
                return this.BadRequest();

            using (var connection = this._dbConnectionFactory.Create())
            using (var transaction = connection.BeginTransaction())
            {
                var matches = await connection.GetMatchesFromMatchDay(matchDayId, transaction, cancellationToken);
                var participants = await connection.GetParticipantsFromMatchDay(matchDayId, transaction, cancellationToken);

                var uniqueMatches = this.CreateUniqueMatches(participants);
                foreach (var match in matches)
                {
                    var uniqueMatch = uniqueMatches.FirstOrDefault(f =>
                        f.Participant1 == match.Item1 && f.Participant2 == match.Item2 ||
                        f.Participant1 == match.Item2 && f.Participant1 == match.Item1);

                    uniqueMatch.Count++;
                }

                return this.Ok();
            }
        }

        private List<Matchup> CreateUniqueMatches(List<int> participants)
        {
            var result = new List<Matchup>();

            for (int i = 0; i <= participants.Count; i++)
            {
                for (int o = i; o <= participants.Count; o++)
                {
                    result.Add(new Matchup
                    {
                        Participant1 = participants[i],
                        Participant2 = participants[o]
                    });
                }
            }

            return result;
        }

        private class Matchup
        {
            public int Participant1 { get; set; }
            public int Participant2 { get; set; }
            public int Count { get; set; }
        }
    }
}