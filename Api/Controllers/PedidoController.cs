using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Dtos;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class PedidoController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PedidoController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<PedidoDto>>> Get()
        {
            var pedido = await _unitOfWork.Pedidos.GetAllAsync();
            return _mapper.Map<List<PedidoDto>>(pedido);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PedidoDto>> Get(int id)
        {
            var pedidos = await _unitOfWork.Pedidos.GetByIdAsync(id);
            if (pedidos == null)
            {
                return NotFound();
            }
            return _mapper.Map<PedidoDto>(pedidos);
        }
        /* [HttpGet("Listado Pedidos Rechazados/{id}")]
        public async Task<List<object>> GetConsulta1()
        {
            var consulta = await _unitOfWork.Pedidos.getConsulta1().ConfigureAwait(false);
            return Ok(consulta);
        } */
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<PedidoDto>> Post(PedidoDto pedidoDto)
        {
            var pedido = _mapper.Map<Pedido>(pedidoDto);
            if (pedido.FechaPedido == DateOnly.MinValue)
            {
                pedidoDto.FechaPedido = DateOnly.FromDateTime(DateTime.Now);
                pedido.FechaPedido = DateOnly.FromDateTime(DateTime.Now);
            }
            if (pedido.FechaEsperada == DateOnly.MinValue)
            {
                pedidoDto.FechaEsperada = DateOnly.FromDateTime(DateTime.Now);
                pedido.FechaEsperada = DateOnly.FromDateTime(DateTime.Now);
            }
            if (pedido.FechaEntrega == DateOnly.MinValue)
            {
                pedidoDto.FechaEntrega = DateOnly.FromDateTime(DateTime.Now);
                pedido.FechaEntrega = DateOnly.FromDateTime(DateTime.Now);
            }
            _unitOfWork.Pedidos.Add(pedido);
            await _unitOfWork.SaveAsync();
            if (pedido == null)
            {
                return BadRequest();
            }
            pedidoDto.CodigoPedido = pedido.CodigoPedido;
            return CreatedAtAction(nameof(Post), new {id = pedidoDto.CodigoPedido}, pedidoDto);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<PedidoDto>> Put(int id, PedidoDto pedidoDto)
        {
            if (pedidoDto.CodigoPedido == 0){
                pedidoDto.CodigoPedido = id;
            }
            if (pedidoDto.CodigoPedido != id){
                return BadRequest();
            }
            if (pedidoDto == null){
                return NotFound();
            }
            var pedidos = _mapper.Map<Pedido>(pedidoDto);
            _unitOfWork.Pedidos.Update(pedidos);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<PedidoDto>(pedidos);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var pedido = await _unitOfWork.Pedidos.GetByIdAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            _unitOfWork.Pedidos.Remove(pedido);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}