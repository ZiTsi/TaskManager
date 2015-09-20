using AutoMapper;
using TaskManager.Common.TypeMapping;
using TaskManager.Web.Api.Models;
using Task = TaskManager.Data.Entities.Task;

namespace TaskManager.Web.Api.AutoMappingConfiguration
{
    public class NewTaskV2ToTaskEntityAutoMapperTypeConfigurator : IAutoMapperTypeConfigurator
    {
        public void Configure()
        {
            Mapper.CreateMap<NewTaskV2, Task>()
                .ForMember(opt => opt.Version, x => x.Ignore())
                .ForMember(opt => opt.CreatedBy, x => x.Ignore())
                .ForMember(opt => opt.TaskId, x => x.Ignore())
                .ForMember(opt => opt.CreatedDate, x => x.Ignore())
                .ForMember(opt => opt.CompletedDate, x => x.Ignore())
                .ForMember(opt => opt.Status, x => x.Ignore())
                .ForMember(opt => opt.Users,
                    x => x.ResolveUsing(newTask => newTask.Assignee == null ? null : new[] { newTask.Assignee }));
        }
    }
}