using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class DetallePedidoController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DetallePedidoController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<DetallePedidoDto>>> Get()
        {
            var detallePedido = await _unitOfWork.DetallePedidos.GetAllAsync();
            return _mapper.Map<List<DetallePedidoDto>>(detallePedido);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DetallePedidoDto>> Get(int id)
        {
            var detallePedidos = await _unitOfWork.DetallePedidos.GetByIdAsync(id);
            if (detallePedidos == null)
            {
                return NotFound();
            }
            return _mapper.Map<DetallePedidoDto>(detallePedidos);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<DetallePedidoDto>> Post(DetallePedidoDto detallePedidoDto)
        {
            var detallePedido = _mapper.Map<DetallePedido>(detallePedidoDto);
            _unitOfWork.DetallePedidos.Add(detallePedido);
            await _unitOfWork.SaveAsync();
            if (detallePedido == null)
            {
                return BadRequest();
            }
            detallePedidoDto.CodigoPedido = detallePedido.CodigoPedido;
            return CreatedAtAction(nameof(Post), new {id = detallePedidoDto.CodigoPedido}, detallePedidoDto);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<DetallePedidoDto>> Put(int id, DetallePedidoDto detallePedidoDto)
        {
            if (detallePedidoDto.CodigoPedido == 0){
                detallePedidoDto.CodigoPedido = id;
            }
            if (detallePedidoDto.CodigoPedido != id){
                return BadRequest();
            }
            if (detallePedidoDto == null){
                return NotFound();
            }
            var detallePedidos = _mapper.Map<DetallePedido>(detallePedidoDto);
            _unitOfWork.DetallePedidos.Update(detallePedidos);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<DetallePedidoDto>(detallePedidos);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var detallePedido = await _unitOfWork.DetallePedidos.GetByIdAsync(id);
            if (detallePedido == null)
            {
                return NotFound();
            }
            _unitOfWork.DetallePedidos.Remove(detallePedido);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}