using System;
using Course.Core.Entities;
using Course.Data;
using Course.Service.Interfaces;
using Course.Service.Dtos.StudentDtos;
using Course.Service.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Course.Service.Implementations
{
	public class StudentService:IStudentService
	{
        private readonly AppDbContext _context;

        public StudentService(AppDbContext context)
        {
            _context = context;

        }
        public int Create(StudentCreateDto createDto)
        {
            Group group = _context.Groups.Include(x=>x.Students).FirstOrDefault(x => x.Id == createDto.GroupId);
            if (group == null)
            {
                throw new EntityNotFoundException();
            }

            if (group.Limit <= group.Students.Count)
            {
                throw new DublicateEntityException();
            }

            Student student = new Student
            {
                FullName = createDto.FullName,
                Email = createDto.Email,
                BirthDate = createDto.Birthdate,
                GroupId = createDto.GroupId
            };

            _context.Students.Add(student);
            _context.SaveChanges();

            return student.Id;
        }

        public void Delete(int id)
        {
            var studentToDelete = _context.Students.Find(id);
            if (studentToDelete == null)
            {
                throw new EntityNotFoundException();

            }

            var group = _context.Groups.FirstOrDefault(c => c.Id == studentToDelete.GroupId);
            if (group != null)
            {
                group.Students.Remove(studentToDelete);
                _context.Groups.Update(group);
            }
            _context.Students.Remove(studentToDelete);
            _context.SaveChanges();
           
        }

        public List<StudentGetDto> GetAll()
        {
            
            var data = _context.Students
               
                .Select(x => new StudentGetDto
                {
                    FullName = x.FullName,
                    Email = x.Email,
                    GroupName = x.Group.No,
                    Birthdate = x.BirthDate
                })
                .ToList();

            return data;
           
        }

        public StudentDetailsDto GetById(int id)
        {
            var data = _context.Students.Include(s => s.Group)
                        .FirstOrDefault(s => s.Id == id);
            if (data == null)
            {
                throw new EntityNotFoundException();
            }

            StudentDetailsDto studentDetailsDto = new StudentDetailsDto()
            {
                FullName = data.FullName,
                Email = data.Email,
                Birthdate = data.BirthDate,
                GroupName = data.Group.No
            };

            return studentDetailsDto;
        }

        public void Update(int id, StudentUpdateDto studentUpdate)
        {
            var existingStudent = _context.Students.Find(id);
            if (existingStudent == null)
            {
                throw new EntityNotFoundException();
            }

            Group group = _context.Groups.Include(x=>x.Students).FirstOrDefault(x => x.Id == studentUpdate.GroupId);
            if (group == null)
            {
                throw new EntityNotFoundException();
            }


            if (group.Limit <= group.Students.Count)
            {
                throw new DublicateEntityException();
            }

            existingStudent.FullName = studentUpdate.FullName;
            existingStudent.Email = studentUpdate.Email;
            existingStudent.BirthDate = studentUpdate.Birthdate;
            existingStudent.GroupId = studentUpdate.GroupId;
            existingStudent.ModifiedAt = DateTime.Now;

            _context.Students.Update(existingStudent);
            _context.SaveChanges();

            
        }

      
    }
}

