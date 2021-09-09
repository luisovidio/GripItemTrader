using GripItemTrader.Core.Dto;
using GripItemTrader.Core.Entities;
using GripItemTrader.Core.Exceptions;
using GripItemTrader.Core.Interfaces;
using GripItemTrader.Core.Interfaces.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GripItemTrader.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonUseCases _personUseCases;

        public PersonController(IPersonUseCases personUseCases)
        {
            _personUseCases = personUseCases;
        }

        /// <summary>
        /// Get all people
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllPeopleAsync()
        {
            var result = await _personUseCases.GetAllPeopleAsync();
            return Ok(result);
        }

        /// <summary>
        /// Get a person by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonByIdAsync(int id)
        {
            try
            {
                var result = await _personUseCases.GetPersonByIdAsync(id);
                return Ok(result);
            }
            catch (GripItemTraderException ex)
            {
                return BadRequest(GripItemTraderErrorDto.BuildErrorResponse(ex));
            }
        }

        /// <summary>
        /// Insert a new person
        /// </summary>
        /// <param name="person">Person to be inserted</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostPersonAsync(string personName)
        {
            try
            {
                await _personUseCases.InsertPersonAsync(personName);
                return Ok();
            }
            catch (GripItemTraderException ex)
            {
                return BadRequest(GripItemTraderErrorDto.BuildErrorResponse(ex));
            }
        }

        /// <summary>
        /// Update the given person
        /// </summary>
        /// <param name="person">Person to be updated</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> PutPersonAsync(PersonUpdateRequestDto personUpdateRequest)
        {
            try
            {
                await _personUseCases.UpdatePersonAsync(personUpdateRequest);
                return Ok();
            }
            catch (GripItemTraderException ex)
            {
                return BadRequest(GripItemTraderErrorDto.BuildErrorResponse(ex));
            }
        }

        /// <summary>
        /// Soft delete a person
        /// </summary>
        /// <param name="personId">Id from the person to be deleted</param>
        /// <returns></returns>
        [HttpDelete("personId")]
        public async Task<IActionResult> DeletePersonAsync(int personId)
        {
            try
            {
                await _personUseCases.DeletePersonAsync(personId);
                return Ok();
            }
            catch (GripItemTraderException ex)
            {
                return BadRequest(GripItemTraderErrorDto.BuildErrorResponse(ex));
            }
        }
    }
}
