﻿using Online_Clinic.API.Interfaces;
using Online_Clinic.API.Models;

namespace Online_Clinic.API.Repositories.Oracle
{
    public class PKG_RESERVATIONS : PKG_BASE, IRepository<Reservation>
    {
        public PKG_RESERVATIONS(IConfiguration configuration) : base(configuration)
        {
        }

        public void AddEntity(Reservation entity)
        {
            throw new NotImplementedException();
        }

        public void UpdateEntity(int id, Reservation entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteEntity(int id)
        {
            throw new NotImplementedException();
        }

        public Reservation GetEntity(int id)
        {
            throw new NotImplementedException();
        }

        public List<Reservation> GetEntities()
        {
            throw new NotImplementedException();
        }
    }
}