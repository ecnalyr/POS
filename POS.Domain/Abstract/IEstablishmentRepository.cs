﻿using System;
using System.Linq;
using POS.Domain.Entities;

namespace POS.Domain.Abstract
{
    public interface IEstablishmentRepository : IDisposable
    {
        #region Public Properties

        IQueryable<Establishment> Establishments { get; }

        #endregion

        #region Public Methods and Operators

        void DeleteEstablishment(Establishment establishment);

        void SaveEstablishment(Establishment establishment);

        #endregion
    }
}