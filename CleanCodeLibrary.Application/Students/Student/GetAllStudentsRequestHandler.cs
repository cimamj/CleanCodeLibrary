using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.Persistance.Students;
using CleanCodeLibrary.Domain.Entities.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CleanCodeLibrary.Domain.Common.Validation.ValidationItems.ValidationItems;
using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.DTOs.Students;
using CleanCodeLibrary.Domain.Common.Validation;

namespace CleanCodeLibrary.Application.Students.Student
{
    public class GetAllStudentsRequest
    {
        //ne triba jer nema parametara
    }

    //public class StudentDto //ovo koristim u repozitoriju studentovome
    //{
    //    public int Id { get; set; }
    //    public string FirstName { get; set; } = string.Empty;
    //    public string LastName { get; set; } = string.Empty;
    //    public DateOnly? DateOfBirth { get; set; }
    //}

    //public class GetAllStudentsResponse //zasto kad stavim static se Students crveni? nemos imat instace npr students
    //{
    //    public IEnumerable<StudentDto> Students { get; set; } //pod ienumerable spremam listu ocito jbt
    //    public GetAllStudentsResponse(IEnumerable<StudentDto> students)
    //    {
    //        Students = students ?? Enumerable.Empty<StudentDto>(); //jel bolje ovo jer moze null poslat nekom greskom
    //    }

    //    public GetAllStudentsResponse() { }

    //}
    
    public class GetAllStudentsRequestHandler : RequestHandler<GetAllStudentsRequest, GetAllResponse<StudentDto>>
    {
        private readonly IStudentRepository _studentRepository;

        public GetAllStudentsRequestHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        protected async override Task<Result<GetAllResponse<StudentDto>>> HandleRequest(
            GetAllStudentsRequest request,
            Result<GetAllResponse<StudentDto>> result
            )
        {
         
            //var domainResult = await CleanCodeLibrary.Domain.Entities.Students.Student.GetAllStudentsAsync(_studentRepository); //kad stavim samo static tu metodu nemogu , mora cila klasa za metodu korsitit?
            //result.SetValidationResult(domainResult.ValidationResult);
            //if (result.HasWarning) //nemoze imat erroor nego warning
            //{
            //    return result;
            //}
            //ode nema potrebe SAVE CHANGES ZVAT, jer se nista nije promoinilo
            //var studentDtos = domainResult.Value.Values.Select(s => new StudentDto //value iz result i values iz getallresponse, moze i foreach pa add u listu
            //{
            //    Id = s.Id,
            //    FirstName = s.FirstName,
            //    LastName = s.LastName,
            //    DateOfBirth = s.DateOfBirth
            //}); //.ToList(); jel vracam listu ili ienumerable

            var students = await _studentRepository.GetAllStudentDtos();

            if(students.Values.Count() == 0)
            {
                result.AddWarning(new ValidationResultItem
                {
                    Message = "Nema nijednog studenta u bazi",
                    ValidationSeverity = ValidationSeverity.Warning,
                });
            }

            result.SetResult(students);

            return result; //jeben ti zivot

        }

        protected override Task<bool> IsAuthorized() //sve abstraktne impementiraj prvo
        {
            return Task.FromResult(true);   //valjda ovak
        } 
    }
}
