using OrderService.Interfaces;
using OrderService.Model;
using OrderService.Repositories;

namespace OrderService.Business_Layer
{
    public class OrderRepositoryBL : IOrderRepositoryBL
    {
        private readonly IOrderInterface _orderRepo;
        public OrderRepositoryBL(IOrderInterface orderRepo)
        {
            _orderRepo = orderRepo;

        }

        public async Task<Orders> AddOrder(Orders orderrequest)
        {
            var addOrder = await _orderRepo.AddOrder(orderrequest);
            return addOrder;
        }

        public async Task DeleteOrder(int id)
        {
            await _orderRepo.DeleteOrder(id);
        }

        public async Task<List<Orders>> GetAllOrders()
        {
            var allOrder = await _orderRepo.GetAllOrders();
            return allOrder;
        }

        public async Task<List<Orders>> GetOrderById(int id)
        {
            var orderId = await _orderRepo.GetOrderById(id);
            return orderId;
        }

        public async Task<Orders> UpdateOrder(Orders orderupdate)
        {
            var updateOrder = await _orderRepo.UpdateOrder(orderupdate);
            return updateOrder;
        }
    }
}
