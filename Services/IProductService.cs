using ProvaPub.Models;

namespace ProvaPub.Services
{
    public interface IProductService
    {
        public ProductList ListProducts(int page);
    }
}
