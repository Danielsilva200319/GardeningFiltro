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
    public class ProductoController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductoController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> Get()
        {
            var producto = await _unitOfWork.Productos.GetAllAsync();
            return _mapper.Map<List<ProductoDto>>(producto);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductoDto>> Get(string id)
        {
            var productos = await _unitOfWork.Productos.GetByIdAsync(int.Parse(id));
            if (productos == null)
            {
                return NotFound();
            }
            return _mapper.Map<ProductoDto>(productos);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<ProductoDto>> Post(ProductoDto productoDto)
        {
            var producto = _mapper.Map<Producto>(productoDto);
            _unitOfWork.Productos.Add(producto);
            await _unitOfWork.SaveAsync();
            if (producto == null)
            {
                return BadRequest();
            }
            productoDto.CodigoProducto = producto.CodigoProducto;
            return CreatedAtAction(nameof(Post), new {id = productoDto.CodigoProducto}, productoDto);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<ProductoDto>> Put(string id, ProductoDto productoDto)
        {
            if (productoDto.CodigoProducto == "0"){
                productoDto.CodigoProducto = id;
            }
            if (productoDto.CodigoProducto != id){
                return BadRequest();
            }
            if (productoDto == null){
                return NotFound();
            }
            var productos = _mapper.Map<Producto>(productoDto);
            _unitOfWork.Productos.Update(productos);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<ProductoDto>(productos);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            var producto = await _unitOfWork.Productos.GetByIdAsync(int.Parse(id));
            if (producto == null)
            {
                return NotFound();
            }
            _unitOfWork.Productos.Remove(producto);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}