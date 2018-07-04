using FluentNHibernate.Mapping;

namespace Logic.Students
{
    public class CourseMap : ClassMap<Course>
    {
        public CourseMap()
        {
            Id(x => x.Id);

            Map(x => x.Name);
            Map(x => x.Credits);
        }
    }
}
