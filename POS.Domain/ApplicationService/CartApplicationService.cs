using System;
using System.Collections.Generic;
using System.Linq;
using POS.Domain.Abstract;
using POS.Domain.Model;



namespace POS.Domain.ApplicationService
{
    public class CartApplicationService : ICartApplicationService
    {
        private IOrderProcessor orderRepository;

        public CartApplicationService(IOrderProcessor processor)
        {
            orderRepository = processor;
        }

        public void Process(Cart cart, ShippingDetails shippingDetails)
        {
            var order = ProcessTheOrder(cart, shippingDetails);
            orderRepository.CreateOrder(order);
            cart.Clear();
        }

        private Order ProcessTheOrder(Cart cart, ShippingDetails shippingDetails)
        {
            var order = new Order();
            var orderDetailsList = new List<OrderDetail>();

            try
            {
                foreach (var item in cart.Lines)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.OrderId,
                        ProductName = item.Product.Name,
                        Quantity = item.Quantity,
                        UnitPrice = item.Product.Price
                    };
                    order.TotalCost += orderDetail.UnitPrice*orderDetail.Quantity;
                    //TODO: Make Unit Test
                    orderDetailsList.Add(orderDetail);
                }
            }
            catch (Exception)
            {
                // TODO: Add HttpException handling in place of Exception below
                throw new Exception("Erorr building list of roder details -> cart.Lines was probably null.");
            }

            var firstCartLineProduct = cart.Lines.FirstOrDefault();
            int establishmentId = firstCartLineProduct.Product.EstablishmentId;

            order.OrderDetails = orderDetailsList;
            order.EstablishmentId = establishmentId;
            order.CustomerName = shippingDetails.Name;
            order.TimeProcessed = DateTime.Now;
            return order;
        }
    }
}
