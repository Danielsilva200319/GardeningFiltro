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
    public class GamaProductoController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GamaProductoController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<GamaProductoDto>>> Get()
        {
            var gamaProducto = await _unitOfWork.Clientes.GetAllAsync();
            return _mapper.Map<List<GamaProductoDto>>(gamaProducto);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GamaProductoDto>> Get(string id)
        {
            var gamaProductos = await _unitOfWork.GamaProductos.GetByIdAsync(int.Parse(id));
            if (gamaProductos == null)
            {
                return NotFound();
            }
            return _mapper.Map<GamaProductoDto>(gamaProductos);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<GamaProductoDto>> Post(GamaProductoDto gamaProductoDto)
        {
            var gamaProducto = _mapper.Map<GamaProducto>(gamaProductoDto);
            _unitOfWork.GamaProductos.Add(gamaProducto);
            await _unitOfWork.SaveAsync();
            if (gamaProducto == null)
            {
                return BadRequest();
            }
            gamaProductoDto.Gama = gamaProductoDto.Gama;
            return CreatedAtAction(nameof(Post), new {id = gamaProductoDto.Gama}, gamaProductoDto);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<GamaProductoDto>> Put(string id, GamaProductoDto gamaProductoDto)
        {
            if (gamaProductoDto.Gama == "0"){
                gamaProductoDto.Gama = id;
            }
            if (gamaProductoDto.Gama != id){
                return BadRequest();
            }
            if (gamaProductoDto == null){
                return NotFound();
            }
            var gamaProductos = _mapper.Map<GamaProducto>(gamaProductoDto);
            _unitOfWork.GamaProductos.Update(gamaProductos);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<GamaProductoDto>(gamaProductos);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            var gamaProducto = await _unitOfWork.GamaProductos.GetByIdAsync(int.Parse(id));
            if (gamaProducto == null)
            {
                return NotFound();
            }
            _unitOfWork.GamaProductos.Remove(gamaProducto);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}