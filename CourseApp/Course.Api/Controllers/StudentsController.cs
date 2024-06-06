using System;
using Course.Service.Dtos.GroupDtos;
using Course.Service.Dtos.StudentDtos;
using Course.Service.Exceptions;
using Course.Service.Implementations;
using Course.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Course.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController:ControllerBase
	{
		private readonly IStudentService _studentService;

		public StudentsController(IStudentService studentService)
		{
			_studentService = studentService;
		}

        [HttpPost("")]
        public ActionResult Create(StudentCreateDto createDto)
        {
            try
            {
                return StatusCode(201, new { id = _studentService.Create(createDto) });
            }   
            catch(EntityNotFoundException e)
            {
                return NotFound();
            }
            catch (DublicateEntityException e)
            {
                return Conflict();
            }
            catch (Exception e)
            {
                return StatusCode(500, "Unknown Error!");
            }
        }

        [HttpGet("")]
        public ActionResult<List<StudentGetDto>> GetAll()
        {
            return Ok(_studentService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<StudentDetailsDto> GetById(int id)
        {
            try
            {
                return Ok(_studentService.GetById(id));
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(500, "An unknown error occurred!");
            }
        }


        [HttpPut("{id}")]
        public IActionResult Update(int id, StudentUpdateDto updateDto)
        {
            try
            {
                _studentService.Update(id, updateDto);
                return NoContent();
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            catch (DublicateEntityException)
            {
                return Conflict();
            }
            catch (Exception)
            {
                return StatusCode(500, "An unknown error occurred!");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _studentService.Delete(id);
                return NoContent();
            }
            catch (EntityNotFoundException e)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, "An unknown error occurred!!");
            }
        }

    }
}

