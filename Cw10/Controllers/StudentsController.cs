﻿using System.Threading.Tasks;
using Cw10.Dto;
using Cw10.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cw10.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentDbService studentDbService;

        public StudentsController(IStudentDbService studentDbService)
        {
            this.studentDbService = studentDbService;
        }

        [HttpGet]
        public async Task<IActionResult> GeStudents()
        {
            return Ok(await studentDbService.GetStudents());
        }

        [HttpPost]
        public IActionResult CreateStudent(StudentDto studentDto)
        {
            return Ok(studentDto);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PutStudent([FromRoute] string id, StudentDto studentDto)
        {
            await studentDbService.Update(studentDto,id);
            return Ok("Ąktualizacja dokończona");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] string id)
        {
            await studentDbService.Delete(id);
            return Ok("Usuwanie ukończone");
        }
    }
}
