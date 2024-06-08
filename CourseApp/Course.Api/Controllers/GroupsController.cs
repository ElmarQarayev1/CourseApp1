using System;
using Course.Service.Dtos.GroupDtos;
using Course.Service.Exceptions;
using Course.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Course.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController:ControllerBase
	{
        private readonly IGroupService _groupService;

        public GroupsController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpPost("")]
        public ActionResult Create(GroupCreateDto createDto)
        {
           
                return StatusCode(201, new { id = _groupService.Create(createDto) });
            
        }
        [HttpGet("")]
        public ActionResult<List<GroupGetDto>> GetAll()
        {
            var result = _groupService.GetAll();
            Log.Information("All Info =>{@result}", result);
            return Ok(result);
           
        }


        [HttpGet("{id}")]
        public ActionResult<GroupDetailsDto> GetById(int id)
        {
            try
            {
                return Ok(_groupService.GetById(id));
            }

            catch (EntityNotFoundException e)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, "An unknown error occurred.");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, GroupUpdateDto updateDto)
        {
            
           
                _groupService.Update(id, updateDto);
                return NoContent();
           
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _groupService.Delete(id);
                return NoContent();
            }
            catch (EntityNotFoundException e)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, "An unknown error occurred.");
            }
        }
    

}
}

