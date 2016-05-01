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
                cfg.AddProfile(new RolesProfile());
                cfg.AddProfile(new EmployeesPositionProfile());
                cfg.AddProfile(new EmployeesProfile());
                cfg.AddProfile(new ClientsProfile());
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
                .ForMember(u => u.Password, opts => opts.MapFrom(src => (string) src[2]))
                .ForMember(u => u.Salt, opts => opts.MapFrom(src => (string) src[3]))
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
                            Position = (string) src[17],
                            Salary = (decimal) src[18],
                            Commission = (int) src[19]
                        }
                    }))
                .ReverseMap()
                .ConstructUsing(x => new object[] { x.Id, x.Username, x.Password, x.Salt, x.RoleId, x.EmployeeId });
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
                .ForMember(u => u.Cost, opts => opts.MapFrom(src => (decimal) src[3]))
                .ReverseMap()
                .ConstructUsing(x => new object[] {x.Id, x.Name, x.Description, x.Cost});
        }
    }

    public class ClientsProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<object[], Client>()
                .ForMember(u => u.Id, opts => opts.MapFrom(src => (int) src[0]))
                .ForMember(u => u.FirstName, opts => opts.MapFrom(src => (string) src[1]))
                .ForMember(u => u.SecondName, opts => opts.MapFrom(src => (string) src[2]))
                .ForMember(u => u.MiddleName, opts => opts.MapFrom(src => (string) src[3]))
                .ForMember(u => u.Occupation, opts => opts.MapFrom(src => (string) src[4]))
                .ForMember(u => u.Address, opts => opts.MapFrom(src => (string) src[5]))
                .ForMember(u => u.PhoneNumber, opts => opts.MapFrom(src => (string) src[6]))
                .ReverseMap()
                .ConstructUsing(
                    x =>
                        new object[]
                        {
                            x.Id,
                            x.FirstName,
                            x.SecondName,
                            x.MiddleName,
                            x.Occupation,
                            x.Address,
                            x.PhoneNumber
                        });
        }
    }

    public class RolesProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<object[], Role>()
                .ForMember(u => u.Id, opts => opts.MapFrom(src => (int)src[0]))
                .ForMember(u => u.Name, opts => opts.MapFrom(src => (string)src[1]))
                .ReverseMap()
                .ConstructUsing(x => new object[] { x.Id, x.Name });
        }
    }

    public class EmployeesPositionProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<object[], EmployeesPosition>()
                .ForMember(u => u.Id, opts => opts.MapFrom(src => (int)src[0]))
                .ForMember(u => u.Position, opts => opts.MapFrom(src => (string)src[1]))
                .ForMember(u => u.Salary, opts => opts.MapFrom(src => (decimal)src[2]))
                .ForMember(u => u.Commission, opts => opts.MapFrom(src => (int)src[3]))
                .ReverseMap()
                .ConstructUsing(x => new object[] { x.Id, x.Position, x.Salary, x.Commission });
        }
    }

    public class EmployeesProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<object[], Employee>()
                .ForMember(u => u.Id, opts => opts.MapFrom(src => (int) src[0]))
                .ForMember(u => u.FirstName, opts => opts.MapFrom(src => (string) src[1]))
                .ForMember(u => u.LastName, opts => opts.MapFrom(src => (string) src[2]))
                .ForMember(u => u.MiddleName, opts => opts.MapFrom(src => (string) src[3]))
                .ForMember(u => u.Address, opts => opts.MapFrom(src => (string) src[4]))
                .ForMember(u => u.PhoneNumber, opts => opts.MapFrom(src => (string) src[5]))
                .ForMember(u => u.EmploymentDate, opts => opts.MapFrom(src => (DateTime) src[6]))
                .ForMember(u => u.EmployeesPositionId, opts => opts.MapFrom(src => (int) src[7]))
                .ForMember(u => u.EmployeesPosition, opts => opts.MapFrom(src =>
                    new EmployeesPosition
                    {
                        Id = (int) src[8],
                        Position = (string) src[9],
                        Salary = (decimal) src[10],
                        Commission = (int) src[11]
                    }))
                .ReverseMap()
                .ConstructUsing(
                    x =>
                        new object[]
                        {
                            x.Id,
                            x.FirstName,
                            x.LastName,
                            x.MiddleName,
                            x.Address,
                            x.PhoneNumber,
                            x.EmploymentDate,
                            x.EmployeesPositionId
                        });
        }
    }
}
