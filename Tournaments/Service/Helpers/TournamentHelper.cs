using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tournaments.Models;

namespace Tournaments.Service.Helpers
{
    public class TournamentHelper
    {
        public List<Game> GenerateGames(List<Player> playerList, int numberOfMeetings, int tournamentId)
        {
            var gameList = new List<Game>();
            int awayPlayerId;
            int homePlayerId;
            List<int> playerIdList = playerList.Select(p => p.Id).ToList();

            if (playerIdList.Count % 2 != 0)
            {
                playerIdList.Add(-1);
            }

            int numDays = (playerIdList.Count - 1);
            int halfSize = playerIdList.Count / 2;

            List<int> teams = new List<int>();
            int round = 0;
            for (int r = 0; r < numberOfMeetings; r++)
            {

                teams.AddRange(playerIdList.Skip(halfSize).Take(halfSize));
                teams.AddRange(playerIdList.Skip(1).Take(halfSize - 1).ToArray().Reverse());

                int teamsSize = teams.Count;

                for (int day = 0; day < numDays; day++)
                {
                    round++;
                    int teamIdx = day%teamsSize;

                    if (day%2 < 1)
                    {
                        homePlayerId = teams[teamIdx];
                        awayPlayerId = playerIdList[0];
                    }
                    else
                    {
                        homePlayerId = playerIdList[0];
                        awayPlayerId = teams[teamIdx];
                    }
                    if (homePlayerId != -1 && awayPlayerId != -1)
                    {
                        var tmpGame = new Game()
                        {
                            HomePlayerId = homePlayerId,
                            AwayPlayerId = awayPlayerId,
                            TournamentId = tournamentId,
                            GameCreated = DateTime.Now,
                            Round = round
                        };
                        gameList.Add(tmpGame);
                    }
                    

                    for (int idx = 1; idx < halfSize; idx++)
                    {
                        int firstTeam = (day + idx)%teamsSize;
                        int secondTeam = (day + teamsSize - idx)%teamsSize;

                        if (teams[firstTeam] != -1 && teams[secondTeam] != -1)
                        {
                            var tmpGame = new Game()
                            {
                                HomePlayerId = teams[firstTeam],
                                AwayPlayerId = teams[secondTeam],
                                TournamentId = tournamentId,
                                GameCreated = DateTime.Now,
                                Round = round
                            };
                            gameList.Add(tmpGame);
                        }
                    }
                }

            }
            return gameList;
        }

        public List<PlayerStatsViewModel> GenerateStandings(List<Player> players, List<Game> games)
        {
            List<PlayerStatsViewModel> standings = new List<PlayerStatsViewModel>();

            //List<Game> playerGames;
            int gamesPlayed;
            int wins;
            int ties;
            int losses;
            int scoresForward;
            int scoresAgainst;
            int points;


            foreach (var player in players)
            {
                wins = 0;
                ties = 0;
                losses = 0;
                scoresForward = 0;
                scoresAgainst = 0;
                points = 0;
                var playerGames = games.FindAll(g => (g.HomePlayerId == player.Id || g.AwayPlayerId == player.Id) && g.HomePlayerScore.HasValue && g.AwayPlayerScore.HasValue);
                gamesPlayed = playerGames.Count;
                foreach (var game in playerGames)
                {
                    if (game.HomePlayerId == player.Id)
                    {
                        scoresForward += game.HomePlayerScore.Value;
                        scoresAgainst += game.AwayPlayerScore.Value;

                        if (game.HomePlayerScore.Value > game.AwayPlayerScore.Value)
                        {
                            wins++;
                            points += 3;
                        }
                        else if (game.AwayPlayerScore.Value > game.HomePlayerScore.Value)
                        {
                            losses++;
                        }
                        else
                        {
                            ties++;
                            points += 1;
                        }
                    }
                    else
                    {
                        scoresAgainst += game.HomePlayerScore.Value;
                        scoresForward += game.AwayPlayerScore.Value;

                        if (game.AwayPlayerScore.Value > game.HomePlayerScore.Value)
                        {
                            wins++;
                            points += 3;
                        }
                        else if (game.HomePlayerScore.Value > game.AwayPlayerScore.Value)
                        {
                            losses++;
                        }
                        else
                        {
                            ties++;
                            points += 1;
                        }
                    }
                }

                var tmpPlayerstatsViewModel = new PlayerStatsViewModel()
                {
                    Player = player,
                    GamesPlayed = gamesPlayed,
                    Wins = wins,
                    Ties = ties,
                    Losses = losses,
                    ScoresForward = scoresForward,
                    ScoresAgainst = scoresAgainst,
                    Points = points
                };
                standings.Add(tmpPlayerstatsViewModel);

            }

            standings = standings.OrderByDescending(s => s.Points).ThenByDescending(s => s.ScoresForward - s.ScoresAgainst).ThenByDescending(s => s.ScoresForward).ToList();
            return standings;
        }
    }
}