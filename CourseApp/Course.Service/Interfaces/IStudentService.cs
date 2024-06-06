using System;
using Course.Service.Dtos.GroupDtos;
using Course.Service.Dtos.StudentDtos;

namespace Course.Service.Interfaces
{
	public interface IStudentService
	{
		void Update(int id, StudentUpdateDto studentUpdate);
		void Delete(int id);
		List<StudentGetDto> GetAll();
		StudentDetailsDto GetById(int id);
        int Create(StudentCreateDto createDto);
    }


}

