﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using POS.Domain.Abstract;
using POS.Domain.Entities;

namespace POS.Domain.Concrete
{
    class EFLocationRepository : ILocationRepository
    {
        #region Fields

        private readonly EFDbContext context = new EFDbContext();

        #endregion

        #region Public Properties

        public IQueryable<Establishment> Establishments
        {
            get
            {
                return context.Establishments;
            }
        }

        #endregion

        public void DeleteEstablishment(Establishment establishment)
        {
            context.Establishments.Remove(establishment);
            context.SaveChanges();
        }

        public void SaveEstablishment(Establishment establishment)
        {
            if (establishment.EstablishmentId == 0)
            {
                context.Establishments.Add(establishment);
            }
            else
            {
                context.Entry(establishment).State = EntityState.Modified;
            }

            context.SaveChanges();
        }
    }
}
