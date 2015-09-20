using AutoMapper;
using TaskManager.Common.TypeMapping;
using TaskManager.Data.Entities;

namespace TaskManager.Web.Api.AutoMappingConfiguration
{
    public class TaskEntityToTaskAutoMapperTypeConfigurator : IAutoMapperTypeConfigurator
    {
        public void Configure()
        {
            Mapper.CreateMap<Task, Models.Task>()
            .ForMember(opt => opt.Links, x => x.Ignore())
            .ForMember(opt => opt.Assignees, x => x.ResolveUsing<TaskAssigneesResolver>());
        }
    }
}