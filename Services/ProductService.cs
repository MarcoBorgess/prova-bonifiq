using ProvaPub.Models;
using ProvaPub.Repository;

namespace ProvaPub.Services
{
	public class ProductService : IProductService
	{
        BaseRepository<Product> _productRepository;

        public ProductService(BaseRepository<Product> productRepository)
		{
			_productRepository = productRepository;
		}

		public ProductList ListProducts(int page)
		{
			List<Product> products = _productRepository.Get(page).ToList();
			bool hasNext = _productRepository.HasNext(page);

			return new ProductList()
			{
				HasNext = hasNext,
				TotalCount = products.Count,
				Products = products
			};
		}

	}
}
