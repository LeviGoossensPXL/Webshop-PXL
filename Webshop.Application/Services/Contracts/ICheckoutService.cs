using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Domain.Entities;

namespace Webshop.Application.Services.Contracts
{
    public interface ICheckoutService
    {
        Task<int> CreateOrderAndConfirmAsync(Address deliveryAddress, string userId);
    }
}
