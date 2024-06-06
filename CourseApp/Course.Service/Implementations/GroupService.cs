using System;
using Course.Core.Entities;
using Course.Core.Helpers;
using Course.Data;
using Course.Service.Dtos.GroupDtos;
using Course.Service.Exceptions;
using Course.Service.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Course.Service.Implementations
{
    public class GroupService : IGroupService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public GroupService(AppDbContext context, IWebHostEnvironment env )
        {
            _context = context;
            _env = env;
        }
        public int Create(GroupCreateDto dto)
        {
            if (_context.Groups.Any(x => x.No == dto.No))
                throw new DublicateEntityException();

            Group entity = new Group
            {
                No = dto.No,
                Limit = dto.Limit
            };
            if (dto.ImageFile != null)
            {
                var savedFilePath = FileManager.Save(dto.ImageFile, _env.WebRootPath, "uploads/group");
                entity.Img = savedFilePath;
            }
            _context.Groups.Add(entity);
            _context.SaveChanges();
            return entity.Id;
        }
        public List<GroupGetDto> GetAll()
        {
            return _context.Groups.Where(x => !x.IsDeleted).Select(x => new GroupGetDto
            {
                Id = x.Id,
                No = x.No,
               Limit=x.Limit,
                StudentCount = x.Students.Count
            }).ToList();
        }
        public GroupDetailsDto GetById(int id)
        {
            var group = _context.Groups.Include(g => g.Students)
                .FirstOrDefault(x => x.Id == id);

            if (group == null)
                throw new EntityNotFoundException();

            return new GroupDetailsDto
            {
                Id = group.Id,
                No = group.No,
                Limit = group.Limit,
                StudentCount = group.Students.Count
            };
        }
        public void Update(int id, GroupUpdateDto updateDto)
        {
            var group = _context.Groups.FirstOrDefault(x => x.Id == id && !x.IsDeleted);  

            if (group == null)
                throw new EntityNotFoundException();

            if (group.No != updateDto.No && _context.Groups.Any(x => x.No == updateDto.No && !x.IsDeleted)) 
                throw new DublicateEntityException();

            group.No = updateDto.No;
            group.Limit = updateDto.Limit;
            group.ModifiedAt = DateTime.Now;

            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var group = _context.Groups.Include(g => g.Students)
                .FirstOrDefault(x => x.Id == id);

            if (group == null)
                throw new EntityNotFoundException();

            group.IsDeleted = true;
            _context.Groups.Update(group);
            _context.SaveChanges();
        }

       
    }
}

