using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Tournaments.Models;
using Tournaments.Service.Context;
using Tournaments.Service.Helpers;
using Microsoft.AspNet.Identity;

namespace Tournaments.Controllers
{
    public class TournamentController : Controller
    {
        private TournamentContext db = new TournamentContext();
        private TournamentHelper tournamentHelper = new TournamentHelper();


        // GET: Tournament
        [Authorize]
        public ActionResult Index()
        {
            var currentUserId = User.Identity.GetUserId();
            return View(db.Tournaments.Where(t => t.UserId == currentUserId).ToList());
        }

        // GET: Tournament/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = db.Tournaments.Find(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            var games = db.Games.Where(g => g.TournamentId == id).ToList();
            var playerList = games.Select(p => p.HomePlayerId).Union(games.Select(p => p.AwayPlayerId).Distinct().ToList());

            var players = db.Players.Where(p => playerList.Contains(p.Id)).ToList();

            var standings = tournamentHelper.GenerateStandings(players, games);

            var tournamentAdministrator = (User.Identity.IsAuthenticated && User.Identity.GetUserId() == tournament.UserId) || Request.Cookies["TournamentAdministrator"] != null && Convert.ToInt32(Request.Cookies["TournamentAdministrator"].Value) == tournament.Id;
            var scheduleVm = new ScheduleViewModel()
            {
                Games = games,
                Players = players,
                TournamentAdministrator = tournamentAdministrator
            };

            var vm = new TournamentDetailsViewModel()
            {
                Schedule = scheduleVm,
                Tournament = tournament,
                Standings = standings
            };

            return View(vm);
        }

        

        [HttpGet]
        public PartialViewResult StandingsPartial(int? homeScore, int? awayScore, int gameId, int tournamentId)
        {
            var dbResult = db.Games.SingleOrDefault(g => g.Id == gameId);
            if (dbResult != null)
            {
                if (!homeScore.HasValue || !awayScore.HasValue)
                {
                    dbResult.HomePlayerScore = null;
                    dbResult.AwayPlayerScore = null;
                    dbResult.GameDate = null;
                    db.SaveChanges();
                }
                else if(homeScore >= 0 && awayScore >= 0)
                {
                    dbResult.HomePlayerScore = homeScore;
                    dbResult.AwayPlayerScore = awayScore;
                    dbResult.GameDate = DateTime.Now;
                    db.SaveChanges();
                }
            }

            var games = db.Games.Where(g => g.TournamentId == tournamentId).ToList();
            var playerList = games.Select(p => p.HomePlayerId).Union(games.Select(p => p.AwayPlayerId).Distinct().ToList());

            var players = db.Players.Where(p => playerList.Contains(p.Id)).ToList();

            var standings = tournamentHelper.GenerateStandings(players, games);

            return PartialView("StandingsPartial", standings);
        }



        // GET: Tournament/Create
        public ActionResult Create()
        {
            var vm = new TournamentCreateViewModel();
            //vm.Tournament.NumberOfMeetings = 1;
            return View(vm);
        }

        // POST: Tournament/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Rounds,TournamentName")] Tournament tournament)
        public ActionResult Create(TournamentCreateViewModel vm, PlayerCreateViewModel players, List<int> playerIds)
        {
            var userId = User.Identity.GetUserId();
            vm.Tournament.UserId = userId;

            

            if (ModelState.IsValid)
            {
                db.Tournaments.Add(vm.Tournament);
                db.SaveChanges();
                var tournamentId = vm.Tournament.Id;

                if (string.IsNullOrEmpty(userId))
                {
                    HttpCookie cookie = new HttpCookie("TournamentAdministrator");
                    cookie.Value = tournamentId.ToString();
                    cookie.Expires.AddDays(30d);
                    Response.Cookies.Add(cookie);
                }

                var playerList = new List<Player>();
                var tmpPlayer = new Player();

                foreach (var playerName in players.NewPlayerList)
                {
                    tmpPlayer.PlayerName = playerName;
                    //tmpPlayer.UserId = userId;
                    db.Players.Add(tmpPlayer);
                    db.SaveChanges();
                    playerList.Add(new Player() { PlayerName = tmpPlayer.PlayerName, Id = tmpPlayer.Id});
                }
                //if (playerIds.Any())
                //{
                //    var existingPlayers = db.Players.Where(p => playerIds.Contains(p.Id)).ToList();
                //    playerList.AddRange(existingPlayers);
                //}

                var games = this.tournamentHelper.GenerateGames(playerList, vm.Tournament.NumberOfMeetings, tournamentId);
                db.Games.AddRange(games);
                db.SaveChanges();

                return RedirectToAction("Details", new {id = tournamentId });
            }

            return View();
        }




        [HttpGet]
        public PartialViewResult PlayerCreatePartial(int nrOfTeams)
        {
            var userId = User.Identity.GetUserId();
            var existingPlayers = new List<Player>();

            if (!string.IsNullOrEmpty(userId))
            {
                existingPlayers = db.Players.Where(p => p.UserId == userId).ToList();
            }

            var playerList = new List<string>();
            string tmpPlayerName;
            for (int i = 0; i < nrOfTeams; i++)
            {
                tmpPlayerName = "player" + (i + 1);
                playerList.Add(tmpPlayerName);
            }

            var playerCreateViewModel = new PlayerCreateViewModel()
            {
                NewPlayerList = playerList,
                ExistingPlayers = existingPlayers
            };

            return PartialView("PlayerCreatePartial", playerCreateViewModel);
        }


        // GET: Tournament/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = db.Tournaments.Find(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            return View(tournament);
        }

        // POST: Tournament/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NumberOfMeetings,TournamentName,UserId")] Tournament tournament)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tournament).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Details", new { id = tournament.Id });
            }
            return View(tournament);
        }

        // GET: Tournament/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = db.Tournaments.Find(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            return View(tournament);
        }

        // POST: Tournament/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tournament tournament = db.Tournaments.Find(id);
            db.Tournaments.Remove(tournament);
            List<Game> games = db.Games.Where(g => g.TournamentId == id).ToList();
            db.Games.RemoveRange(games);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public ActionResult SendEmail(string emailAddress, string url)
        {
            try
            {
                MailMessage mail = new MailMessage("hillipen@gmail.com", emailAddress);
                mail.Subject = "Follow this tournament!";
                mail.Body = "Follow this link to checkout whats going on in this tournament: " + url;


                SmtpClient client = new SmtpClient();
                client.Host = "smtp.elasticemail.com";
                client.Port = 2525;
                client.UseDefaultCredentials = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("niicklas@hotmail.com", "be82cd12-7867-4671-b882-a7e6ec70ef23");
                client.Send(mail);

                //SmtpClient client = new SmtpClient();
                //client.Host = "smtp.gmail.com";
                //client.Port = 587;
                //client.UseDefaultCredentials = false;
                //client.DeliveryMethod = SmtpDeliveryMethod.Network;
                //client.EnableSsl = true;
                //client.Credentials = new NetworkCredential("hillipen@gmail.com", "");
                //client.Send(mail);
            }
            catch (Exception)
            {
            }
            return this.View();
        }
    }
}
