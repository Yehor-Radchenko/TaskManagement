namespace TaskManagement.Common.Profile;

using AutoMapper;
using TaskManagement.Common.Dto.User;
using TaskManagement.DAL.Models;
using TaskManagement.BLL.Services;
using TaskManagement.Common.Dto.Task;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        this.CreateMap<UserRegistrationDto, User>()
            .ForMember(dest => dest.Tasks, opt => opt.Ignore());

        this.CreateMap<TaskDto, Task>();
    }
}