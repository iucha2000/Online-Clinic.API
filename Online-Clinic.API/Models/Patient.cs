﻿using System.Text.Json.Serialization;

namespace Online_Clinic.API.Models
{
    public class Patient : Account
    {
        public Reservation[]? Reservations { get; set; }
    }
}
