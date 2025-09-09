using AutoMapper;
using ERPSystem.Application.DTOs.Department;
using ERPSystem.Application.DTOs.Employee;
using ERPSystem.Domain.Entities;
using ERPSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.Mapping
{
    public class HRMappingProfile : Profile
    {
        public HRMappingProfile()
        {
            CreateMap<Department, GetDepartmentDTO>();


            CreateMap<DepartmentDTO, Department>();



            /* Employee Mapping*/

            CreateMap<Employee, EmployeeDTO>();

            CreateMap<Employee, GetEmployeesDTO>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()));

            CreateMap<EmployeeDTO, Employee>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => Enum.Parse<Gender>(src.Gender, true)));

        }
    }
}
