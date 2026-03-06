using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Domain.Entities;

namespace Webshop.Application.Repositories
{
    public interface IAppUserRepository
    {
        Task<IEnumerable<AppUser>> GetAll();
        Task<AppUser?> GetById(string id);
        Task Delete(string id);
    }
}
