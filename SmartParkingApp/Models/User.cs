﻿using System;

namespace SmartParkingApp.Models {
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CarPlateNumber { get; set; }
        public string Phone { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime BirthDate { get; set; }

    }
}
