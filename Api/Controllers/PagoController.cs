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
    public class PagoController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PagoController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<PagoDto>>> Get()
        {
            var pago = await _unitOfWork.Pagos.GetAllAsync();
            return _mapper.Map<List<PagoDto>>(pago);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PagoDto>> Get(int id)
        {
            var pagos = await _unitOfWork.Pagos.GetByIdAsync(id);
            if (pagos == null)
            {
                return NotFound();
            }
            return _mapper.Map<PagoDto>(pagos);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<PagoDto>> Post(PagoDto pagoDto)
        {
            var pago = _mapper.Map<Pago>(pagoDto);
            if (pago.FechaPago == DateOnly.MinValue)
            {
                pagoDto.FechaPago = DateOnly.FromDateTime(DateTime.Now);
                pago.FechaPago = DateOnly.FromDateTime(DateTime.Now);
            }
            _unitOfWork.Pagos.Add(pago);
            await _unitOfWork.SaveAsync();
            if (pago == null)
            {
                return BadRequest();
            }
            pagoDto.CodigoCliente = pago.CodigoCliente;
            return CreatedAtAction(nameof(Post), new {id = pagoDto.CodigoCliente}, pagoDto);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<PagoDto>> Put(int id, PagoDto pagoDto)
        {
            if (pagoDto.CodigoCliente == 0){
                pagoDto.CodigoCliente = id;
            }
            if (pagoDto.CodigoCliente != id){
                return BadRequest();
            }
            if (pagoDto == null){
                return NotFound();
            }
            var pagos = _mapper.Map<Pago>(pagoDto);
            _unitOfWork.Pagos.Update(pagos);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<PagoDto>(pagos);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var pago = await _unitOfWork.Pagos.GetByIdAsync(id);
            if (pago == null)
            {
                return NotFound();
            }
            _unitOfWork.Pagos.Remove(pago);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}