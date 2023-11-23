using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Persistence.Data;

namespace Application.Repositories
{
    public class ClienteRepository : GenericRepository<Cliente>, ICliente
    {
        private readonly GardenContext _context;

        public ClienteRepository(GardenContext context) : base(context)
        {
            _context = context;
        }
        /* public Task<IQueryable<string>> getConsulta8()
        {
            var consulta = from cliente in _context.Clientes
                            join pedidos in _context.Pedidos
        } */
    }
}