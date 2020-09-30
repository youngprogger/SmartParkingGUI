﻿using Newtonsoft.Json;
using SmartParkingApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SmartParkingApp {
    public class ParkingManager
    {
        private List<ParkingSession> pastSessions;
        private List<ParkingSession> activeSessions;
        private List<Tariff> tariffTable;

        private List<User> users;

        private int parkingCapacity;
        private int freeLeavePeriod;
        private int nextTicketNumber;

        public ParkingManager()
        {
            LoadData();
        }

        public ParkingSession EnterParking(string carPlateNumber)
        {
            if (activeSessions.Count >= parkingCapacity || activeSessions.Any(s => s.CarPlateNumber == carPlateNumber))
                return null;

            var session = new ParkingSession
            {
                CarPlateNumber = carPlateNumber,
                EntryDt = DateTime.Now,
                TicketNumber = nextTicketNumber++,
                User = users.FirstOrDefault(u => u.CarPlateNumber == carPlateNumber)
            };
            session.UserId = session.User?.Id;

            activeSessions.Add(session);
            SaveData();
            return session;
        }

        public bool TryLeaveParkingWithTicket(int ticketNumber, out ParkingSession session)
        {
            session = GetSessionByTicketNumber(ticketNumber);

            var currentDt = DateTime.Now;  // Getting the current datetime only once

            var diff = (currentDt - (session.PaymentDt ?? session.EntryDt)).TotalMinutes;
            if (diff <= freeLeavePeriod)
            {
                session.TotalPayment = 0;
                CompleteSession(session, currentDt);
                return true;
            }
            else
            {
                session = null;
                return false;
            }
        }

        public decimal GetRemainingCost(int ticketNumber)
        {
            var currentDt = DateTime.Now;
            var session = GetSessionByTicketNumber(ticketNumber);

            var diff = (currentDt - (session.PaymentDt ?? session.EntryDt)).TotalMinutes;
            return GetCost(diff);
        }

        public void PayForParking(int ticketNumber, decimal amount)
        {
            var session = GetSessionByTicketNumber(ticketNumber);
            session.TotalPayment = (session.TotalPayment ?? 0) + amount;
            session.PaymentDt = DateTime.Now;
            SaveData();
        }

        public bool TryLeaveParkingByCarPlateNumber(string carPlateNumber, out ParkingSession session)
        {
            session = activeSessions.FirstOrDefault(s => s.CarPlateNumber == carPlateNumber);
            if (session == null)
                return false;

            var currentDt = DateTime.Now;

            if (session.PaymentDt != null)
            {
                if ((currentDt - session.PaymentDt.Value).TotalMinutes <= freeLeavePeriod)
                {
                    CompleteSession(session, currentDt);
                    return true;
                }
                else
                {
                    session = null;
                    return false;
                }
            }
            else
            {
                // No payment, within free leave period -> allow exit
                if ((currentDt - session.EntryDt).TotalMinutes <= freeLeavePeriod)
                {
                    session.TotalPayment = 0;
                    CompleteSession(session, currentDt);
                    return true;
                }
                else
                {
                    // The session has no connected customer
                    if (session.User == null)
                    {
                        session = null;
                        return false;
                    }
                    else
                    {
                        session.TotalPayment = GetCost((currentDt - session.EntryDt).TotalMinutes - freeLeavePeriod);
                        session.PaymentDt = currentDt;
                        CompleteSession(session, currentDt);
                        return true;
                    }
                }
            }
        }
        public bool Rewriter(string Login, string Name, DateTime? dateTime, string Password, string CarPlateNumber)
        {
            var user = new User();
            user.Login = Login;
            user.Name = Name;
            user.CarPlateNumber = CarPlateNumber;
            user.Password = Password;
            user.BirthDate = dateTime ?? new DateTime(0,0,0);
            if (users == null)
            {
                users = new List<User>();
                users.Add(user);
            }
            if (!users.Exists(u => u.Login == user.Login))
            {
                users.Add(user);
                Serialize(UsersFileName, users);
                return true;
            }
            return false;

        }
        public bool CheckLoginAndPassword(string login, string password)
        {
            if (users == null)
                return false;
                
            else
            {
                foreach (var s in users)
                {
                    if (s.Login == login && s.Password == password)
                    {
                        return true;
                    }
                }
                return false;

            }
        }

        public User User_Id(string Login)
        {
            return users.Find(u => u.Login == Login);
        }

        public ParkingSession Active_Session(User user)
        {
            return activeSessions.FirstOrDefault( a => a.UserId == user.Id);
        }
        public List<string> Passive_Session(User user)
        {
            var passive = pastSessions.FindAll(a => a.UserId == user.Id);
            List<string> past = new List<string>();
            foreach(var s in passive)
            {
                past.Add($"past ;{s.EntryDt};{s.ExitDt}");
            }
            return past;


        }
        public ParkingSession Find_String(string EntryDt, string ExitDt)
        {
            return pastSessions.FirstOrDefault(a => a.EntryDt.ToString() == EntryDt && a.ExitDt.ToString() == ExitDt) ?? activeSessions.FirstOrDefault(a => a.EntryDt.ToString() == EntryDt && a.ExitDt.ToString() == ExitDt);
        }

        public List<string> Active_Sessions()
        {
            List<string> active = new List<string>();
            foreach (var s in activeSessions)
            {
               active.Add($"active ;{s.EntryDt};{s.ExitDt}");
            }
            return active;
        }

        public List<string> Passive_Sessions()
        {
            List<string> passive = new List<string>();
            foreach (var s in pastSessions)
            {
                passive.Add($"active ;{s.EntryDt};{s.ExitDt}");
            }
            return passive;
        }

        public double ProgressBar()
        {
            return double.Parse(activeSessions.Count.ToString()) / double.Parse(parkingCapacity.ToString());
        }


        #region Helper methods
        private ParkingSession GetSessionByTicketNumber(int ticketNumber) {
            var session = activeSessions.FirstOrDefault(s => s.TicketNumber == ticketNumber);
            if(session == null)
                throw new ArgumentException($"Session with ticket number = {ticketNumber} does not exist");
            return session;
        }

        private decimal GetCost(double diffInMinutes) {
            var tariff = tariffTable.FirstOrDefault(t => t.Minutes >= diffInMinutes) ?? tariffTable.Last();
            return tariff.Rate;
        }

        private T Deserialize<T>(string fileName) {
            using(var sr = new StreamReader(fileName)) {
                using(var jsonReader = new JsonTextReader(sr)) {
                    var serializer = new JsonSerializer();
                    return serializer.Deserialize<T>(jsonReader);
                }
            }
        }

        private void Serialize<T>(string fileName, T data) {
            using(var sw = new StreamWriter(fileName)) {
                using(var jsonWriter = new JsonTextWriter(sw)) {
                    var serializer = new JsonSerializer();
                    serializer.Serialize(jsonWriter, data);
                }
            }
        }

        private class ParkingData {
            public List<ParkingSession> PastSessions { get; set; }
            public List<ParkingSession> ActiveSessions { get; set; }
            public int Capacity { get; set; }
        }

        private const string TariffsFileName = "../../../../data/tariffs.json";
        private const string ParkingDataFileName = "../../../../data/parkingdata.json";
        private const string UsersFileName = "../../../../data/users.json";

        private void LoadData() {
            tariffTable = Deserialize<List<Tariff>>(TariffsFileName);
            var data = Deserialize<ParkingData>(ParkingDataFileName);
            users = Deserialize<List<User>>(UsersFileName);

            parkingCapacity = data.Capacity;
            pastSessions = data.PastSessions ?? new List<ParkingSession>();
            activeSessions = data.ActiveSessions ?? new List<ParkingSession>();

            freeLeavePeriod = tariffTable.First().Minutes;
            nextTicketNumber = activeSessions.Count > 0 ? activeSessions.Max(s => s.TicketNumber) + 1 : 1;
        }

        private void SaveData() {
            var data = new ParkingData
            {
                Capacity = parkingCapacity,
                ActiveSessions = activeSessions,
                PastSessions = pastSessions
            };

            Serialize(ParkingDataFileName, data);
        }

        private void CompleteSession(ParkingSession session, DateTime currentDt) {
            session.ExitDt = currentDt;
            activeSessions.Remove(session);
            pastSessions.Add(session);
            SaveData();
        }
        #endregion
    }
}
