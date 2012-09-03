namespace POS.Domain.Abstract
{
    using System;

    using POS.Domain.Model;

    public interface ICartApplicationService
    {

        #region Public Methods and Operators

        void Process(Cart cart, ShippingDetails shippingDetails);

        #endregion
    }
}
