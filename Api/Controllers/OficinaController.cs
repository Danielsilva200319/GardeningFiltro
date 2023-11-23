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
    public class OficinaController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OficinaController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<OficinaDto>>> Get()
        {
            var oficina = await _unitOfWork.Oficinas.GetAllAsync();
            return _mapper.Map<List<OficinaDto>>(oficina);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OficinaDto>> Get(string id)
        {
            var oficinas = await _unitOfWork.Oficinas.GetByIdAsync(int.Parse(id));
            if (oficinas == null)
            {
                return NotFound();
            }
            return _mapper.Map<OficinaDto>(oficinas);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<OficinaDto>> Post(OficinaDto oficinaDto)
        {
            var oficina = _mapper.Map<Oficina>(oficinaDto);
            _unitOfWork.Oficinas.Add(oficina);
            await _unitOfWork.SaveAsync();
            if (oficina == null)
            {
                return BadRequest();
            }
            oficinaDto.CodigoOficina = oficina.CodigoOficina;
            return CreatedAtAction(nameof(Post), new {id = oficinaDto.CodigoOficina}, oficinaDto);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<OficinaDto>> Put(string id, OficinaDto oficinaDto)
        {
            if (oficinaDto.CodigoOficina == "0"){
                oficinaDto.CodigoOficina = id;
            }
            if (oficinaDto.CodigoOficina != id){
                return BadRequest();
            }
            if (oficinaDto == null){
                return NotFound();
            }
            var oficinas = _mapper.Map<Oficina>(oficinaDto);
            _unitOfWork.Oficinas.Update(oficinas);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<OficinaDto>(oficinas);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            var oficina = await _unitOfWork.Oficinas.GetByIdAsync(int.Parse(id));
            if (oficina == null)
            {
                return NotFound();
            }
            _unitOfWork.Oficinas.Remove(oficina);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}