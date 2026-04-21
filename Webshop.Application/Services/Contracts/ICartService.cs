using System.Threading.Tasks;
using Webshop.Domain.Entities;

namespace Webshop.Application.Services.Contracts
{
    public interface ICartService
    {
        ShoppingCart GetCart();
        Task AddToCartAsync(int productId);
        void RemoveFromCart(int productId);
        void ClearCart();
        void UpdateQuantity(int productId, int change);
    }
}