using ProvaPub.Models;

namespace ProvaPub.Services
{
	public class OrderService
	{
		public async Task<Order> PayOrder(string paymentMethod, decimal paymentValue, int customerId)
		{
			switch (paymentMethod)
			{
				case "pix":
					//Faz pagamento...
					break;
				case "creditcard":
					//Faz pagamento...
					break;
				case "paypal":
					//Faz pagamento...
					break;
				default:
					break;
            }

			return await Task.FromResult( new Order()
			{
				Value = paymentValue
			});
		}
	}
}
