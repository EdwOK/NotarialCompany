using System;
using AutoMapper;
using NotarialCompany.Models;

namespace NotarialCompany.Configuration
{
    public static class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new UserProfile());
                cfg.AddProfile(new ServicesProfile());
            });
        }
    }

    public class UserProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<object[], User>()
                .ForMember(u => u.Id, opts => opts.MapFrom(src => (int) src[0]))
                .ForMember(u => u.Username, opts => opts.MapFrom(src => (string) src[1]))
                .ForMember(u => u.RoleId, opts => opts.MapFrom(src => (int) src[4]))
                .ForMember(u => u.EmployeeId, opts => opts.MapFrom(src => (int) src[5]))
                .ForMember(u => u.Role,
                    opts => opts.MapFrom(src => new Role {Id = (int) src[6], Name = (string) src[7]}))
                .ForMember(u => u.Employee, opts => opts.MapFrom(src =>
                    new Employee
                    {
                        Id = (int) src[8],
                        FirstName = (string) src[9],
                        LastName = (string) src[10],
                        MiddleName = (string) src[11],
                        Address = (string) src[12],
                        PhoneNumber = (string) src[13],
                        EmploymentDate = (DateTime) src[14],
                        EmployeesPositionId = (int) src[15],
                        EmployeesPosition = new EmployeesPosition
                        {
                            Id = (int) src[16],
                            Postition = (string) src[17],
                            Salary = (decimal) src[18],
                            Commission = (int) src[19]
                        }
                    }));
        }
    }

    public class ServicesProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<object[], Service>()
                .ForMember(u => u.Id, opts => opts.MapFrom(src => (int) src[0]))
                .ForMember(u => u.Name, opts => opts.MapFrom(src => (string) src[1]))
                .ForMember(u => u.Description, opts => opts.MapFrom(src => (string) src[2]))
                .ForMember(u => u.Cost, opts => opts.MapFrom(src => (decimal) src[3]));
        }
    }
}
