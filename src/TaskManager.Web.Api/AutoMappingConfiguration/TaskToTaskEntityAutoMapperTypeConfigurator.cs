using AutoMapper;
using TaskManager.Common.TypeMapping;
using TaskManager.Web.Api.Models;

namespace TaskManager.Web.Api.AutoMappingConfiguration
{
    public class TaskToTaskEntityAutoMapperTypeConfigurator : IAutoMapperTypeConfigurator
    {
        public void Configure()
        {
            Mapper.CreateMap<Task, Data.Entities.Task>()
                .ForMember(opt => opt.Version, x => x.Ignore())
                .ForMember(opt => opt.CreatedBy, x => x.Ignore())
                .ForMember(opt => opt.Users, x => x.MapFrom(t => t.Assignees));
        }
    }
}